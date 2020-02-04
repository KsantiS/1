namespace TestSMApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCompanyMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.InspectionNames",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Inspections",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(),
                        Comment = c.String(),
                        InspectionName_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.InspectionNames", t => t.InspectionName_Id)
                .Index(t => t.InspectionName_Id);
            
            CreateTable(
                "dbo.Remarks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(),
                        Comment = c.String(),
                        RemarkName_Id = c.Int(),
                        Inspection_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RemarkNames", t => t.RemarkName_Id)
                .ForeignKey("dbo.Inspections", t => t.Inspection_Id)
                .Index(t => t.RemarkName_Id)
                .Index(t => t.Inspection_Id);
            
            CreateTable(
                "dbo.RemarkNames",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Inspectors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FIO = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Remarks", "Inspection_Id", "dbo.Inspections");
            DropForeignKey("dbo.Remarks", "RemarkName_Id", "dbo.RemarkNames");
            DropForeignKey("dbo.Inspections", "InspectionName_Id", "dbo.InspectionNames");
            DropIndex("dbo.Remarks", new[] { "Inspection_Id" });
            DropIndex("dbo.Remarks", new[] { "RemarkName_Id" });
            DropIndex("dbo.Inspections", new[] { "InspectionName_Id" });
            DropTable("dbo.Inspectors");
            DropTable("dbo.RemarkNames");
            DropTable("dbo.Remarks");
            DropTable("dbo.Inspections");
            DropTable("dbo.InspectionNames");
        }
    }
}
