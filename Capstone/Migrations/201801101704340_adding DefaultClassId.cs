namespace Capstone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addingDefaultClassId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "DefaultClassId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "DefaultClassId");
        }
    }
}
