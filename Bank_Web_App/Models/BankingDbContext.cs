using System.Collections.Generic;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;

namespace Bank_Web_App.Models
{
    public class BankingDbContext : DbContext
    {
        public BankingDbContext(DbContextOptions<BankingDbContext> options) : base(options) { }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Login> Logins { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<BillPay> BillPays { get; set; }
        public DbSet<Payee> Payees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Define foreign key relationships
            modelBuilder.Entity<Login>()
                .HasOne(l => l.Customer)
                .WithOne(c => c.Login)
                .HasForeignKey<Login>(l => l.CustomerID);

            modelBuilder.Entity<Account>()
                .HasOne(a => a.Customer)
                .WithMany(c => c.Accounts)
                .HasForeignKey(a => a.CustomerID);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Account)
                .WithMany(a => a.Transactions)
                .HasForeignKey(t => t.AccountNumber);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.DestinationAccount)
                .WithMany()
                .HasForeignKey(t => t.DestinationAccountNumber);

            modelBuilder.Entity<BillPay>()
                .HasOne(b => b.Account)
                .WithMany()
                .HasForeignKey(b => b.AccountNumber);

            modelBuilder.Entity<BillPay>()
                .HasOne(b => b.Payee)
                .WithMany()
                .HasForeignKey(b => b.PayeeID);

            // Specify the store type for the decimal properties
            modelBuilder.Entity<BillPay>()
                .Property(b => b.Amount)
                .HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<Transaction>()
                .Property(t => t.Amount)
                .HasColumnType("decimal(18, 2)");
        }

    }

}
