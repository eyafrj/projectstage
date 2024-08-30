using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Portfolio.Models
{
    public class Account
    {
        [Key]

        public int AccountID { get; set; }
        [MaxLength(50)]
        public string AccountName { get; set; }
        [MaxLength(50)]
        public string AccountType { get; set; }

        [Precision(16, 2)]
        public decimal TotalMarketValue { get; set; }
       public List<SecurityShare> SecurityShares { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        [Precision(16, 2)]
        public decimal budgets { get; set; }
      

        public List<Order> Orders { get; set; }

        public List<Transaction> Transactions { get; set; }


        public Account()
        {
            SecurityShares = new List<SecurityShare>();
            Orders = new List<Order>();
            Transactions = new List<Transaction>();
          

        }




    }

}
