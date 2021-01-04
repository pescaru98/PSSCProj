using CSharp.Choices;
using System;
using System.Collections.Generic;
using System.Text;

namespace StackUnderflow.Domain.Core.Contexts.Question.QuestionOwnerAckOp
{
    [AsChoice]
    public static partial class QuestionOwnerAckResult
    {
        public interface IQuestionOwnerAckResult { } //IDynClonable ?

        public class QuestionOwnerAcknowledged : IQuestionOwnerAckResult
        {
            /*
             * Nu mai trebuie nimic aici
             */
        }

        public class InvalidRequest : IQuestionOwnerAckResult
        {
            public string Message { get; }

            public InvalidRequest (string message)
            {
                Message = message;
            }
        }
    }
}
