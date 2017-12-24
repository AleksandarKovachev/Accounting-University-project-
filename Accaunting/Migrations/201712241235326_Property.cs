namespace Accaunting.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Property : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Properties",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        key = c.String(),
                        value = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Properties");
        }
    }
}
