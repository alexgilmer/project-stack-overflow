namespace project_stack_overflow.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddQuestion : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Questions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Body = c.String(),
                        ApplicationUserId = c.String(maxLength: 128),
                        Date = c.DateTime(nullable: false),
                        VoteTotal = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId)
                .Index(t => t.ApplicationUserId);
            
            AddColumn("dbo.AspNetUsers", "Reputation", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Questions", "ApplicationUserId", "dbo.AspNetUsers");
            DropIndex("dbo.Questions", new[] { "ApplicationUserId" });
            DropColumn("dbo.AspNetUsers", "Reputation");
            DropTable("dbo.Questions");
        }
    }
}
