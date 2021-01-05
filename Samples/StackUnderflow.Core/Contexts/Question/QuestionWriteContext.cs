﻿using StackUnderflow.EF.Models;
using System;
using System.Collections.Generic;
using System.Text;
using static LanguageExt.Prelude;
using Access.Primitives.IO;
using LanguageExt;
using System.Linq;

namespace StackUnderflow.Domain.Core.Contexts.Question
{
    public class QuestionWriteContext
    {
        public ICollection<Post> Posts { get; }
        public ICollection<User> Users { get; }
        public QuestionWriteContext(ICollection<Post> posts, ICollection<User> users)
        {
            Posts = posts ?? new List<Post>(0);
            Users = users ?? new List<User>(0);
        }
    }
}
