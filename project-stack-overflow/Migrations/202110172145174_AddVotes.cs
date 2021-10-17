namespace project_stack_overflow.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddVotes : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserVotes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ApplicationUserId = c.String(maxLength: 128),
                        QuestionId = c.Int(),
                        AnswerId = c.Int(),
                        Vote = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Answers", t => t.AnswerId)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId)
                .ForeignKey("dbo.Questions", t => t.QuestionId)
                .Index(t => t.ApplicationUserId)
                .Index(t => t.QuestionId)
                .Index(t => t.AnswerId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserVotes", "QuestionId", "dbo.Questions");
            DropForeignKey("dbo.UserVotes", "ApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserVotes", "AnswerId", "dbo.Answers");
            DropIndex("dbo.UserVotes", new[] { "AnswerId" });
            DropIndex("dbo.UserVotes", new[] { "QuestionId" });
            DropIndex("dbo.UserVotes", new[] { "ApplicationUserId" });
            DropTable("dbo.UserVotes");
        }
    }
}
