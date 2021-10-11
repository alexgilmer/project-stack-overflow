namespace project_stack_overflow.Migrations
{
    using Microsoft.AspNet.Identity.EntityFramework;
    using project_stack_overflow.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<project_stack_overflow.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            // this is the admin account
            // admin@test.com
            // abcABC123!@#
            ApplicationUser admin = new ApplicationUser
            {
                Id = "8ead4c6e-c2c3-4d5f-b2ff-6f59018f0858",
                Email = "admin@test.com",
                EmailConfirmed = false,
                PasswordHash = "AMv/DtCh9KVZDCo4m62SAne+qTUOW1Hk63IzC6h6Hb51pWNEDwxogMr4gygV7yly2A==", //'abcABC123!@#'
                SecurityStamp = "bfe99026-4e8e-446a-9ef5-23ed1d3fce68",
                PhoneNumber = null,
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                LockoutEndDateUtc = null,
                LockoutEnabled = true,
                AccessFailedCount = 0,
                UserName = "admin@test.com",
                Reputation = 0
            };
            context.Users.AddOrUpdate(u => u.UserName, admin);
            MembershipHelper.AddUserToRole(admin.Id, "Admin");

            // this is the moderator account
            // moderator@test.com
            // abcABC123!@#
            ApplicationUser moderator = new ApplicationUser
            {
                Id = "8bb7b4d1-96ee-4a2f-b730-66df0a5b6525",
                Email = "moderator@test.com",
                EmailConfirmed = false,
                PasswordHash = "AFjsoRFuB62woYs92nlDXzROA0zxYdmGD5LHRFj1ytHhdNKZGYENL9wQbadNnAPPJA==", //'abcABC123!@#'
                SecurityStamp = "ac5faf31-c75c-4407-a9af-6d3e3f5c07d9",
                PhoneNumber = null,
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                LockoutEndDateUtc = null,
                LockoutEnabled = true,
                AccessFailedCount = 0,
                UserName = "moderator@test.com",
                Reputation = 0
            };
            context.Users.AddOrUpdate(u => u.UserName, moderator);
            MembershipHelper.AddUserToRole(moderator.Id, "Moderator");

            // three-user block
            {
                ApplicationUser user1 = new ApplicationUser
                {
                    Id = "ba194914-4cfa-4e0a-918e-8faf6f0d5a31",
                    Email = "user1@test.com",
                    EmailConfirmed = false,
                    PasswordHash = "AKkDIkDKcJa0IivPfnuXcMWFf1p3GXKm6dO3GTvUtB3+G6q4cJjrhmhevv1lH7/3HA==", //'abcABC123!@#'
                    SecurityStamp = "df133424-2f7b-4c12-be52-1b7fcf8158d5",
                    PhoneNumber = null,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEndDateUtc = null,
                    LockoutEnabled = true,
                    AccessFailedCount = 0,
                    UserName = "user1@test.com",
                    Reputation = 0
                };

                ApplicationUser user2 = new ApplicationUser
                {
                    Id = "b6ce8157-818d-438c-aeab-ba4b280ac7c4",
                    Email = "user2@test.com",
                    EmailConfirmed = false,
                    PasswordHash = "AMGclD9MGcmpKh7skH307U++i5ebT1S4eICxHTrW35iNj4E8rOnOpdWvKs/KlXcdfg==", //'abcABC123!@#'
                    SecurityStamp = "ba736c86-8eaa-4c63-8fe8-6a6198388a9a",
                    PhoneNumber = null,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEndDateUtc = null,
                    LockoutEnabled = true,
                    AccessFailedCount = 0,
                    UserName = "user2@test.com",
                    Reputation = 0
                };

                ApplicationUser user3 = new ApplicationUser
                {
                    Id = "6930cad0-6174-4113-a047-bc25f9cb4157",
                    Email = "user3@test.com",
                    EmailConfirmed = false,
                    PasswordHash = "AFk0AFOZNirOTYs6rzqlJIUcKmZQ8BLiHDMsY1dQkRrNQSrvD9FvpkyulUv4fc6MDQ==", //'abcABC123!@#'
                    SecurityStamp = "cfef232b-1197-406d-9aa2-1280abb92837",
                    PhoneNumber = null,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEndDateUtc = null,
                    LockoutEnabled = true,
                    AccessFailedCount = 0,
                    UserName = "user3@test.com",
                    Reputation = 0
                };

                context.Users.AddOrUpdate(u => u.UserName, user1, user2, user3);
            }

            //Tag block
            {
                List<Tag> tagList = new List<Tag>();

                Tag cSharp = new Tag("C#");
                tagList.Add(cSharp);

                Tag cPlus = new Tag("C++");
                tagList.Add(cPlus);

                Tag css = new Tag("CSS");
                tagList.Add(css);

                Tag html = new Tag("HTML");
                tagList.Add(html);

                Tag java = new Tag("Java");
                tagList.Add(java);

                Tag js = new Tag("Javascript");
                tagList.Add(js);

                Tag mvc = new Tag("MVC");
                tagList.Add(mvc);

                Tag py = new Tag("Python");
                tagList.Add(py);

                Tag ruby = new Tag("Ruby");
                tagList.Add(ruby);

                Tag sql = new Tag("SQL");
                tagList.Add(sql);

                Tag vb = new Tag("Visual Basic");
                tagList.Add(vb);

                context.Tags.AddOrUpdate(t => t.Name, tagList.ToArray());
            }


        }
    }
}
