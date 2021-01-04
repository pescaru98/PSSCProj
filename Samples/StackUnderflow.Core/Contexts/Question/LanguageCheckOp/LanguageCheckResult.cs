using CSharp.Choices;
using StackUnderflow.EF.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace StackUnderflow.Domain.Core.Contexts.Question.LanguageCheckOp
{
    [AsChoice]
    public static partial class LanguageCheckResult
    {
        public interface ILanguageCheckResult { } //IDynClonable ??

        public class LanguageOK : ILanguageCheckResult
        {
            /*
             * Aici poate fi un Post (cel primit de la IValidateResult, doar daca si in cmd este acel Post)
             * in loc de title si body, deoarece nu e relevant
             * titlul si body-ul pentru urmatoarea intrare, cea a ack-ului, ci UserId-ul
             */


        }

        public class LanguageNotOK : ILanguageCheckResult
        {
/*            public string Reason;

            public LanguageNotOK(string reason)
            {
                Reason = reason;
            }*/
        }

        public class InvalidRequest : ILanguageCheckResult
        {
            public string Message { get; }

            public InvalidRequest(string message)
            {
                Message = message;
            }
        }
    }
}
