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
            optionsBuilder.UseSqlServer(@"Server=.\SQL2026;Database=BankSystem;Trusted_Connection=True;TrustServerCertificate=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Relations
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
                     .HasForeignKey(AC => AC.BranchCode);
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

            #endregion
            #region Seeding
            modelBuilder.Entity<Branch>().HasData(
                new Branch { Code = "A01", Name = "Main Branch", Address = "Cairo", PhoneNumber = "01242063325" },
                new Branch { Code = "Z02", Name = "Z02 Branch", Address = "Alex", PhoneNumber = "01023046981" }
            );

            modelBuilder.Entity<Manager>().HasData(
                new Manager { ManagerId = 1, FullName = "Ahmed", Email = "Ahmed@mail.com", PhoneNumber = "01242063325", HireDate = new DateTime(2024, 1, 1), BranchCode = "A01" },
                new Manager { ManagerId = 2, FullName = "Mohamed", Email = "Mohamed@mail.com", PhoneNumber = "01023046981", HireDate = new DateTime(2024, 1, 1), BranchCode = "Z02" }
            );

            modelBuilder.Entity<Customer>().HasData(
                new Customer { CustomerId = 1, FullName = "Mona Mohamed", NationalId = "12345678901234", DateOfBirth = new DateTime(2000, 5, 10), Email = "mona@mail.com", PhoneNumber = "01012345678", Address = "Tanta", CustomerType = "Individual" }
            );

            modelBuilder.Entity<Account>().HasData(
                new Account { AccountNumber = "A1001", CurrentBalance = 5000, AccountType = "Savings", OpeningDate = new DateTime(2024, 1, 1), BranchCode = "Z02" }
            );

            modelBuilder.Entity<CustomerAccount>().HasData(
                new
                {
                    CustomerId = 1,
                    AccountNumber = "A1001",
                    OwnershipType = "Primary",
                    OwnershipStartDate = new DateTime(2024, 1, 1),
                    AccountStatus = "Active"
                }
            );           
            #endregion

        }

        public DbSet<Branch> Branchs { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Transaction> Transactions { get; set; }


    }
}
