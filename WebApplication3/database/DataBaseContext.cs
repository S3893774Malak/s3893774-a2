using Microsoft.EntityFrameworkCore;
using WebApplication3.Models;

namespace WebApplication3.database
{
    public class DataBaseContext:DbContext
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options) { }
        public DbSet<Account> Account { get; set; }
        public DbSet<BillPay> BillPay { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<Login> Login { get; set; }
        public DbSet<Payee> Payee { get; set; }
        public DbSet<Transaction> Transaction { get; set; }
    }
}
