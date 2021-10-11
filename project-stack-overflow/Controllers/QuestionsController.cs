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

        // TODO: Implement tagsearch view.
        public ActionResult TagSearch()
        {
            ViewBag.TagList = db.Tags.ToList();
            return View();
        }

        [HttpPost]
        public ActionResult TagSearch(int tagId)
        {
            ViewBag.TagList = db.Tags.ToList();

            var results = db.QuestionTags.Where(qt => qt.TagId == tagId).Select(qt => qt.Question);
            return View(results.ToList());
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
                if (title == "")
                {
                    ViewBag.TitleError = true;
                    returnToView = true;
                }
                if (body == "")
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
                    ApplicationUserId = db.Users.First(u => u.UserName == User.Identity.Name).Id,
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
    }
}