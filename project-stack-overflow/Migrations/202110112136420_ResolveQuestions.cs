namespace project_stack_overflow.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ResolveQuestions : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Answers", "CorrectAnswer", c => c.Boolean(nullable: false));
            AddColumn("dbo.Questions", "Resolved", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Questions", "Resolved");
            DropColumn("dbo.Answers", "CorrectAnswer");
        }
    }
}
