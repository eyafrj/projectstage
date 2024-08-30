using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.core.Models
{
    public class Transaction
    {
        [Key]
        public int TransactionID { get; set; }
        public int OrderID { get; set; }
        public int SecurityId { get; set; }
        public int AccountID { get; set; }
        public int Quantite { get; set; }

        [Precision(16, 2)]

        public decimal Price { get; set; }
        public DateTime TransactionDate { get; set; }
        [ForeignKey("AccountID")]
        public Account Account { get; set; }

        [ForeignKey("SecurityId")]
        public Security Security { get; set; }

        [ForeignKey("OrderID")]
        public Order Order { get; set; }





    
}
}
