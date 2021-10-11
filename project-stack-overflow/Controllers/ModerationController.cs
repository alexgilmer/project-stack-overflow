using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace project_stack_overflow.Controllers
{
    [Authorize(Roles = "Admin, Moderator")]
    public class ModerationController : Controller
    {
        // GET: Moderation
        public ActionResult Index()
        {
            return View();
        }
    }
}