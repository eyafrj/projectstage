using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Portfolio.Models;
using Portfolio.Services;
using Portfolio.ViewModels;

namespace Portfolio.Controllers
{
    public class TransactionsController : Controller
    {
        private readonly ApplicationDbContext context;
      
        private readonly ILogger<TransactionsController> _logger;
        public TransactionsController(ApplicationDbContext context, ILogger<TransactionsController> logger)
        {
            this.context = context;
            _logger = logger;

        }

        public IActionResult Index()
        {
            var transactions = context.Transactions
                .Include(t => t.Order)
                .Include(t => t.Security)
                .Include(t => t.Account)
                .ToList();

            if (!transactions.Any())
            {
                _logger.LogInformation("No transactions found.");
            }

            return View(transactions);
        }



        public IActionResult Create()
        {
            var viewModel = new TransactionCreate
            {
                Orders = context.Orders.Select(o => new SelectListItem
                {
                    Value = o.OrderID.ToString(),
                    Text = o.OrderID.ToString() // Vous pouvez adapter cela pour afficher plus d'informations sur la commande
                }).ToList(),

                Accounts = context.Accounts.Select(a => new SelectListItem
                {
                    Value = a.AccountID.ToString(),
                    Text = a.AccountName
                }).ToList(),

                Securities = context.securities.Select(s => new SelectListItem
                {
                    Value = s.SecurityId.ToString(),
                    Text = s.SecurityName
                }).ToList()
            };

            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TransactionCreate viewModel)
        {
            if (!ModelState.IsValid)
            {
                // Repopuler les listes en cas d'erreur
                viewModel.Orders = context.Orders.Select(o => new SelectListItem
                {
                    Value = o.OrderID.ToString(),
                    Text = o.OrderID.ToString()
                }).ToList();

                viewModel.Accounts = context.Accounts.Select(a => new SelectListItem
                {
                    Value = a.AccountID.ToString(),
                    Text = a.AccountName
                }).ToList();

                viewModel.Securities = context.securities.Select(s => new SelectListItem
                {
                    Value = s.SecurityId.ToString(),
                    Text = s.SecurityName
                }).ToList();

                return View(viewModel);
            }

            var transaction = new Transaction
            {
                OrderID = viewModel.OrderID,
                AccountID = viewModel.AccountID,
                SecurityId = viewModel.SecurityId,
                Quantite = viewModel.Quantite,
                Price = viewModel.Price,
                TransactionDate = DateTime.Now
            };

            context.Transactions.Add(transaction);
            await context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var transaction = await context.Transactions.FindAsync(id);
            if (transaction != null)
            {
                context.Transactions.Remove(transaction);
                await context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }



    }
}
