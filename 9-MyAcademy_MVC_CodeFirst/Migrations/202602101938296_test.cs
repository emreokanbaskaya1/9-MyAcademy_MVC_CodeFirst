namespace _9_MyAcademy_MVC_CodeFirst.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class test : DbMigration
    {
        public override void Up()
        {
            // Project column already doesn't exist, just add InsuranceType if not exists
            Sql(@"
                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[ContactMessages]') AND name = 'InsuranceType')
                BEGIN
                    ALTER TABLE dbo.ContactMessages ADD InsuranceType NVARCHAR(100) NOT NULL DEFAULT 'Other';
                END
            ");
        }
        
        public override void Down()
        {
            Sql(@"
                IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[ContactMessages]') AND name = 'InsuranceType')
                BEGIN
                    ALTER TABLE dbo.ContactMessages DROP COLUMN InsuranceType;
                END
            ");
        }
    }
}
