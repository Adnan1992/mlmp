namespace LearningManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class assignment : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Assignmets",
                c => new
                    {
                        AssignmetID = c.Int(nullable: false, identity: true),
                        FileName = c.String(),
                        Path = c.String(),
                        BatchID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AssignmetID)
                .ForeignKey("dbo.Batches", t => t.BatchID, cascadeDelete: true)
                .Index(t => t.BatchID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Assignmets", "BatchID", "dbo.Batches");
            DropIndex("dbo.Assignmets", new[] { "BatchID" });
            DropTable("dbo.Assignmets");
        }
    }
}
