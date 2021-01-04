using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace StackUnderflow.Domain.Core.Contexts.Question.LanguageCheckOp
{
    public struct LanguageCheckCmd
    {
        public LanguageCheckCmd(string title, string body)
        {
            Title = title;
            Body = body;
        }

        [Required]
        public string Title { get; set; }
        [Required]
        public string Body { get; set; }

        /*
         * poate ar trebui toata atributele 
         * din primul cmd, de la ValidateQuestionCmd, deoarece 
         * este nevoie de UserId pentru urmatoarea operatiune, QuestionOwnerAck,
         * insa UserId cred ca poate fi luat si din iesirea lui ValidateQuestionResult
         */
    }
}
