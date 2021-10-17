using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace project_stack_overflow.Models
{
    public class TagEditViewModel
    {
        public Question Question { get; set; }
        public ICollection<QuestionTag> CurrentTags { get; set; }
        public ICollection<Tag> AddableTags { get; set; }

        public TagEditViewModel()
        {
            this.CurrentTags = new List<QuestionTag>();
            this.AddableTags = new List<Tag>();
        }
    }
}