using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Portfolio.Models
{
    public class SecurityAdd
    {
      
     
		[Required , MaxLength(100)]
        public string SecurityName { get; set; } = "";
        [Required, MaxLength(100)]
        public string symbol { get; set; } = "";
        [Required, MaxLength(100)]
        public string SecurityType { get; set; } = "";
        [Required]
        public decimal currentPrice { get; set; }
        [Required]
        public decimal MarketValue { get; set; }
		public DateTime Lastupdate { get; set; }

        public List<SecurityShare> SecurityShares { get; set; }
        public SecurityAdd()
        {
            SecurityShares = new List<SecurityShare>();
        }
    }
}
