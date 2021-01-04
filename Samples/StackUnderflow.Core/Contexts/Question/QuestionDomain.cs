using Access.Primitives.IO;
using StackUnderflow.Domain.Core.Contexts.Question.LanguageCheckOp;
using StackUnderflow.Domain.Core.Contexts.Question.QuestionOwnerAckOp;
using StackUnderflow.Domain.Core.Contexts.Question.ValidateQuestionOp;

using System;
using System.Collections.Generic;
using System.Text;
using static PortExt;
using LanguageExt;
using static StackUnderflow.Domain.Core.Contexts.Question.LanguageCheckOp.LanguageCheckResult;
using static StackUnderflow.Domain.Core.Contexts.Question.QuestionOwnerAckOp.QuestionOwnerAckResult;
using static StackUnderflow.Domain.Core.Contexts.Question.ValidateQuestionOp.ValidateQuestionResult;

namespace StackUnderflow.Domain.Core
{
    public static class QuestionDomain
    {
        public static Port<IValidateQuestionResult> ValidateQuestion(ValidateQuestionCmd command)
        {
            return NewPort<ValidateQuestionCmd, IValidateQuestionResult>(command);
        }

        public static Port<IQuestionOwnerAckResult> QuestionOwnerAck(QuestionOwnerAckCmd command)
        {
            return NewPort<QuestionOwnerAckCmd, IQuestionOwnerAckResult>(command);
        }

        public static Port<ILanguageCheckResult> LanguageCheck(LanguageCheckCmd command)
        {
            return NewPort<LanguageCheckCmd, ILanguageCheckResult>(command);
        }
    }
}
