namespace Capstone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removingcorestandardsfromquesitons : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Question", new[] { "StandardId" });
            RenameColumn(table: "dbo.Question", name: "StandardId", newName: "CoreStandard_Id");
            AlterColumn("dbo.Question", "CoreStandard_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Question", "CoreStandard_Id");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Question", new[] { "CoreStandard_Id" });
            AlterColumn("dbo.Question", "CoreStandard_Id", c => c.String(nullable: false, maxLength: 128));
            RenameColumn(table: "dbo.Question", name: "CoreStandard_Id", newName: "StandardId");
            CreateIndex("dbo.Question", "StandardId");
        }
    }
}
