using MathNet.Numerics;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;


namespace BLL.core.Models
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
        public string SecurityType { get; set; } = "";
        [Precision(16, 2)]
        public decimal currentPrice { get; set; }
        [Precision(16, 2)]
        public decimal MarketValue { get; set; }
        public DateTime Lastupdate { get; set; }
    }
}
