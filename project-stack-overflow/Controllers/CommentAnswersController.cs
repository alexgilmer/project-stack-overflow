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
    public class CommentAnswersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: CommentAnswers
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Questions");
        }

        // GET: CommentAnswers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CommentAnswer commentAnswer = db.CommentAnswers.Find(id);
            if (commentAnswer == null)
            {
                return HttpNotFound();
            }
            return View(commentAnswer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, string body)
        {
            var ca = db.CommentAnswers.Find(id);
            if (ca == null)
                return HttpNotFound();

            if (string.IsNullOrEmpty(body))
            {
                return RedirectToAction("Delete", new { id });
            }

            ca.Body = body;
            db.SaveChanges();
            return RedirectToAction("ViewQuestion", "Questions", new { id = ca.Answer.QuestionId });
        }

        // GET: CommentAnswers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CommentAnswer commentAnswer = db.CommentAnswers.Find(id);
            if (commentAnswer == null)
            {
                return HttpNotFound();
            }
            return View(commentAnswer);
        }

        // POST: CommentAnswers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CommentAnswer commentAnswer = db.CommentAnswers.Find(id);
            int redirectId = commentAnswer.Answer.QuestionId;
            db.CommentAnswers.Remove(commentAnswer);
            db.SaveChanges();
            return RedirectToAction("ViewQuestion", "Questions", new { id = redirectId });
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
