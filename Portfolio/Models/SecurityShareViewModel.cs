// ViewModels/SecurityShareCreateViewModel.cs
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Portfolio.ViewModels
{
    public class SecurityShareCreateViewModel
    {
        public int SecurityshareId { get; set; }
        public int AccountId { get; set; }
        public string AccountName { get; set; }
        public List<SelectListItem> Securities { get; set; }
        public int SecurityId { get; set; }
        [Precision(16, 2)]
        public decimal Quantite { get; set; }
    }
}
