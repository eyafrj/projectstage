using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Portfolio.ViewModels
{
    public class OrderCreate
    {
        public int AccountID { get; set; } // Utiliser AccountID pour la cohérence
        public int SecurityId { get; set; }

        [Required(ErrorMessage = "The OrderType field is required.")]
        public string OrderType { get; set; }

        
        public string OrderStatus { get; set; }

        [Required(ErrorMessage = "The Quantity field is required.")]
        public int Quantity { get; set; }

        [BindNever]
        public List<SelectListItem> Securities { get; set; } = new List<SelectListItem>(); // Initialiser la liste

        [BindNever]
        public List<SelectListItem> Accounts { get; set; } = new List<SelectListItem>();  // Initialiser la liste

        public DateTime OrderDate { get; set; }
        public int BrokerID { get; set; }
        public OrderCreate()
        {
            OrderStatus = "pending";
        }
    }

}
