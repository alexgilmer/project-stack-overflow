using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using project_stack_overflow.Models;

namespace project_stack_overflow.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TagsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Tags
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View(db.Tags.OrderBy(t => t.Name).ToList());
        }

        // GET: Tags/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tag tag = db.Tags.Find(id);
            if (tag == null)
            {
                return HttpNotFound();
            }
            return View(tag);
        }

        // GET: Tags/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tags/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] Tag tag)
        {
            if (ModelState.IsValid)
            {
                if (db.Tags.Any(t => t.Name == tag.Name))
                {
                    Tag oldTag = db.Tags.First(t => t.Name == tag.Name);
                    return RedirectToAction("Details", new { id = oldTag.Id });
                }
                db.Tags.Add(tag);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tag);
        }

        // GET: Tags/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tag tag = db.Tags.Find(id);
            if (tag == null)
            {
                return HttpNotFound();
            }
            return View(tag);
        }

        // POST: Tags/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] Tag tag)
        {
            if (ModelState.IsValid)
            {
                if (db.Tags.Any(t => t.Name == tag.Name))
                {
                    ViewBag.TagExists = true;
                    return View(tag);
                }
                db.Entry(tag).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tag);
        }

        // GET: Tags/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tag tag = db.Tags.Find(id);
            if (tag == null)
            {
                return HttpNotFound();
            }
            return View(tag);
        }

        // POST: Tags/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tag tag = db.Tags.Find(id);
            db.Tags.Remove(tag);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult EditQuestionTags(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Question question = db.Questions.Find(id);

            if (question == null)
                return HttpNotFound();

            var removableTags = question.QuestionTags.Select(qt => qt.Tag).ToHashSet();
            var fullTagList = db.Tags.ToHashSet();

            var addableTags = fullTagList.Where(t => !removableTags.Contains(t)).OrderBy(t => t.Name).ToList();

            var vm = new TagEditViewModel();
            vm.Question = question;
            vm.CurrentTags = question.QuestionTags.OrderBy(qt => qt.Tag.Name).ToList();
            vm.AddableTags = addableTags;
            
            return View(vm);
        }

        public ActionResult RemoveTag(int id, int qtId)
        {
            QuestionTag tagToRemove = db.QuestionTags.Find(qtId);
            if (tagToRemove == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            db.QuestionTags.Remove(tagToRemove);
            db.SaveChanges();

            return RedirectToAction("EditQuestionTags", new { id });
        }

        public ActionResult AddTag(int id, int tId)
        {
            Tag tag = db.Tags.Find(tId);
            if (tag == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            QuestionTag tagToAdd = new QuestionTag
            {
                QuestionId = id,
                TagId = tId
            };
            db.QuestionTags.Add(tagToAdd);
            db.SaveChanges();

            return RedirectToAction("EditQuestionTags", new { id });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
