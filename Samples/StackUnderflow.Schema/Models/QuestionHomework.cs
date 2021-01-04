using System;
using System.Collections.Generic;
using System.Text;

namespace StackUnderflow.EF.Models
{
    public partial class QuestionHomework
    {
        public int QuestionId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
    }
}
