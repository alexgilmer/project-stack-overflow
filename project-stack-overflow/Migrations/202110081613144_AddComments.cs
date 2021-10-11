namespace project_stack_overflow.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddComments : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CommentAnswers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ApplicationUserId = c.String(maxLength: 128),
                        AnswerId = c.Int(nullable: false),
                        Body = c.String(),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Answers", t => t.AnswerId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId)
                .Index(t => t.ApplicationUserId)
                .Index(t => t.AnswerId);
            
            CreateTable(
                "dbo.CommentQuestions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ApplicationUserId = c.String(maxLength: 128),
                        QuestionId = c.Int(nullable: false),
                        Body = c.String(),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId)
                .ForeignKey("dbo.Questions", t => t.QuestionId, cascadeDelete: true)
                .Index(t => t.ApplicationUserId)
                .Index(t => t.QuestionId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CommentQuestions", "QuestionId", "dbo.Questions");
            DropForeignKey("dbo.CommentQuestions", "ApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.CommentAnswers", "ApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.CommentAnswers", "AnswerId", "dbo.Answers");
            DropIndex("dbo.CommentQuestions", new[] { "QuestionId" });
            DropIndex("dbo.CommentQuestions", new[] { "ApplicationUserId" });
            DropIndex("dbo.CommentAnswers", new[] { "AnswerId" });
            DropIndex("dbo.CommentAnswers", new[] { "ApplicationUserId" });
            DropTable("dbo.CommentQuestions");
            DropTable("dbo.CommentAnswers");
        }
    }
}
