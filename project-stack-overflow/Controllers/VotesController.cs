using project_stack_overflow.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace project_stack_overflow.Controllers
{
    public class VotesController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        private const int RepuationPerVote = 5; //set by specifications.  

        // GET: Votes
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Home");
        }

        public ActionResult CastUpVote(int qId, string userId)
        {
            UserVote uv = db.UserVotes.FirstOrDefault(vote => vote.ApplicationUserId == userId && vote.QuestionId == qId);
            Question q = db.Questions.Find(qId);

            if (uv == null)
            {
                uv = new UserVote
                {
                    QuestionId = qId,
                    ApplicationUserId = userId,
                    Vote = true
                };
                db.UserVotes.Add(uv);
                q.VoteTotal++;
                q.ApplicationUser.Reputation += RepuationPerVote;

                db.SaveChanges();
            }
            else
            {
                //uv already exists in the system
                switch (uv.Vote)
                {
                    case true:
                        db.UserVotes.Remove(uv);
                        q.VoteTotal--;
                        q.ApplicationUser.Reputation -= RepuationPerVote;
                        break;
                    case false:
                        uv.Vote = true;
                        //double value, because we remove the down, AND give it an upvote.
                        q.VoteTotal += 2; 
                        q.ApplicationUser.Reputation += (2 * RepuationPerVote);
                        break;
                    default:
                        throw new Exception("Your CastUpVote switch statement blew up. ");
                }
                db.SaveChanges();
            }
            return RedirectToAction("ViewQuestion", "Questions", new { id = qId });
        }

        public ActionResult CastDownVote(int qId, string userId)
        {
            UserVote uv = db.UserVotes.FirstOrDefault(vote => vote.ApplicationUserId == userId && vote.QuestionId == qId);
            Question q = db.Questions.Find(qId);

            if (uv == null)
            {
                uv = new UserVote
                {
                    QuestionId = qId,
                    ApplicationUserId = userId,
                    Vote = false
                };
                db.UserVotes.Add(uv);
                q.VoteTotal--;
                q.ApplicationUser.Reputation -= RepuationPerVote;

                db.SaveChanges();
            }
            else
            {
                //uv already exists in the system
                switch (uv.Vote)
                {
                    case false:
                        db.UserVotes.Remove(uv);
                        q.VoteTotal++;
                        q.ApplicationUser.Reputation += RepuationPerVote;
                        break;
                    case true:
                        uv.Vote = false;
                        //double value
                        q.VoteTotal -= 2;
                        q.ApplicationUser.Reputation -= (2 * RepuationPerVote);
                        break;
                    default:
                        throw new Exception("Your CastDownVote switch statement blew up. ");
                }
                db.SaveChanges();
            }

            return RedirectToAction("ViewQuestion", "Questions", new { id = qId });
        }

        public ActionResult CastAnswerUpVote(int aId, string userId)
        {
            UserVote uv = db.UserVotes.FirstOrDefault(vote => vote.ApplicationUserId == userId && vote.AnswerId == aId);
            Answer a = db.Answers.Find(aId);

            if (uv == null)
            {
                uv = new UserVote
                {
                    AnswerId = aId,
                    ApplicationUserId = userId,
                    Vote = true
                };
                db.UserVotes.Add(uv);
                a.VoteTotal++;
                a.ApplicationUser.Reputation += RepuationPerVote;

                db.SaveChanges();
            }
            else
            {
                //uv already exists in the system
                switch (uv.Vote)
                {
                    case true:
                        db.UserVotes.Remove(uv);
                        a.VoteTotal--;
                        a.ApplicationUser.Reputation -= RepuationPerVote;
                        break;
                    case false:
                        uv.Vote = true;
                        //double value
                        a.VoteTotal += 2;
                        a.ApplicationUser.Reputation += (2 * RepuationPerVote);
                        break;
                    default:
                        throw new Exception("Your CastAnswerUpVote switch statement blew up. ");
                }
                db.SaveChanges();
            }

            return RedirectToAction("ViewQuestion", "Questions", new { id = a.QuestionId });
        }
        public ActionResult CastAnswerDownVote(int aId, string userId)
        {
            UserVote uv = db.UserVotes.FirstOrDefault(vote => vote.ApplicationUserId == userId && vote.AnswerId == aId);
            Answer a = db.Answers.Find(aId);

            if (uv == null)
            {
                uv = new UserVote
                {
                    AnswerId = aId,
                    ApplicationUserId = userId,
                    Vote = false
                };
                db.UserVotes.Add(uv);
                a.VoteTotal--;
                a.ApplicationUser.Reputation -= RepuationPerVote;

                db.SaveChanges();
            }
            else
            {
                //uv already exists in the system
                switch (uv.Vote)
                {
                    case false:
                        db.UserVotes.Remove(uv);
                        a.VoteTotal++;
                        a.ApplicationUser.Reputation += RepuationPerVote;
                        break;
                    case true:
                        uv.Vote = false;
                        //double value
                        a.VoteTotal -= 2;
                        a.ApplicationUser.Reputation -= (2 * RepuationPerVote);
                        break;
                    default:
                        throw new Exception("Your CastDownVote switch statement blew up. ");
                }
                db.SaveChanges();
            }

            return RedirectToAction("ViewQuestion", "Questions", new { id = a.QuestionId });
        }

    }
}