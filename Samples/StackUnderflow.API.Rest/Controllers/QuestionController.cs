using Access.Primitives.EFCore;
using Access.Primitives.IO;
using Access.Primitives.Extensions.ObjectExtensions;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Orleans;
using GrainInterfaces;

using StackUnderflow.Domain.Core;
using StackUnderflow.Domain.Core.Contexts;
using StackUnderflow.Domain.Core.Contexts.Question;
using StackUnderflow.Domain.Core.Contexts.Question.ValidateQuestionOp;
using StackUnderflow.EF.Models;
using StackUnderflow.Domain.Schema;
using static StackUnderflow.Domain.Core.Contexts.Question.ValidateQuestionOp.ValidateQuestionResult;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using LanguageExt;
using StackUnderflow.Domain.Core.Contexts.Question.LanguageCheckOp;
using static StackUnderflow.Domain.Core.Contexts.Question.LanguageCheckOp.LanguageCheckResult;

namespace StackUnderflow.API.Rest.Controllers
{
    [Route("question")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private readonly IInterpreterAsync _interpreter;
        private readonly StackUnderflowContext _dbContext;
        private readonly IClusterClient _client;

        public QuestionController(IInterpreterAsync interpreter, StackUnderflowContext dbContext, IClusterClient client)
        {
            _interpreter = interpreter;
            _dbContext = dbContext;
            _client = client;
        }

        [HttpPost("question")]
        public async Task<IActionResult> CreateQuestionAndSendEmail([FromBody] ValidateQuestionCmd validateQuestionCmd)
        {
            QuestionWriteContext ctx = new QuestionWriteContext(new EFList<Post>(_dbContext.Post));

            var dependencies = new QuestionDependencies();

            var expr = from validateQuestionResult in QuestionDomain.ValidateQuestion(validateQuestionCmd)
                       let post = validateQuestionResult.SafeCast<QuestionValidated>().Select(p => p.Post)
                       let postResult = validateQuestionResult.SafeCast<QuestionValidated>().ToList().Single()
                       let languageCmd = new LanguageCheckCmd(postResult.Post.Title, postResult.Post.PostText)
                       from languageCheck in QuestionDomain.LanguageCheck(languageCmd)
                       select new { validateQuestionResult, languageCheck };
            
            var r = await _interpreter.Interpret(expr, ctx, dependencies);


            if (r.languageCheck.SafeCast<LanguageOK>().IsNone)
                return BadRequest("Bad Language");

            _dbContext.SaveChanges();
            return r.validateQuestionResult.Match(
                created => (IActionResult)Ok(created.Post.PostId),
                notCreated => StatusCode(StatusCodes.Status500InternalServerError, "Question could not be created."),
                invalidRequest => BadRequest("Invalid request."));
            
        }
    }
}
