using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace project_stack_overflow.Models
{
    public class QuestionTag
    {
        public int Id { get; set; }
        public int TagId { get; set; }
        public virtual Tag Tag { get; set; }
        public int QuestionId { get; set; }
        public virtual Question Question { get; set; }
    }
}