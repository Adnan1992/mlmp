namespace LearningManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Attendances",
                c => new
                {
                    AttendanceID = c.Int(nullable: false, identity: true),
                    AttendanceDate = c.DateTime(nullable: false),
                    EnrollmentID = c.Int(nullable: false),
                    BatchID = c.Int(nullable: false),
                    AttendanceStatus = c.String(),
                })
                .PrimaryKey(t => t.AttendanceID)
                .ForeignKey("dbo.Enrollments", t => t.EnrollmentID, cascadeDelete: true)
                .Index(t => t.EnrollmentID);

            CreateTable(
                "dbo.Enrollments",
                c => new
                {
                    EnrollmentID = c.Int(nullable: false, identity: true),
                    BatchID = c.Int(nullable: false),
                    StudentID = c.Int(nullable: false),
                    IsApproved = c.String(),
                    UserId = c.String(),
                })
                .PrimaryKey(t => t.EnrollmentID)
                .ForeignKey("dbo.Batches", t => t.BatchID, cascadeDelete: true)
                .ForeignKey("dbo.Students", t => t.StudentID, cascadeDelete: false)
                .Index(t => t.BatchID)
                .Index(t => t.StudentID);

            CreateTable(
                "dbo.Batches",
                c => new
                {
                    BatchID = c.Int(nullable: false, identity: true),
                    BatchName = c.String(),
                    StartingDate = c.DateTime(nullable: false),
                    FinishingDate = c.DateTime(nullable: false),
                    Title = c.String(),
                    Description = c.String(),
                    InstructorNamme = c.String(),
                    CourseID = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.BatchID)
                .ForeignKey("dbo.Courses", t => t.CourseID, cascadeDelete: true)
                .Index(t => t.CourseID);

            CreateTable(
                "dbo.Courses",
                c => new
                {
                    CourseID = c.Int(nullable: false, identity: true),
                    Title = c.String(nullable: false),
                    Description = c.String(nullable: false, maxLength: 1000),
                    ImageName = c.String(),
                    Path = c.String(),
                })
                .PrimaryKey(t => t.CourseID);

            CreateTable(
                "dbo.Payments",
                c => new
                {
                    PaymentID = c.Int(nullable: false),
                    Amount = c.String(),
                    PaymentForBatch = c.String(),
                    PaymentDate = c.DateTime(nullable: false),
                })
                .PrimaryKey(t => t.PaymentID)
                .ForeignKey("dbo.Batches", t => t.PaymentID)
                .Index(t => t.PaymentID);

            CreateTable(
                "dbo.FileUploads",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    FileName = c.String(),
                    Image = c.String(),
                    DocFile = c.String(),
                    UserId = c.String(),
                })
                .PrimaryKey(t => t.Id);

        }

        public override void Down()
        {

            DropForeignKey("dbo.Attendances", "EnrollmentID", "dbo.Enrollments");
            DropForeignKey("dbo.Enrollments", "StudentID", "dbo.Students");
            DropForeignKey("dbo.Enrollments", "BatchID", "dbo.Batches");
            DropForeignKey("dbo.Payments", "PaymentID", "dbo.Batches");
            DropForeignKey("dbo.Batches", "CourseID", "dbo.Courses");

            
            DropIndex("dbo.Payments", new[] { "PaymentID" });
            DropIndex("dbo.Batches", new[] { "CourseID" });
            DropIndex("dbo.Enrollments", new[] { "StudentID" });
            DropIndex("dbo.Enrollments", new[] { "BatchID" });
            DropIndex("dbo.Attendances", new[] { "EnrollmentID" });

            
            DropTable("dbo.FileUploads");

            DropTable("dbo.Payments");
            DropTable("dbo.Courses");
            DropTable("dbo.Batches");
            DropTable("dbo.Enrollments");
            DropTable("dbo.Attendances");
        }
    }
}
