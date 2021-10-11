using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace project_stack_overflow.Models
{
    public class Answer
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public virtual Question Question { get; set; }
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public DateTime Date { get; set; }
        public int VoteTotal { get; set; }
        public string Body { get; set; }

    }
}