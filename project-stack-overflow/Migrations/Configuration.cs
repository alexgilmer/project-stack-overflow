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

            MembershipHelper.AddRole("Admin");
            MembershipHelper.AddRole("Moderator");

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
            MembershipHelper.MakeUser(admin);
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
            MembershipHelper.MakeUser(moderator);
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

            //Question block
            {
                List<Question> qList = new List<Question>();
                Question q1 = new Question
                {
                    Id = 1,
                    Title = "Test question asked by User1",
                    Body = "These are the contents of the first question from User1.  What is HTML?",
                    ApplicationUserId = "ba194914-4cfa-4e0a-918e-8faf6f0d5a31",
                    Date = DateTime.Parse("2021-10-11 15:29:11.577"),
                    VoteTotal = 0,
                    Resolved = false
                };
                qList.Add(q1);

                Question q2 = new Question
                {
                    Id = 2,
                    Title = "This is a question about Python",
                    Body = "I HAVE A QUESTION ABOUT PYTHON RAWWWWWWWWWWWWR",
                    ApplicationUserId = "ba194914-4cfa-4e0a-918e-8faf6f0d5a31",
                    Date = DateTime.Parse("2021-10-12 12:42:35.960"),
                    VoteTotal = 0,
                    Resolved = false
                };
                qList.Add(q2);

                Question q3 = new Question
                {
                    Id = 3,
                    Title = "This question is to show Mohamad that asking questions works!",
                    Body = "What should I ask?  How do I know?",
                    ApplicationUserId = "ba194914-4cfa-4e0a-918e-8faf6f0d5a31",
                    Date = DateTime.Parse("2021-10-12 13:00:27.100"),
                    VoteTotal = 0,
                    Resolved = false
                };
                qList.Add(q3);

                Question q4 = new Question
                {
                    Id = 4,
                    Title = "bulk question 1",
                    Body = "something?",
                    ApplicationUserId = "8ead4c6e-c2c3-4d5f-b2ff-6f59018f0858",
                    Date = DateTime.Parse("2021-10-16 16:01:27.907"),
                    VoteTotal = 0,
                    Resolved = false
                };
                qList.Add(q4);

                Question q5 = new Question
                {
                    Id = 5,
                    Title = "bulk question 2",
                    Body = "something again?",
                    ApplicationUserId = "8ead4c6e-c2c3-4d5f-b2ff-6f59018f0858",
                    Date = DateTime.Parse("2021-10-16 16:01:38.940"),
                    VoteTotal = 0,
                    Resolved = false
                };
                qList.Add(q5);

                Question q6 = new Question
                {
                    Id = 6,
                    Title = "bulk question 3",
                    Body = "now, something to do with css!?",
                    ApplicationUserId = "8ead4c6e-c2c3-4d5f-b2ff-6f59018f0858",
                    Date = DateTime.Parse("2021-10-16 16:01:53.187"),
                    VoteTotal = 0,
                    Resolved = false
                };
                qList.Add(q6);

                Question q7 = new Question
                {
                    Id = 7,
                    Title = "bulk question 4",
                    Body = "html time!",
                    ApplicationUserId = "8ead4c6e-c2c3-4d5f-b2ff-6f59018f0858",
                    Date = DateTime.Parse("2021-10-16 16:02:07.810"),
                    VoteTotal = 0,
                    Resolved = true
                };
                qList.Add(q7);

                Question q8 = new Question
                {
                    Id = 8,
                    Title = "bulk question 5",
                    Body = "these java questions, they get so.... bulky. ",
                    ApplicationUserId = "8ead4c6e-c2c3-4d5f-b2ff-6f59018f0858",
                    Date = DateTime.Parse("2021-10-16 16:02:27.673"),
                    VoteTotal = 0,
                    Resolved = false
                };
                qList.Add(q8);

                Question q9 = new Question
                {
                    Id = 9,
                    Title = "bulk question 6",
                    Body = "Why did I have to create so many gosh darn tags?",
                    ApplicationUserId = "8ead4c6e-c2c3-4d5f-b2ff-6f59018f0858",
                    Date = DateTime.Parse("2021-10-16 16:02:45.183"),
                    VoteTotal = 0,
                    Resolved = false
                };
                qList.Add(q9);

                Question q10 = new Question
                {
                    Id = 10,
                    Title = "bulk question 7",
                    Body = "And 10 per page?  That's gotta be a crime against future me. ",
                    ApplicationUserId = "8ead4c6e-c2c3-4d5f-b2ff-6f59018f0858",
                    Date = DateTime.Parse("2021-10-16 16:03:01.810"),
                    VoteTotal = 0,
                    Resolved = true
                };
                qList.Add(q10);

                Question q11 = new Question
                {
                    Id = 11,
                    Title = "bulk question 8",
                    Body = "CREATIVITY IS HARD",
                    ApplicationUserId = "8ead4c6e-c2c3-4d5f-b2ff-6f59018f0858",
                    Date = DateTime.Parse("2021-10-16 16:03:30.730"),
                    VoteTotal = 0,
                    Resolved = false
                };
                qList.Add(q11);

                Question q12 = new Question
                {
                    Id = 12,
                    Title = "bulk question 9",
                    Body = "mAyBe I sHoUlD uSe ThE sArCaSm FoNt",
                    ApplicationUserId = "8ead4c6e-c2c3-4d5f-b2ff-6f59018f0858",
                    Date = DateTime.Parse("2021-10-16 16:03:59.190"),
                    VoteTotal = 0,
                    Resolved = false
                };
                qList.Add(q12);

                Question q13 = new Question
                {
                    Id = 13,
                    Title = "bulk question 10",
                    Body = "SQL?  Isn't that just 3 different languages in a trenchcoat?  That's what a friend of mine said. ",
                    ApplicationUserId = "8ead4c6e-c2c3-4d5f-b2ff-6f59018f0858",
                    Date = DateTime.Parse("2021-10-16 16:04:39.087"),
                    VoteTotal = 0,
                    Resolved = true
                };
                qList.Add(q13);

                Question q14 = new Question
                {
                    Id = 14,
                    Title = "Bulk question 11",
                    Body = "Because this list goes to ELEVEN",
                    ApplicationUserId = "8ead4c6e-c2c3-4d5f-b2ff-6f59018f0858",
                    Date = DateTime.Parse("2021-10-16 16:04:55.707"),
                    VoteTotal = 0,
                    Resolved = false
                };
                qList.Add(q14);

                context.Questions.AddOrUpdate(q => q.Body, qList.ToArray());
            }

            //Answers
            {
                Answer a1 = new Answer
                {
                    QuestionId = 13,
                    ApplicationUserId = "ba194914-4cfa-4e0a-918e-8faf6f0d5a31",
                    Date = DateTime.Parse("2021-10-17 15:58:50.277"),
                    VoteTotal = 0,
                    Body = "I'm that friend.  IT REALLY IS. ",
                    CorrectAnswer = false
                };

                Answer a2 = new Answer
                {
                    QuestionId = 13,
                    ApplicationUserId = "b6ce8157-818d-438c-aeab-ba4b280ac7c4",
                    Date = DateTime.Parse("2021-10-17 16:00:32.987"),
                    VoteTotal = 0,
                    Body = "It is not.  It's an efficient way of querying databases.  Study up!",
                    CorrectAnswer = true
                };
                context.Answers.AddOrUpdate(a => a.Body, a1, a2);
            }

            //CommentQuestions
            {
                CommentQuestion cq1 = new CommentQuestion
                {
                    ApplicationUserId = "ba194914-4cfa-4e0a-918e-8faf6f0d5a31",
                    QuestionId = 10,
                    Body = "Hello, future you!  GET BACK TO WORK!",
                    Date = DateTime.Parse("2021-10-17 15:59:10.420")
                };

                CommentQuestion cq2 = new CommentQuestion
                {
                    ApplicationUserId = "b6ce8157-818d-438c-aeab-ba4b280ac7c4",
                    QuestionId = 13,
                    Body = "(Needs Citation) lmao",
                    Date = DateTime.Parse("2021-10-17 16:00:06.053")
                };
                //Sometimes, this works.  Other times, it breaks.  I can't tell how or why. 
                //Foriegn Key constraint error, again.  
                //context.CommentQuestions.AddOrUpdate(cq => cq.Body, cq1, cq2);
            }

            //CommentAnswers
            {
                CommentAnswer ca1 = new CommentAnswer
                {
                    ApplicationUserId = "b6ce8157-818d-438c-aeab-ba4b280ac7c4",
                    AnswerId = 1,
                    Body = "Is it, though?  I'm not convinced by your logic.  ",
                    Date = DateTime.Parse("2021-10-17 15:59:49.287")
                };
                //This line seems to throw a Foreign Key constraint error during the seed method. 
                //context.CommentAnswers.AddOrUpdate(ca => ca.Body, ca1);
            }

            //QuestionTags
            {
                List<QuestionTag> qtList = new List<QuestionTag>();

                qtList.Add(new QuestionTag
                {
                    TagId = 11,
                    QuestionId = 14
                });

                qtList.Add(new QuestionTag
                {
                    TagId = 10,
                    QuestionId = 13
                });

                qtList.Add(new QuestionTag
                {
                    TagId = 9,
                    QuestionId = 12
                });

                qtList.Add(new QuestionTag
                {
                    TagId = 8,
                    QuestionId = 11
                });

                qtList.Add(new QuestionTag
                {
                    TagId = 7,
                    QuestionId = 10
                });

                qtList.Add(new QuestionTag
                {
                    TagId = 6,
                    QuestionId =9
                });

                qtList.Add(new QuestionTag
                {
                    TagId = 5,
                    QuestionId =9
                });

                qtList.Add(new QuestionTag
                {
                    TagId = 5,
                    QuestionId = 8
                });

                qtList.Add(new QuestionTag
                {
                    TagId = 4,
                    QuestionId = 8
                });

                qtList.Add(new QuestionTag
                {
                    TagId = 4,
                    QuestionId = 7
                });

                qtList.Add(new QuestionTag
                {
                    TagId = 3,
                    QuestionId = 7
                });

                qtList.Add(new QuestionTag
                {
                    TagId = 3,
                    QuestionId =6
                });

                qtList.Add(new QuestionTag
                {
                    TagId = 2,
                    QuestionId = 5
                });

                qtList.Add(new QuestionTag
                {
                    TagId = 1,
                    QuestionId = 4
                });

                qtList.Add(new QuestionTag
                {
                    TagId = 1,
                    QuestionId = 3
                });

                qtList.Add(new QuestionTag
                {
                    TagId = 7,
                    QuestionId = 3
                });

                qtList.Add(new QuestionTag
                {
                    TagId = 10,
                    QuestionId = 3
                });

                qtList.Add(new QuestionTag
                {
                    TagId = 8,
                    QuestionId = 2
                });

                qtList.Add(new QuestionTag
                {
                    TagId = 4,
                    QuestionId = 1
                });

                //throwing an error, for unknown reasons. 
                //The INSERT statement conflicted with the FOREIGN KEY constraint "FK_dbo.QuestionTags_dbo.Tags_TagId".
                //The conflict occurred in database "project-stack-overflow", table "dbo.Tags", column 'Id'.
                //context.QuestionTags.AddOrUpdate(qt => new { qt.TagId, qt.QuestionId }, qtList.ToArray());
            }
        }
    }
}
