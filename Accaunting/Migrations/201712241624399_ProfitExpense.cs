namespace Accaunting.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProfitExpense : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ExpenseCategories",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        name = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Expenses",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        amount = c.Double(nullable: false),
                        category_id = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.ExpenseCategories", t => t.category_id)
                .Index(t => t.category_id);
            
            CreateTable(
                "dbo.ProfitCategories",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        name = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Profits",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        amount = c.Double(nullable: false),
                        category_id = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.ProfitCategories", t => t.category_id)
                .Index(t => t.category_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Profits", "category_id", "dbo.ProfitCategories");
            DropForeignKey("dbo.Expenses", "category_id", "dbo.ExpenseCategories");
            DropIndex("dbo.Profits", new[] { "category_id" });
            DropIndex("dbo.Expenses", new[] { "category_id" });
            DropTable("dbo.Profits");
            DropTable("dbo.ProfitCategories");
            DropTable("dbo.Expenses");
            DropTable("dbo.ExpenseCategories");
        }
    }
}
