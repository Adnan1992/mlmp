namespace LearningManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class correctInstructor : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Instructors", "PhoneNumber", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Instructors", "PhoneNumber", c => c.String(maxLength: 11));
        }
    }
}
