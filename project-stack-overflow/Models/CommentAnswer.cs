using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace project_stack_overflow.Models
{
    public class CommentAnswer
    {
        public int Id { get; set; }
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public int AnswerId { get; set; }
        public virtual Answer Answer { get; set; }
        public string Body { get; set; }
        public DateTime Date { get; set; }
    }
}