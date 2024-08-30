using Microsoft.AspNetCore.Mvc;
using Portfolio.Models;
using Portfolio.Services;

namespace Portfolio.Controllers
{
	public class SecurityController : Controller
	{
		private ApplicationDbContext context;
		private readonly IWebHostEnvironment environment;
        private readonly ILogger<SecurityController> _logger;

        public SecurityController(ApplicationDbContext context, IWebHostEnvironment environment, ILogger<SecurityController> logger)
		{
			this.context = context;
			this.environment = environment;
            this._logger = logger;
        }
		//retourne le view d'affichage
		public IActionResult Index()
		{
			var securites = context.securities.OrderByDescending(p => p.SecurityId).ToList();

			return View(securites);
		}

		public IActionResult Create()
		{
			return View();

		}
		//controle de saisie 
		[HttpPost]
		public IActionResult Create(SecurityAdd securityAdd)
		{
			if (!ModelState.IsValid)
			{
				return View(securityAdd);

			}
			var security = new Security
			{

				SecurityName = securityAdd.SecurityName,
				symbol = securityAdd.symbol,
				SecurityType = securityAdd.SecurityType,
				currentPrice = securityAdd.currentPrice,
				MarketValue = securityAdd.MarketValue,
				Lastupdate = DateTime.Now
			};

			context.securities.Add(security);
			context.SaveChanges();


			return RedirectToAction("Index");

		}
		public IActionResult Edit(int SecurityId)
		{
			var Security = context.securities.Find(SecurityId);
			if (Security == null)
			{
				return RedirectToAction("Index");
			}

			var SecurityAdd = new SecurityAdd
			{
				SecurityName = Security.SecurityName,
				symbol = Security.symbol, // Assurez-vous que la casse est correcte
				SecurityType = Security.SecurityType,
				currentPrice = Security.currentPrice, // Assurez-vous que la casse est correcte
				MarketValue = Security.MarketValue
			};
			ViewData["SecurityId"] = Security.SecurityId;
			ViewData["Lastupdate"] =Security.Lastupdate.ToString("MM/dd/yyyy");

			return View(SecurityAdd);
		}
        [HttpPost]
        public IActionResult Edit(int SecurityId, SecurityAdd SecurityAdd)
        {
            var Security = context.securities.Find(SecurityId);
            if (Security == null)
            {
                return RedirectToAction("Index");
            }

            if (!ModelState.IsValid)
            {
                // Re-assign SecurityId and Lastupdate to ViewData
                ViewData["SecurityId"] = Security.SecurityId;
                ViewData["Lastupdate"] = Security.Lastupdate.ToString("MM/dd/yyyy");

                // Return view with the same model (SecurityAdd) to show validation errors
                return View(SecurityAdd);
            }

            // Update Security with values from SecurityAdd
            Security.SecurityName = SecurityAdd.SecurityName;
            Security.symbol = SecurityAdd.symbol;
            Security.SecurityType = SecurityAdd.SecurityType;
            Security.currentPrice = SecurityAdd.currentPrice;
            Security.MarketValue = SecurityAdd.MarketValue;

            // Save changes to the database
            context.SaveChanges();

            return RedirectToAction("Index");
        }
        public IActionResult Delete(int SecurityId)
        {
            try
            {
                var security = context.securities.Find(SecurityId);
                if (security == null)
                {
                    TempData["ErrorMessage"] = "Security not found.";
                    return RedirectToAction("Index");
                }

                // Vérifier si la sécurité a des transactions associées
                var hasTransactions = context.Transactions.Any(t => t.OrderID == SecurityId);
                if (hasTransactions)
                {
                    TempData["ErrorMessage"] = "Cannot delete security because it has associated transactions.";
                    return RedirectToAction("Index");
                }

                context.securities.Remove(security);
                context.SaveChanges();

                TempData["SuccessMessage"] = "Security deleted successfully.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the security.");
                TempData["ErrorMessage"] = "Cannot delete security because it has associated transactions.";
                return RedirectToAction("Index");
            }
        }


    }
}
