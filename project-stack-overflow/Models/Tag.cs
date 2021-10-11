using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace project_stack_overflow.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<QuestionTag> QuestionTags { get; set; }
        public Tag()
        {
            this.QuestionTags = new HashSet<QuestionTag>();
        }
        public Tag(string name)
        {
            this.Name = name;
            this.QuestionTags = new HashSet<QuestionTag>();
        }
    }
}