namespace _9_MyAcademy_MVC_CodeFirst.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNewTablesForIndexPage : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Abouts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 200),
                        Subtitle = c.String(nullable: false, maxLength: 200),
                        Description = c.String(nullable: false),
                        ImageUrl = c.String(maxLength: 500),
                        InsurancePolicies = c.Int(nullable: false),
                        AwardsWon = c.Int(nullable: false),
                        SkilledAgents = c.Int(nullable: false),
                        TeamMembers = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Blogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 200),
                        Description = c.String(nullable: false, maxLength: 500),
                        Content = c.String(nullable: false),
                        ImageUrl = c.String(maxLength: 500),
                        Author = c.String(nullable: false, maxLength: 100),
                        CategoryName = c.String(maxLength: 100),
                        CommentCount = c.Int(nullable: false),
                        PublishDate = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Contacts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Address = c.String(nullable: false, maxLength: 500),
                        Email = c.String(nullable: false, maxLength: 200),
                        Phone = c.String(nullable: false, maxLength: 50),
                        FacebookUrl = c.String(maxLength: 200),
                        TwitterUrl = c.String(maxLength: 200),
                        InstagramUrl = c.String(maxLength: 200),
                        LinkedInUrl = c.String(maxLength: 200),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.FAQs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Question = c.String(nullable: false, maxLength: 500),
                        Answer = c.String(nullable: false),
                        OrderNumber = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Features",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Icon = c.String(nullable: false, maxLength: 100),
                        Title = c.String(nullable: false, maxLength: 200),
                        Description = c.String(nullable: false, maxLength: 500),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Sliders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 200),
                        Subtitle = c.String(nullable: false, maxLength: 200),
                        Description = c.String(nullable: false, maxLength: 500),
                        ImageUrl = c.String(maxLength: 500),
                        ButtonText = c.String(maxLength: 200),
                        ButtonUrl = c.String(maxLength: 200),
                        OrderNumber = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TeamMembers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Position = c.String(nullable: false, maxLength: 100),
                        ImageUrl = c.String(maxLength: 500),
                        FacebookUrl = c.String(maxLength: 200),
                        TwitterUrl = c.String(maxLength: 200),
                        LinkedInUrl = c.String(maxLength: 200),
                        InstagramUrl = c.String(maxLength: 200),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Testimonials",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ClientName = c.String(nullable: false, maxLength: 100),
                        Position = c.String(nullable: false, maxLength: 100),
                        Comment = c.String(nullable: false, maxLength: 1000),
                        ImageUrl = c.String(maxLength: 500),
                        Rating = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AlterColumn("dbo.Categories", "Name", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Products", "Name", c => c.String(nullable: false, maxLength: 200));
            AlterColumn("dbo.Products", "Description", c => c.String(nullable: false, maxLength: 1000));
            AlterColumn("dbo.Products", "ImageUrl", c => c.String(maxLength: 500));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Products", "ImageUrl", c => c.String());
            AlterColumn("dbo.Products", "Description", c => c.String());
            AlterColumn("dbo.Products", "Name", c => c.String());
            AlterColumn("dbo.Categories", "Name", c => c.String());
            DropTable("dbo.Testimonials");
            DropTable("dbo.TeamMembers");
            DropTable("dbo.Sliders");
            DropTable("dbo.Features");
            DropTable("dbo.FAQs");
            DropTable("dbo.Contacts");
            DropTable("dbo.Blogs");
            DropTable("dbo.Abouts");
        }
    }
}
