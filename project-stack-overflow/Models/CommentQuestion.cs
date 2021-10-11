using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace project_stack_overflow.Models
{
    public class CommentQuestion
    {
        public int Id { get; set; }
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public int QuestionId { get; set; }
        public virtual Question Question { get; set; }
        public string Body { get; set; }
        public DateTime Date { get; set; }

    }
}