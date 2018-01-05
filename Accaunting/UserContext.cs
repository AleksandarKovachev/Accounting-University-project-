namespace Accaunting
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class UserContext : DbContext
    {
        public UserContext() : base("name=UserContext") { }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Property> Properties { get; set; }
        public virtual DbSet<Profit> Profits { get; set; }
        public virtual DbSet<ProfitCategory> ProfitCategories { get; set; }
        public virtual DbSet<Expense> Expenses { get; set; }
        public virtual DbSet<ExpenseCategory> ExpenseCategories { get; set; }
    }
}