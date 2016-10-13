namespace LearningManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class comment : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        CommentID = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                        BatchID = c.Int(nullable: false),
                        MadeBy = c.String(),
                    })
                .PrimaryKey(t => t.CommentID)
                .ForeignKey("dbo.Batches", t => t.BatchID, cascadeDelete: true)
                .Index(t => t.BatchID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comments", "BatchID", "dbo.Batches");
            DropIndex("dbo.Comments", new[] { "BatchID" });
            DropTable("dbo.Comments");
        }
    }
}
