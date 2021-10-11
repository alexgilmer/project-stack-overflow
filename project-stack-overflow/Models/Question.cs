using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace project_stack_overflow.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public DateTime Date { get; set; }
        public int VoteTotal { get; set; }
        public virtual ICollection<Answer> Answers { get; set; }
        public virtual ICollection<QuestionTag> QuestionTags { get; set; }
        public virtual ICollection<CommentQuestion> Comments { get; set; }
        public bool Resolved { get; set; }


        public Question()
        {
            this.Answers = new HashSet<Answer>();
            this.QuestionTags = new HashSet<QuestionTag>();
            this.Comments = new HashSet<CommentQuestion>();
        }

    }
}