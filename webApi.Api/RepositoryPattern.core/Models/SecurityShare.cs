using MathNet.Numerics;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using BLL.core.Models;

namespace BLL.core.Models
{
    public class SecurityShare
    {
        [Key]

        public int SecurityshareId { get; set; }
        // Composite key made up of AccountId and SecurityId
        //   [Key, Column(Order = 0)]
        public int AccountId { get; set; }

        //    [Key, Column(Order = 1)]
        public int SecurityId { get; set; }
        [Precision(16, 2)]
        public decimal Quantite { get; set; }

        // Foreign key relationships
        [ForeignKey("AccountId")]
        public Account Account { get; set; }

        [ForeignKey("SecurityId")]
        public Security Security { get; set; }

    }
}
