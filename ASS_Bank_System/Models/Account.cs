using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASS_Bank_System.Models
{
    public class Account
    {
        public string AccountNumber { get; set; }
        public decimal CurrentBalance { get; set; }
        public string AccountType { get; set; }
        public DateTime OpeningDate { get; set; }


        public string BranchCode { get; set; }

        public Branch Branch { get; set; }

        public ICollection<Transaction>? Transactions { get; set; }

        public ICollection<CustomerAccount>? CustomerAccounts { get; set; }

    }
}
