namespace LearningManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class amountrequired : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Payments", "Amount", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Payments", "Amount", c => c.String());
        }
    }
}
