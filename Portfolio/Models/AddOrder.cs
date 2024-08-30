using Microsoft.AspNetCore.Mvc.Rendering;

namespace Portfolio.ViewModels
{
    public class AddOrder
    {

        public int AccountID { get; set; }
        public int SecurityId { get; set; }
        public string OrderType { get; set; }
        public int Quantity { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderStatus { get; set; }
        public DateTime ExecutionDate { get; set; }
        public int BrokerID { get; set; }
        public List<SelectListItem> Securities { get; set; }
        public List<SelectListItem> Accounts { get; set; }
    }
}
