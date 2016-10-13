namespace LearningManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StudentFees : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.StudentFees",
                c => new
                    {
                        StudentFeeID = c.Int(nullable: false, identity: true),
                        Amount = c.String(nullable: false),
                        EnrollmentID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.StudentFeeID)
                .ForeignKey("dbo.Enrollments", t => t.EnrollmentID, cascadeDelete: true)
                .Index(t => t.EnrollmentID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StudentFees", "EnrollmentID", "dbo.Enrollments");
            DropIndex("dbo.StudentFees", new[] { "EnrollmentID" });
            DropTable("dbo.StudentFees");
        }
    }
}
