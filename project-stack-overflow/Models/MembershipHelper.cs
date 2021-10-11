using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace project_stack_overflow.Models
{
    public class MembershipHelper
    {
        static ApplicationDbContext db = new ApplicationDbContext();

        static RoleManager<IdentityRole> roleManager = new RoleManager<IdentityRole>
            (new RoleStore<IdentityRole>(db));
        static UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>
            (new UserStore<ApplicationUser>(db));

        public static List<String> GetAllRoles()
        {
            var result = roleManager.Roles.Select(r => r.Name).ToList();
            return result;
        }

        public static bool CheckIfUserInRole(string userId, string role)
        {
            var result = userManager.IsInRole(userId, role);

            return result;
        }

        public static bool AddUserToRole(string userId, string role)
        {
            if (CheckIfUserInRole(userId, role))
            {
                //User already in that role
                return false;
            }

            userManager.AddToRole(userId, role);
            return true;
        }

        public static bool AddRole(string role)
        {
            if (roleManager.RoleExists(role))
            {
                //role already exists
                return false;
            }

            roleManager.Create(new IdentityRole { Name = role });
            return true;
        }

        public static bool DeleteRoll(string role)
        {
            if (!roleManager.RoleExists(role))
            {
                //role doesn't exist
                return false;
            }

            roleManager.Delete(new IdentityRole { Name = role });
            return true;
        }

        public static List<string> GetAllRolesForUser(string userId)
        {
            return userManager.GetRoles(userId).ToList();
        }
    }
}