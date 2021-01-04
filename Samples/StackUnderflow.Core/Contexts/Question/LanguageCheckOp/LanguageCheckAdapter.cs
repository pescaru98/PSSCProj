using Access.Primitives.Extensions.ObjectExtensions;
using Access.Primitives.IO;
using Access.Primitives.IO.Mocking;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using static StackUnderflow.Domain.Core.Contexts.Question.LanguageCheckOp.LanguageCheckResult;

namespace StackUnderflow.Domain.Core.Contexts.Question.LanguageCheckOp
{
    public partial class LanguageCheckAdapter : Adapter<LanguageCheckCmd, ILanguageCheckResult, QuestionWriteContext, QuestionDependencies>
    {
        private readonly IExecutionContext _ex;

        public LanguageCheckAdapter(IExecutionContext ex)
        {
            _ex = ex;
        }

        public override Task PostConditions(LanguageCheckCmd cmd, ILanguageCheckResult result, QuestionWriteContext state)
        {
            return Task.CompletedTask;
        }

        public override async Task<ILanguageCheckResult> Work(LanguageCheckCmd cmd, QuestionWriteContext state, QuestionDependencies dependencies)
        {
            var workflow = from valid in cmd.TryValidate()
                           let p = checkLanguageCmd(cmd)
                           select p;
            var result = await workflow.Match(
                Succ: r => r,
                Fail: ex => new InvalidRequest(ex.ToString()));

            return result;
        }

        private ILanguageCheckResult checkLanguageCmd(LanguageCheckCmd cmd)
        {
            if (cmd.Title.Contains("injurii") || cmd.Body.Contains("injurii"))
                return new LanguageNotOK();
            else
                return new LanguageOK();


        }
    }
}
