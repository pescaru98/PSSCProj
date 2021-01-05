using Access.Primitives.Extensions.Cloning;
using CSharp.Choices;
using StackUnderflow.EF.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Access.Primitives.IO;
using LanguageExt;

namespace StackUnderflow.Domain.Core.Contexts.Question.ValidateQuestionOp
{
    [AsChoice]
    public static partial class ValidateQuestionResult
    {
        public interface IValidateQuestionResult : IDynClonable { }

        public class QuestionValidated : IValidateQuestionResult
        {
            public Post Post { get; }
            public User User { get; }

            public QuestionValidated(Post post, User user)
            {
                Post = post;
                User = user;
            }

            public object Clone() => this.ShallowClone(); //what's this?

        }

        public class QuestionInvalidated : IValidateQuestionResult
        {
            public string Reason { get; private set; }

            public QuestionInvalidated(string reason)
            {
                Reason = reason;
            }
            //TODO
            public object Clone() => this.ShallowClone(); //what's this?
        }

        public class InvalidRequest : IValidateQuestionResult
        {
            public string Message { get; private set; }

            public InvalidRequest(string message)
            {
                Message = message;
            }

            //TODO
            public object Clone() => this.ShallowClone(); //what's this?

        }
    }
}
