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
        public ActionResult Index(int? pageNumber)
        {
            //For now, pagination is hard-coded to 10 per page. 
            int resultsPerPage = 10;

            int skippedPages = 0;
            if (pageNumber != null)
                skippedPages = (int)pageNumber - 1;

            //check to make sure our skippedPages takes us to a valid page
            //if invalid, take us to last page instead. 
            if (db.Questions.Count() <= resultsPerPage * skippedPages)
            {
                ViewBag.SkippedTooMany = true;
                skippedPages = (db.Questions.Count() - 1) / resultsPerPage;
            }

            var paginatedQuestions = db.Questions
                .OrderByDescending(q => q.Date)
                .Skip(resultsPerPage * skippedPages)
                .Take(resultsPerPage)
                .ToList();

            ViewBag.CurrentPage = skippedPages + 1;
            ViewBag.ResultsPerPage = resultsPerPage;
            ViewBag.MaxPages = (int)Math.Ceiling((double)db.Questions.Count() / resultsPerPage);
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

            if (User.IsInRole("Admin"))
                ViewBag.UserIsAdmin = true;

            return View(question);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult DeleteQuestion(int? id)
        {
            // TODO: Add confirmation click. 
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            /*
            db.Questions.Remove(question);
            db.SaveChanges();
            */
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
    }
}