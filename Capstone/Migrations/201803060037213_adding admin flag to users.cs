namespace Capstone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addingadminflagtousers : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Admin", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Admin");
        }
    }
}
