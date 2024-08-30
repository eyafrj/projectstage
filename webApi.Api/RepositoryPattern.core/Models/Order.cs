using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.core.Models
{
    public class Order
    {
        [Key]

        public int OrderID { get; set; }
        public int AccountID { get; set; }
        public int SecurityId { get; set; }
        public string OrderType { get; set; }
        public int Quantity { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderStatus { get; set; }
        public DateTime ExecutionDate { get; set; }
        public int BrokerID { get; set; }
        [ForeignKey("AccountID")]
        public Account Account { get; set; }

        [ForeignKey("SecurityId")]
        public Security Security { get; set; }
    }
}
