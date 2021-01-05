using Access.Primitives.IO;
using Access.Primitives.IO.Mocking;
using GrainInterfaces;
using Orleans;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static StackUnderflow.Domain.Core.Contexts.Question.QuestionOwnerAckOp.QuestionOwnerAckResult;

namespace StackUnderflow.Domain.Core.Contexts.Question.QuestionOwnerAckOp
{
    public partial class QuestionOwnerAckAdapter : Adapter<QuestionOwnerAckCmd, IQuestionOwnerAckResult, QuestionWriteContext, QuestionDependencies>
    {

        private readonly IClusterClient _clusterClient;

        public QuestionOwnerAckAdapter(IClusterClient clusterClient)
        {
            _clusterClient = clusterClient;
        }

        public override Task PostConditions(QuestionOwnerAckCmd cmd, IQuestionOwnerAckResult result, QuestionWriteContext state)
        {
            return Task.CompletedTask;
        }

        public override async Task<IQuestionOwnerAckResult> Work(QuestionOwnerAckCmd cmd, QuestionWriteContext state, QuestionDependencies dependencies)
        {
            var sendEmailGrain = this._clusterClient.GetGrain<IEmailSender>(0);

            var sendEmailGrainResult = await sendEmailGrain.SendEmailAsync(cmd.Message,"Title",cmd.User.Email,cmd.User.Name);

            return new QuestionOwnerAcknowledged();
        }
    }
}
