using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace project_stack_overflow.Models
{
    public class ViewQuestionViewModel
    {
        public Question Question { get; set; }
        public bool UserIsAdmin { get; set; }
        public bool UserMayAnswer { get; set; }
        public bool SolutionMarkable { get; set; }
        public bool UserIsAuthenticated { get; set; }
        public string UserId { get; set; }
        public bool? UserQuestionVote { get; set; }
        public Dictionary<Answer, bool?> AnswersAndVotes { get; set; }
        public ViewQuestionViewModel()
        {
            this.AnswersAndVotes = new Dictionary<Answer, bool?>();
        }
    }
}