namespace _9_MyAcademy_MVC_CodeFirst.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddManualPolicyStatus : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PolicySales", "ManualPolicyStatus", c => c.Int());
            DropColumn("dbo.PolicySales", "PolicyStatus");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PolicySales", "PolicyStatus", c => c.Int(nullable: false));
            DropColumn("dbo.PolicySales", "ManualPolicyStatus");
        }
    }
}
