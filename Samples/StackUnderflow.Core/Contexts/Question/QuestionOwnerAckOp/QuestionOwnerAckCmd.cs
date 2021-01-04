﻿using EarlyPay.Primitives.ValidationAttributes;
using StackUnderflow.EF.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace StackUnderflow.Domain.Core.Contexts.Question.QuestionOwnerAckOp
{
    public struct QuestionOwnerAckCmd
    {
        public QuestionOwnerAckCmd(User user)
        {
            User = user;
        }

        [Required]
        public User User { get; set; }

    }
}
