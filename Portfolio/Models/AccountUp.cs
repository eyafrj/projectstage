using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Portfolio.Models
{
    public class AccountUp
    {
        [Required, MaxLength(50)]
        public string AccountName { get; set; }
        [Required, MaxLength(50)]
        public string AccountType { get; set; }

        [Required, Precision(16, 2)]
        public decimal TotalMarketValue { get; set; }


        public DateTime ModifiedDate { get; set; }


    }
}
