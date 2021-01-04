using EarlyPay.Primitives.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace StackUnderflow.Domain.Core.Contexts.Question.ValidateQuestionOp
{

    public struct ValidateQuestionCmd
    {
        public ValidateQuestionCmd(int tenantId, Guid userId, byte postType, string title, string body)
        {
            TenantId = tenantId;
            UserId = userId;
            PostType = postType;
            Title = title;
            Body = body;
        }

        [Required]
        public int TenantId { get; set; }

        [GuidNotEmpty]
        public Guid UserId { get; set; }

        [Required]
        public byte PostType { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Body { get; set; }


    }
}
