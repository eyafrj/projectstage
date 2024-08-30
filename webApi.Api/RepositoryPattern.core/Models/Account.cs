using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.core.Models
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

        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
 






    }
}
