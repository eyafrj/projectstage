using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;

namespace Portfolio.Models
{
    public class Security
    {
		[Key]
		
		public int SecurityId { get; set; }

		[MaxLength(100)]
        public string SecurityName { get; set; } = "";
        [MaxLength(100)]
        public string symbol { get; set; } = "";
        [MaxLength(100)]
        public string  SecurityType { get ; set;} = "";
        [Precision(16 ,2)]
        public  decimal currentPrice { get; set; }
        [Precision(16, 2)]
        public decimal MarketValue { get; set; }
        public DateTime Lastupdate {  get; set; }

        public List<SecurityShare> SecurityShares { get; set; }
        public List<Order> Orders { get; set; }
        public List<Transaction> Transactions { get; set; }


        public Security() {
          
                SecurityShares = new List<SecurityShare>();
            Orders = new List<Order>();
            Transactions = new List<Transaction>();



        }



    }
}
