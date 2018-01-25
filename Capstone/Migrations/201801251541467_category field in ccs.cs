namespace Capstone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class categoryfieldinccs : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CoreStandard", "Category", c => c.String());
            AlterColumn("dbo.CoreStandard", "Grade", c => c.String(nullable: false, maxLength: 20, fixedLength: true));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CoreStandard", "Grade", c => c.String(nullable: false, maxLength: 10, fixedLength: true));
            DropColumn("dbo.CoreStandard", "Category");
        }
    }
}
