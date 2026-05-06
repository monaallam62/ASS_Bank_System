using ASS_Bank_System.Models;
using Microsoft.EntityFrameworkCore;

namespace ASS_Bank_System
{
    internal class Program
    {
        static void Main(string[] args)
        {
            #region Another way Seeding 
            //using AddDbContext Context = new AddDbContext();
            //Context.Database.Migrate();

            //if (!Context.Branchs.Any())
            //{
            //    Context.Branchs.AddRange(
            //        new Branch
            //        {
            //            Code = "Z11",
            //            Name = "Mona Allam",
            //            Address = "Cairo",
            //            PhoneNumber = "0102325598"
            //        },
            //        new Branch
            //        {
            //            Code = "A02",
            //            Name = "Myar Sharqawy",
            //            Address = "Cairo",
            //            PhoneNumber = "01234332579"
            //        });
            //    Context.SaveChanges();
            //    Console.WriteLine("Branches seeded successfully");
            //}
            //if (!Context.Managers.Any())
            //{
            //    Context.Managers.AddRange(
            //        new Manager { ManagerId= 1,  FullName = "Mohamed Kamel", Email = "Mohamed12@mail.com", PhoneNumber = "0101234385", HireDate = DateTime.Now, BranchCode = "Z11" },
            //        new Manager { ManagerId = 2, FullName = "Ahmed Ali", Email = "ahmed@mail.com", PhoneNumber = "0113572193", HireDate = DateTime.Now, BranchCode = "A02" }
            //        );
            //    Context.SaveChanges();
            //    Console.WriteLine("Managers seeded successfully");
            //}
            #endregion
            #region Part3
            using AddDbContext context = new AddDbContext();

            while(true)
            {
                Console.Clear();
                Console.WriteLine("==================National Bank-Mnaagment===================== \n");
                Console.WriteLine("1) Add a new Customer");
                Console.WriteLine("2) Open a new Account for a Customer");
                Console.WriteLine("3) Update Account Status");
                Console.WriteLine("4) Remove an Account from a Customer");
                Console.WriteLine("5) List all Customers (with accounts)");
                Console.WriteLine("0) Exit \n");

                Console.Write("Enter Choice :");
                var choice = Console.ReadLine();

                switch(choice)
                {
                    case "1":
                        AddCustomer(context);
                        break;
                    case "2":
                        OpenAccount(context);
                        break;

                    case "3":
                        UpdateStatus(context);
                        break;

                    case "4":
                        RemoveAccount(context);
                        break;

                    case "5":
                        ListCustomers(context);
                        break;

                    case "0":
                        return;
                }
                Console.WriteLine("Press any Key to return to the Menu......");
                Console.ReadKey();

            }
            #endregion

        }

        #region AddCustomer
        static void AddCustomer(AddDbContext context)
        {
            var customer = new Customer();
            Console.Write("Full Name  :");
            customer.FullName = Console.ReadLine();

            Console.Write("National ID  :");
            customer.NationalId = Console.ReadLine();

            Console.Write("Date of Birth  :");
            customer.DateOfBirth = DateTime.Parse(Console.ReadLine());

            Console.Write("Email   :");
            customer.Email = Console.ReadLine();

            Console.Write("Phone   :");
            customer.PhoneNumber = Console.ReadLine();

            Console.Write("Address   :");
            customer.Address = Console.ReadLine();

            Console.Write("Type: ");
            customer.CustomerType = Console.ReadLine();

            context.Set<Customer>().Add(customer);
            context.SaveChanges();

        }
        #endregion
        #region OpenAccount
        static void OpenAccount(AddDbContext context)
        {
            Console.Write("Account Number    : ");
            var accNum = Console.ReadLine();

            Console.Write("Account Type    : ");
            var accType = Console.ReadLine();

            Console.Write("Branch Code    : ");
            var branchCode = Console.ReadLine();

            Console.Write("Customer Id: ");
            var custId = int.Parse(Console.ReadLine());

            if (!context.Branchs.Any(b => b.Code == branchCode) ||
                !context.Customers.Any(c => c.CustomerId == custId))
            {
                Console.WriteLine("Invalid data!");
                return;
            }

            var account = new Account
            {
                AccountNumber = accNum,
                AccountType= accType,
                BranchCode = branchCode,
                OpeningDate = DateTime.Now,
                CurrentBalance = 0
            };

            context.Accounts.Add(account);

            context.Set<CustomerAccount>().Add(new CustomerAccount
            {
                CustomerId = custId,
                AccountNumber = accNum,
                OwnershipType = "Primary",
                OwnershipStartDate = DateTime.Now,
                AccountStatus = "Active"
            });

            context.SaveChanges();
        }
        #endregion
        #region Update Status 
        static void UpdateStatus(AddDbContext context)
        {
            Console.Write("Account Number    : ");
            var acc = Console.ReadLine();

            Console.Write("Customer Id    : ");
            var custId = int.Parse(Console.ReadLine());

            var ca = context.Set<CustomerAccount>()
                .FirstOrDefault(x => x.AccountNumber == acc && x.CustomerId == custId);

            if (ca != null)
            {
                ca.AccountStatus = ca.AccountStatus == "Active" ? "Inactive" : "Active";
                context.SaveChanges();
            }
        }
        #endregion
        #region Remove
        static void RemoveAccount(AddDbContext context)
        {
            Console.Write("Account Number    : ");
            var acc = Console.ReadLine();

            Console.Write("Customer Id: ");
            var custId = int.Parse(Console.ReadLine());

            var ca = context.Set<CustomerAccount>()
                .FirstOrDefault(x => x.AccountNumber == acc && x.CustomerId == custId);

            if (ca != null)
            {
                context.Set<CustomerAccount>().Remove(ca);
                context.SaveChanges();
            }
        }
        #endregion
        #region List Customer
        static void ListCustomers(AddDbContext context)
        {
            var customers = context.Customers
                .Include(c => c.CustomerAccounts)
                .ThenInclude(ca => ca.Account)
                .ToList();

            foreach (var c in customers)
            {
                Console.WriteLine($"Customer    : {c.FullName}");

                foreach (var acc in c.CustomerAccounts)
                {
                    Console.WriteLine($"Account    : {acc.AccountNumber} - {acc.Account.AccountType}");
                }
            }
        }
        #endregion

    }
    }
