namespace TestSMApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddInspectorFieldMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Inspections", "Inspector_Id", c => c.Int());
            CreateIndex("dbo.Inspections", "Inspector_Id");
            AddForeignKey("dbo.Inspections", "Inspector_Id", "dbo.Inspectors", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Inspections", "Inspector_Id", "dbo.Inspectors");
            DropIndex("dbo.Inspections", new[] { "Inspector_Id" });
            DropColumn("dbo.Inspections", "Inspector_Id");
        }
    }
}
