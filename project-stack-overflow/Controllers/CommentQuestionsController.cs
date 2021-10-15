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
    public class CommentQuestionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: CommentQuestions
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Questions");
        }

        // GET: CommentQuestions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CommentQuestion commentQuestion = db.CommentQuestions.Find(id);
            if (commentQuestion == null)
            {
                return HttpNotFound();
            }
            return View(commentQuestion);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, string body)
        {
            var cq = db.CommentQuestions.Find(id);
            if (cq == null)
                return HttpNotFound();

            if (string.IsNullOrEmpty(body))
            {
                return RedirectToAction("Delete", new { id });
            }

            cq.Body = body;
            db.SaveChanges();
            return RedirectToAction("ViewQuestion", "Questions", new { id = cq.QuestionId });
        }

        // GET: CommentQuestions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CommentQuestion commentQuestion = db.CommentQuestions.Find(id);
            if (commentQuestion == null)
            {
                return HttpNotFound();
            }
            return View(commentQuestion);
        }

        // POST: CommentQuestions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CommentQuestion commentQuestion = db.CommentQuestions.Find(id);
            db.CommentQuestions.Remove(commentQuestion);
            db.SaveChanges();
            return RedirectToAction("ViewQuestion", "Questions", new { id = commentQuestion.QuestionId });
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
