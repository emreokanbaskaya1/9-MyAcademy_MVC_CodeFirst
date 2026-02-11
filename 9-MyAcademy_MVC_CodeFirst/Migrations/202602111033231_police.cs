namespace _9_MyAcademy_MVC_CodeFirst.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class police : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PolicySales",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false, maxLength: 100),
                        LastName = c.String(nullable: false, maxLength: 100),
                        Email = c.String(nullable: false, maxLength: 200),
                        PhoneNumber = c.String(nullable: false, maxLength: 20),
                        TCIdentityNumber = c.String(maxLength: 11),
                        Address = c.String(maxLength: 500),
                        ProductId = c.Int(nullable: false),
                        SaleAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PolicyStartDate = c.DateTime(nullable: false),
                        PolicyEndDate = c.DateTime(nullable: false),
                        PaymentStatus = c.Int(nullable: false),
                        PolicyStatus = c.Int(nullable: false),
                        Notes = c.String(maxLength: 1000),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 100),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PolicySales", "ProductId", "dbo.Products");
            DropIndex("dbo.PolicySales", new[] { "ProductId" });
            DropTable("dbo.PolicySales");
        }
    }
}
