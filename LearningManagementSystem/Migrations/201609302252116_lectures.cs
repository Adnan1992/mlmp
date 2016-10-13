namespace LearningManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class lectures : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Lectures",
                c => new
                    {
                        LecturesID = c.Int(nullable: false, identity: true),
                        FileName = c.String(),
                        Path = c.String(),
                        BatchID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.LecturesID)
                .ForeignKey("dbo.Batches", t => t.BatchID, cascadeDelete: true)
                .Index(t => t.BatchID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Lectures", "BatchID", "dbo.Batches");
            DropIndex("dbo.Lectures", new[] { "BatchID" });
            DropTable("dbo.Lectures");
        }
    }
}
