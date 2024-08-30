using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Portfolio.ViewModels
{
    public class TransactionCreate
    {
    
        public int OrderID { get; set; }
        public int SecurityId { get; set; }
        public int AccountID { get; set; }
        public int Quantite { get; set; }

        [Precision(16, 2)]

        public decimal Price { get; set; }
        public DateTime TransactionDate { get; set; }
        [BindNever]
        public List<SelectListItem> Securities { get; set; } = new List<SelectListItem>(); // Initialiser la liste

        [BindNever]
        public List<SelectListItem> Accounts { get; set; } = new List<SelectListItem>();  // Initialiser la liste
        [BindNever]
        public List<SelectListItem> Orders { get; set; } = new List<SelectListItem>(); // Initialiser la liste

      
    }
}
