using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using Portfolio.Models;
using Portfolio.Services;


namespace Portfolio.Controllers
{
    public class AccountsController : Controller
    {
        private ApplicationDbContext context;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AccountsController> _logger;
        public AccountsController(ApplicationDbContext context , ILogger<AccountsController> logger)
        {
            this.context = context;
            _context = context;
            this._logger = logger;

        }
        public IActionResult Accounts()
        {
            var Accounts = context.Accounts.ToList();
            return View(Accounts);
        }
       
        public IActionResult Create()
        {

            return View();
        }
        [HttpPost]

        public IActionResult Create(Account account) // Assurez-vous que le nom de la classe est correct
        {
            if (!ModelState.IsValid)
            {
                account.TotalMarketValue = 0;
                return View(account);
            }

            // Créer un nouvel objet Accounts avec les données fournies
            Account newAccount = new Account()
            {
                AccountName = account.AccountName,
                AccountType = account.AccountType,
                TotalMarketValue = account.TotalMarketValue,
                CreatedDate = DateTime.Now,

            };

            // Ajouter le nouvel objet à la base de données
            context.Accounts.Add(newAccount);
            context.SaveChanges();

            // Rediriger vers l'action "Accounts" après la création réussie
            return RedirectToAction("Accounts");
        }




        public IActionResult Ediit(int AccountID)
        {
            var Ac = context.Accounts.Find(AccountID);
            if (Ac == null)
            {
                return RedirectToAction("Accounts");
            }

            var AccountUp = new AccountUp
            {
                AccountName = Ac.AccountName,
                AccountType = Ac.AccountType, // Assurez-vous que la casse est correcte
                TotalMarketValue = Ac.TotalMarketValue,
                ModifiedDate = Ac.ModifiedDate,
            };
            ViewData["AccountID"] = Ac.AccountID;
            ViewData["CreatedDate"] = Ac.CreatedDate.ToString("MM/dd/yyyy");

            return View(AccountUp);
        }
        [HttpPost]
        public IActionResult Ediit(int accountID, AccountUp AccountUp)
        {
            // Trouver l'account existant
            var existingAccount = context.Accounts.Find(accountID);
            if (existingAccount == null)
            {
                // Si l'account n'est pas trouvé, rediriger vers la liste des comptes
                return RedirectToAction("Accounts");
            }

            if (!ModelState.IsValid)
            {
                // Re-assigner AccountID et CreatedDate à ViewData pour conserver les valeurs
                ViewData["AccountID"] = accountID;
                ViewData["CreatedDate"] = existingAccount.CreatedDate.ToString("MM/dd/yyyy");

                // Retourner la vue avec le modèle en cas d'erreur de validation
                return View(AccountUp);
            }

            // Mettre à jour les propriétés de l'account avec les valeurs du modèle
            existingAccount.AccountName = AccountUp.AccountName;
            existingAccount.AccountType = AccountUp.AccountType;
            existingAccount.TotalMarketValue = AccountUp.TotalMarketValue;
            existingAccount.ModifiedDate = DateTime.Now; // Mettez à jour la date de modification

            // Sauvegarder les modifications dans la base de données
            context.SaveChanges();

            // Rediriger vers la liste des comptes après la mise à jour réussie
            return RedirectToAction("Accounts");
        }
        public IActionResult Delete(int accountId)
        {
            try
            {
                var account = context.Accounts.Find(accountId);
                if (account == null)
                {
                    TempData["Message"] = "Account not found.";
                    TempData["MessageType"] = "danger";
                    return RedirectToAction("Accounts");
                }

                // Vérifier s'il y a des transactions associées à ce compte (si applicable)
                var hasTransactions = context.Transactions.Any(t => t.AccountID == accountId);
                if (hasTransactions)
                {
                    TempData["Message"] = "Cannot delete the account because it has associated transactions.";
                    TempData["MessageType"] = "danger";
                    return RedirectToAction("Accounts");
                }

                context.Accounts.Remove(account);
                context.SaveChanges();

                TempData["Message"] = "Account deleted successfully.";
                TempData["MessageType"] = "success";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the account.");
                TempData["Message"] = "An error occurred while deleting the account. Please try again later.";
                TempData["MessageType"] = "danger";
            }

            return RedirectToAction("Accounts");
        }




    }



}









