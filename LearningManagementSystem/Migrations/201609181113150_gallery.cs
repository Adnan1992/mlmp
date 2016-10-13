namespace LearningManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class gallery : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Galleries",
                c => new
                    {
                        GalleryID = c.Int(nullable: false, identity: true),
                        ImageName = c.String(),
                        ImagePath = c.String(),
                    })
                .PrimaryKey(t => t.GalleryID);
            
            AddColumn("dbo.Instructors", "Biography", c => c.String());
            AddColumn("dbo.Instructors", "Certificates", c => c.String());
            AddColumn("dbo.Instructors", "SocialLinks", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Instructors", "SocialLinks");
            DropColumn("dbo.Instructors", "Certificates");
            DropColumn("dbo.Instructors", "Biography");
            DropTable("dbo.Galleries");
        }
    }
}
