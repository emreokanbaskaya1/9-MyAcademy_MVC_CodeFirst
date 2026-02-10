namespace _9_MyAcademy_MVC_CodeFirst.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAIClassificationFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ContactMessages", "AICategory", c => c.String(maxLength: 50));
            AddColumn("dbo.ContactMessages", "AIConfidence", c => c.Double());
            AddColumn("dbo.ContactMessages", "AIIsUrgent", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ContactMessages", "AIIsUrgent");
            DropColumn("dbo.ContactMessages", "AIConfidence");
            DropColumn("dbo.ContactMessages", "AICategory");
        }
    }
}
