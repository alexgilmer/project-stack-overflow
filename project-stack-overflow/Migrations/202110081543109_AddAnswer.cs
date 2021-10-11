namespace project_stack_overflow.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAnswer : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Answers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        QuestionId = c.Int(nullable: false),
                        ApplicationUserId = c.String(maxLength: 128),
                        Date = c.DateTime(nullable: false),
                        VoteTotal = c.Int(nullable: false),
                        Body = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId)
                .ForeignKey("dbo.Questions", t => t.QuestionId, cascadeDelete: true)
                .Index(t => t.QuestionId)
                .Index(t => t.ApplicationUserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Answers", "QuestionId", "dbo.Questions");
            DropForeignKey("dbo.Answers", "ApplicationUserId", "dbo.AspNetUsers");
            DropIndex("dbo.Answers", new[] { "ApplicationUserId" });
            DropIndex("dbo.Answers", new[] { "QuestionId" });
            DropTable("dbo.Answers");
        }
    }
}
