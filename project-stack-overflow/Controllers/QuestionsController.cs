using Microsoft.AspNet.Identity;
using project_stack_overflow.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace project_stack_overflow.Controllers
{
    public class QuestionsController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        // GET: Questions
        public ActionResult Index(int? pageNumber, string SortSelect, string ResolvedSelect)
        {
            //For now, pagination is hard-coded to 10 per page. 
            int resultsPerPage = 10;

            int skippedPages = 0;
            if (pageNumber != null)
                skippedPages = (int)pageNumber - 1;

            List<Question> filteredList;

            if (ResolvedSelect == "onlyResolved")
            {
                filteredList = db.Questions.Where(q => q.Resolved).ToList();
                ViewBag.FilterResolution = "resolved";
            }
            else if (ResolvedSelect == "onlyUnresolved")
            {
                filteredList = db.Questions.Where(q => !q.Resolved).ToList();
                ViewBag.FilterResolution = "unresolved";
            }
            else
            {
                filteredList = db.Questions.ToList();
                ViewBag.FilterResolution = "all";
            }

            //check to make sure our skippedPages takes us to a valid page
            //if invalid, take us to last page instead. 
            if (filteredList.Count() <= resultsPerPage * skippedPages)
            {
                ViewBag.SkippedTooMany = true;
                skippedPages = (filteredList.Count() - 1) / resultsPerPage;
            }

            List<Question> paginatedQuestions;

            if (SortSelect == "answers")
            {
                ViewBag.SelectedSort = "number of answers";
                paginatedQuestions = filteredList
                    .OrderByDescending(q => q.Answers.Count)
                    .Skip(resultsPerPage * skippedPages)
                    .Take(resultsPerPage)
                    .ToList();
            }
            else
            {
                // this is the default search, so null should land here
                ViewBag.SelectedSort = "most recent";
                paginatedQuestions = filteredList
                    .OrderByDescending(q => q.Date)
                    .Skip(resultsPerPage * skippedPages)
                    .Take(resultsPerPage)
                    .ToList();
            }

            ViewBag.SortSelect = SortSelect ?? "date";
            ViewBag.ResolvedSelect = ResolvedSelect ?? "all";

            ViewBag.CurrentPage = skippedPages + 1;
            ViewBag.ResultsPerPage = resultsPerPage;
            ViewBag.MaxPages = (int)Math.Ceiling((double)filteredList.Count() / resultsPerPage);
            return View(paginatedQuestions);
        }

        //TODO: implement search view
        public ActionResult TextSearch()
        {
            return View();
        }
        [HttpPost]
        public ActionResult TextSearch(string query)
        {
            //basic string matching.
            //if the querystring exists anywhere in the title, it's a hit. 3
            //this is horribly inefficient, and scales poorly.  

            var results = db.Questions.Where(q => q.Title.Contains(query));
            return View(results.ToList());
        }

        public ActionResult TagSearch(int? TagSelect)
        {
            ViewBag.TagSelect = new SelectList(db.Tags.OrderBy(t => t.Name), "Id", "Name");
            
            if (TagSelect != null)
            {
                return TagSearch((int)TagSelect);
            }

            return View();
        }

        [HttpPost]
        public ActionResult TagSearch(int TagSelect)
        {
            ViewBag.TagSelect = new SelectList(db.Tags.OrderBy(t => t.Name), "Id", "Name");

            var results = db.QuestionTags.Where(qt => qt.TagId == TagSelect).Select(qt => qt.Question).ToList();
            return View(results);
        }

        
        [Authorize]
        public ActionResult AskQuestion()
        {
            ViewBag.TagList = db.Tags.OrderBy(t => t.Name).ToList();
            return View();
        }
        
        [Authorize]
        [HttpPost]
        public ActionResult AskQuestion(string title, string body, string[] tags)
        {
            ViewBag.TagList = db.Tags.OrderBy(t => t.Name).ToList();

            if (User.Identity.IsAuthenticated)
            {
                bool returnToView = false;
                if (string.IsNullOrEmpty(title))
                {
                    ViewBag.TitleError = true;
                    returnToView = true;
                }
                if (string.IsNullOrEmpty(body))
                {
                    ViewBag.BodyError = true;
                    returnToView = true;
                }
                if (returnToView)
                {
                    ViewBag.PostedTitle = title;
                    ViewBag.PostedBody = body;
                    return View();
                }
                
                Question newQ = new Question
                {
                    Title = title,
                    Body = body,
                    VoteTotal = 0,
                    ApplicationUserId = User.Identity.GetUserId(),
                    Date = DateTime.Now,
                };
                db.Questions.Add(newQ);

                var tagsToAdd = new List<Tag>();
                foreach (string tag in tags)
                {
                    tagsToAdd.Add(db.Tags.FirstOrDefault(t => t.Name == tag));
                }

                foreach(Tag tag in tagsToAdd)
                {
                    QuestionTag qt = new QuestionTag
                    {
                        Question = newQ,
                        Tag = tag,
                    };
                    db.QuestionTags.Add(qt);
                }
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        public ActionResult ViewQuestion(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }

            ViewQuestionViewModel vm = new ViewQuestionViewModel();
            vm.Question = question;


            if (User.IsInRole("Admin"))
                vm.UserIsAdmin = true;

            if (User.Identity.IsAuthenticated && !question.Resolved)
                vm.UserMayAnswer = true;

            if (User.Identity.GetUserId() == question.ApplicationUserId && !question.Resolved)
                vm.SolutionMarkable = true;

            if (User.Identity.IsAuthenticated)
            {
                vm.UserId = User.Identity.GetUserId();

                vm.UserIsAuthenticated = true;

                vm.UserQuestionVote = FetchQuestionVote(vm.UserId, question.Id)?.Vote; //a?.b returns null if a==null, otherwise return a.b

                foreach (Answer answer in question.Answers)
                {
                    vm.AnswersAndVotes.Add(answer, 
                        FetchAnswerVote(vm.UserId, answer.Id)?.Vote); //a?.b returns null if a==null, otherwise return a.b
                }
            }
            else
            {
                vm.UserIsAuthenticated = false;
                vm.UserQuestionVote = null;
                foreach(Answer answer in question.Answers)
                {
                    vm.AnswersAndVotes.Add(answer, null);
                }
            }

            return View(vm);
        }

        public UserVote FetchQuestionVote(string userId, int? qId)
        {
            return db.UserVotes.FirstOrDefault
                (vote =>
                vote.ApplicationUserId == userId
                && vote.QuestionId == qId);
        }

        public UserVote FetchAnswerVote(string userId, int? aId)
        {
            return db.UserVotes.FirstOrDefault
                (vote =>
                vote.ApplicationUserId == userId
                && vote.AnswerId == aId);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult DeleteQuestion(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            return View(question);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult DeleteQuestion(int id)
        {
            Question question = db.Questions.Find(id);
            if (question == null)
                return HttpNotFound();

            db.Questions.Remove(question);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult AnswerQuestion(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Question question = db.Questions.Find(id);

            if (question == null)
                return HttpNotFound();

            if (question.Resolved)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            return View(question);
        }

        [Authorize]
        [HttpPost]
        public ActionResult AnswerQuestion(int id, string body)
        {
            Question question = db.Questions.Find(id);
            if (question == null)
                return HttpNotFound();

            if (question.Resolved)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            if (string.IsNullOrEmpty(body))
            {
                ViewBag.EmptyAnswerError = true;
                return View(question);
            }

            ApplicationUser user = db.Users.Find(User.Identity.GetUserId());
            if (user == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Answer newAnswer = new Answer
            {
                ApplicationUser = user,
                Body = body,
                Date = DateTime.Now,
                Question = question,
                CorrectAnswer = false,
                VoteTotal = 0,
            };
            db.Answers.Add(newAnswer);
            db.SaveChanges();
            
            return RedirectToAction("ViewQuestion", new { id });
        }

        [Authorize]
        public ActionResult CommentOnQuestion(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }

            return View(question);
        }

        [Authorize]
        [HttpPost]
        public ActionResult CommentOnQuestion(int id, string body)
        {
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }

            if (string.IsNullOrEmpty(body))
            {
                ViewBag.EmptyCommentError = true;
                return View(question);
            }

            var user = db.Users.Find(User.Identity.GetUserId());
            if (user == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            CommentQuestion cq = new CommentQuestion
            {
                Body = body,
                ApplicationUser = user,
                Date = DateTime.Now,
                Question = question,
            };
            db.CommentQuestions.Add(cq);
            db.SaveChanges();
            return RedirectToAction("ViewQuestion", new { id });
        }

        [Authorize]
        public ActionResult CommentOnAnswer(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Answer answer = db.Answers.Find(id);
            if (answer == null)
            {
                return HttpNotFound();
            }

            return View(answer);
        }

        [Authorize]
        [HttpPost]
        public ActionResult CommentOnAnswer(int id, string body)
        {
            Answer answer = db.Answers.Find(id);
            if (answer == null)
            {
                return HttpNotFound();
            }

            if (string.IsNullOrEmpty(body))
            {
                ViewBag.EmptyCommentError = true;
                return View(answer);
            }

            var user = db.Users.Find(User.Identity.GetUserId());
            if (user == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            CommentAnswer ca = new CommentAnswer
            {
                Body = body,
                ApplicationUser = user,
                Date = DateTime.Now,
                Answer = answer,
            };
            db.CommentAnswers.Add(ca);
            db.SaveChanges();
            return RedirectToAction("ViewQuestion", new { id = answer.QuestionId });
        }

        [Authorize]
        public ActionResult MarkSolution(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Answer answer = db.Answers.Find(id);
            if (answer == null)
                return HttpNotFound();

            //check if the question is already answered
            if (answer.Question.Resolved)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            answer.CorrectAnswer = true;
            answer.Question.Resolved = true;
            db.SaveChanges();
            return RedirectToAction("ViewQuestion", new { id = answer.QuestionId });
        }

        [Authorize(Roles = "Admin")]
        public ActionResult DeleteAnswer(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Answer answer = db.Answers.Find(id);
            if (answer == null)
                return HttpNotFound();

            return View(answer);
        }
        
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ActionName("DeleteAnswer")]
        public ActionResult DeleteAnswerConfirmed(int id)
        {
            Answer answer = db.Answers.Find(id);
            if (answer == null)
                return HttpNotFound();

            int redirectId = answer.QuestionId;
            if (answer.CorrectAnswer)
                answer.Question.Resolved = false;

            db.Answers.Remove(answer);
            db.SaveChanges();

            return RedirectToAction("ViewQuestion", new { id = redirectId });
        }
    }
}