using ASS_Bank_System.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASS_Bank_System
{
    public class AddDbContext:DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=.\SQL2026;Database=BankSys;Trusted_Connection=True;TrustServerCertificate=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Branch>(BR =>
            {
                BR.HasKey(B => B.Code);
                BR.HasOne(B => B.Manager)
                     .WithOne(M => M.Branch)
                     .HasForeignKey<Manager>(M => M.BranchCode);

            });

            modelBuilder.Entity<Account>(A =>
            {
                A.HasKey(AC => AC.AccountNumber);
                A.HasOne(AC => AC.Branch)
                     .WithMany(B => B.Accounts)
                     .HasForeignKey(AC=>AC.BranchCode);
            });

            modelBuilder.Entity<Transaction>(TR =>
            {
                TR.HasKey(T => T.TransactionNumber);
                TR.HasOne(T => T.Account)
                      .WithMany(AC => AC.Transactions)
                      .HasForeignKey(T => T.AccountNum);
            });

            modelBuilder.Entity<CustomerAccount>(CA =>
            {
                CA.HasKey(ca => new { ca.CustomerId, ca.AccountNumber });

                CA.HasOne(ca => ca.Account)
                .WithMany(AC => AC.CustomerAccounts)
                .HasForeignKey(ca => ca.AccountNumber);

                CA.HasOne(ca => ca.Customer)
                .WithMany(C => C.CustomerAccounts)
                .HasForeignKey(ca => ca.CustomerId);
            });


        }

        public DbSet<Branch> Branchs { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Transaction> Transactions { get; set; }


    }
}
