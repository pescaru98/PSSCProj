using Access.Primitives.Extensions.ObjectExtensions;
using Access.Primitives.IO;
using Access.Primitives.IO.Mocking;

using StackUnderflow.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static StackUnderflow.Domain.Core.Contexts.Question.ValidateQuestionOp.ValidateQuestionResult;


namespace StackUnderflow.Domain.Core.Contexts.Question.ValidateQuestionOp
{
    public partial class ValidateQuestionAdapter : Adapter<ValidateQuestionCmd, IValidateQuestionResult, QuestionWriteContext, QuestionDependencies>
    {
        private readonly IExecutionContext _ex;

        public ValidateQuestionAdapter(IExecutionContext ex)
        {
            _ex = ex;
        }

        public override Task PostConditions(ValidateQuestionCmd cmd, IValidateQuestionResult result, QuestionWriteContext state)
        {
            return Task.CompletedTask;
        }

        public override async Task<IValidateQuestionResult> Work(ValidateQuestionCmd cmd, QuestionWriteContext state, QuestionDependencies dependencies)
        {
            var workflow = from valid in cmd.TryValidate()
                           let p = addQuestionIfMissing(state, CreatePostFromCommand(cmd), GetUserFromCmd(state,cmd))
                           select p;

            var result = await workflow.Match(
                Succ: r => r,
                Fail: ex => new InvalidRequest(ex.ToString()));

            return result;
        }

        public IValidateQuestionResult addQuestionIfMissing(QuestionWriteContext state, Post post, User user)
        {
            if (state.Posts.Any(p => String.Compare(p.Title,post.Title) == 0) == true)
                return new QuestionInvalidated("Duplicate Title");
            if (state.Posts.All(p => p.PostId != post.PostId))
                state.Posts.Add(post);

            return new QuestionValidated(post, user);
        }

        private Post CreatePostFromCommand(ValidateQuestionCmd cmd)
        {
            var post = new Post()
            {
                TenantId = cmd.TenantId,
                PostTypeId = cmd.PostType,
                Title = cmd.Title,
                PostText = cmd.Body,
                PostedBy = cmd.UserId
            };

            return post;
        }

        private User GetUserFromCmd(QuestionWriteContext state, ValidateQuestionCmd cmd)
        {
            IEnumerable<User> ReturnedUser = from user in state.Users
                                             where user.UserId == cmd.UserId
                                             select user;

            return ReturnedUser.First();
        }
    }
}
