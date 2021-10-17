using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace project_stack_overflow.Models
{
    public class UserVote
    {
        public int Id { get; set; }
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public int? QuestionId { get; set; }
        public virtual Question Question { get; set; }
        public int? AnswerId { get; set; }
        public virtual Answer Answer { get; set; }

        public bool Vote { get; set; }
        //true = upvote, false = downvote
    }
}