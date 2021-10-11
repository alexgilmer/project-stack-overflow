using project_stack_overflow.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace project_stack_overflow.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdministrationController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        //Add a new Tag to the system
        //Give/Revoke Moderator priviledges
        //Ban/Unban Users
        //Go to Moderator Control Panel
    }
}