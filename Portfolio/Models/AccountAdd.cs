using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Portfolio.Models
{
    public class AccountAdd
    {
        [Required, MaxLength(50)]
        public string AccountName { get; set; }
        [Required, MaxLength(50)]
        public string AccountType { get; set; }

        [Required, Precision(16, 2)]
        public decimal TotalMarketValue { get; set; } =  0;
        public List<SecurityShare> SecurityShares { get; set; }



        public DateTime CreatedDate { get; set; }

        public AccountAdd()
        {
            SecurityShares = new List<SecurityShare>();
            TotalMarketValue = 0;  // Ensure it's set to 0 by default
        }





    } 
}
