namespace Capstone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class questionindex : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Question", "QuestionIndex", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Question", "QuestionIndex");
        }
    }
}
