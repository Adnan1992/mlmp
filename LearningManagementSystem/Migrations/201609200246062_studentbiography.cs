namespace LearningManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class studentbiography : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Students", "Biography", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Students", "Biography");
        }
    }
}
