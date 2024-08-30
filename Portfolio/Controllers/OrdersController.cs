using Microsoft.AspNetCore.Mvc;
using Portfolio.Models;
using Portfolio.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Portfolio.ViewModels;
using Microsoft.Extensions.Logging;

public class OrdersController : Controller
{
    private readonly ApplicationDbContext context;
    private readonly ILogger<OrdersController> _logger;
    public OrdersController(ApplicationDbContext context, ILogger<OrdersController> logger)
    {
        this.context = context;
        _logger = logger;
    }

    // Liste de toutes les commandes
    public IActionResult Index()
    {
        var orders = context.Orders.Include(o => o.Account).Include(o => o.Security).ToList();
        return View(orders);
    }

    // Création d'une nouvelle commande


    public IActionResult Create()
    {
        var viewModel = new OrderCreate
        {
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
    public async Task<IActionResult> Create(OrderCreate viewModel)
    {
        if (!ModelState.IsValid)
        {
            // Repopuler les listes en cas d'erreur
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

        // Création de l'ordre avec "OrderStatus" fixé à "pending"
        var order = new Order
        {
            AccountID = viewModel.AccountID,
            SecurityId = viewModel.SecurityId,
            OrderType = viewModel.OrderType,
            Quantity = viewModel.Quantity,
            OrderStatus = "pending", // Statut par défaut à "pending"
            OrderDate = DateTime.Now,
            BrokerID = viewModel.BrokerID
        };

        context.Orders.Add(order);
        await context.SaveChangesAsync();

        return RedirectToAction("Index");
    }

    // Suppression d'une commande
    public IActionResult Delete(int orderId)
    {
        try
        {
            var order = context.Orders.Find(orderId);
            if (order == null)
            {
                TempData["Message"] = "Order not found.";
                TempData["MessageType"] = "danger";
                return RedirectToAction("Index");
            }

            // Vérifier s'il y a des transactions associées à la commande
            var hasTransactions = context.Transactions.Any(t => t.OrderID == orderId);
            if (hasTransactions)
            {
                TempData["Message"] = "Cannot delete the order because it has associated transactions.";
                TempData["MessageType"] = "danger";
                return RedirectToAction("Index");
            }

            context.Orders.Remove(order);
            context.SaveChanges();

            TempData["Message"] = "Order deleted successfully.";
            TempData["MessageType"] = "success";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while deleting the order.");
            TempData["Message"] = "An error occurred while deleting the order. Please try again later.";
            TempData["MessageType"] = "danger";
        }

        return RedirectToAction("Index");
    }

    /*  public async Task<IActionResult> Execute(int orderId)
      {
          // Récupérer l'ordre par son ID
          var order = await context.Orders
              .Include(o => o.Account)
              .Include(o => o.Security)
              .FirstOrDefaultAsync(o => o.OrderID == orderId);

          if (order == null)
          {
              return NotFound();
          }

          // Vérifier si l'ordre est déjà exécuté
          if (order.OrderStatus == "executed")
          {
              ModelState.AddModelError("", "This order has already been executed.");
              return RedirectToAction("Index");
          }

          // Récupérer les SecurityShares correspondants à cet utilisateur
          var securityShare = await context.SecurityShares
              .FirstOrDefaultAsync(ss => ss.AccountId == order.AccountID && ss.SecurityId == order.SecurityId);

          if (order.OrderType == "buy")
          {
              // Acheter: augmenter la quantité de la sécurité
              if (securityShare != null)
              {
                  securityShare.Quantite += order.Quantity;
                  Console.WriteLine($"SecurityShare trouvé, nouvelle quantité: {securityShare.Quantite}");
              }
              else
              {
                  // Si l'utilisateur n'a pas encore cette sécurité, créer une nouvelle entrée
                  var newShare = new SecurityShare
                  {
                      AccountId = order.AccountID,
                      SecurityId = order.SecurityId,
                      Quantite = order.Quantity
                  };
                  context.SecurityShares.Add(newShare);
                  Console.WriteLine($"Nouvelle SecurityShare créée avec quantité: {order.Quantity}");
              }
          }
          else if (order.OrderType == "sell")
          {
              // Vendre: diminuer la quantité de la sécurité
              if (securityShare == null)
              {
                  ModelState.AddModelError("", "SecurityShare not found for selling.");
                  Console.WriteLine("Erreur: SecurityShare introuvable pour cette vente.");
                  return RedirectToAction("Index");
              }

              Console.WriteLine($"Quantité actuelle de SecurityShare: {securityShare.Quantite}, Quantité à vendre: {order.Quantity}");

              if (securityShare.Quantite < order.Quantity)
              {
                  ModelState.AddModelError("", "Not enough shares to sell.");
                  Console.WriteLine("Erreur: Pas assez de SecurityShares pour vendre.");
                  return RedirectToAction("Index");
              }

              // Réduire la quantité
              securityShare.Quantite -= order.Quantity;
              Console.WriteLine($"SecurityShare mise à jour, nouvelle quantité après vente: {securityShare.Quantite}");

              // Si la quantité tombe à 0, supprimer l'entrée de SecurityShare
              if (securityShare.Quantite == 0)
              {
                  context.SecurityShares.Remove(securityShare);
                  Console.WriteLine($"SecurityShare supprimée car la quantité est à 0.");
              }
          }

          // Mettre à jour l'état de la commande en "executed"
          order.OrderStatus = "executed";
          order.ExecutionDate = DateTime.Now;

          try
          {
              await context.SaveChangesAsync();
              Console.WriteLine("Changements sauvegardés avec succès.");
          }
          catch (Exception ex)
          {
              Console.WriteLine($"Erreur lors de la sauvegarde: {ex.Message}");
              ModelState.AddModelError("", "Error while saving changes.");
              return RedirectToAction("Index");
          }

          return RedirectToAction("Index");
      }

      */
    /*
    public async Task<IActionResult> Execute(int orderId)
    {
        var order = await context.Orders
            .Include(o => o.Account)
            .Include(o => o.Security)
            .FirstOrDefaultAsync(o => o.OrderID == orderId);

        if (order == null)
        {
            _logger.LogError($"Order with ID {orderId} not found.");
            TempData["MessageType"] = "error";
            TempData["Message"] = "Order not found.";
            return RedirectToAction("Index");
        }

        if (order.OrderStatus == "executed")
        {
            _logger.LogWarning($"Order with ID {orderId} has already been executed.");
            TempData["MessageType"] = "warning";
            TempData["Message"] = "Order has already been executed.";
            return RedirectToAction("Index");
        }

        var securityShare = await context.SecurityShares
            .FirstOrDefaultAsync(ss => ss.AccountId == order.AccountID && ss.SecurityId == order.SecurityId);

        if (order.OrderType == "buy")
        {
            if (securityShare != null)
            {
                securityShare.Quantite += order.Quantity;
                _logger.LogInformation($"Increased quantity of SecurityShare for AccountID {order.AccountID} and SecurityId {order.SecurityId}.");
            }
            else
            {
                var newShare = new SecurityShare
                {
                    AccountId = order.AccountID,
                    SecurityId = order.SecurityId,
                    Quantite = order.Quantity
                };
                context.SecurityShares.Add(newShare);
                _logger.LogInformation($"Created new SecurityShare for AccountID {order.AccountID} and SecurityId {order.SecurityId}.");
            }
        }
        else if (order.OrderType == "sell")
        {
            if (securityShare == null || securityShare.Quantite < order.Quantity)
            {
                _logger.LogError("Not enough shares to sell or SecurityShare not found.");
                TempData["MessageType"] = "error";
                TempData["Message"] = "Not enough shares to sell or SecurityShare not found.";
                return RedirectToAction("Index");
            }
            securityShare.Quantite -= order.Quantity;
            _logger.LogInformation($"Decreased quantity of SecurityShare for AccountID {order.AccountID} and SecurityId {order.SecurityId}.");
            if (securityShare.Quantite == 0)
            {
                context.SecurityShares.Remove(securityShare);
                _logger.LogInformation($"Removed SecurityShare for AccountID {order.AccountID} and SecurityId {order.SecurityId} as quantity is zero.");
            }
        }
        else
        {
            _logger.LogError("Unknown order type.");
            TempData["MessageType"] = "error";
            TempData["Message"] = "Unknown order type.";
            return RedirectToAction("Index");
        }

        order.OrderStatus = "executed";
        order.ExecutionDate = DateTime.Now;

        var transaction = new Transaction
        {
            OrderID = order.OrderID,
            SecurityId = order.SecurityId,
            AccountID = order.AccountID,
            Quantite = order.Quantity,
            Price = 100, // Assurez-vous d'avoir un prix valide, peut-être à partir de l'ordre ou d'une autre source
            TransactionDate = order.ExecutionDate // Utiliser la date d'exécution de l'ordre comme date de transaction
        };

        _logger.LogInformation($"Creating transaction for OrderID {order.OrderID}.");
        context.Transactions.Add(transaction);

        try
        {
            await context.SaveChangesAsync();
            _logger.LogInformation("Transaction saved successfully.");
            TempData["MessageType"] = "success";
            TempData["Message"] = "Order executed and transaction saved successfully.";
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error saving transaction: {ex.Message}");
            TempData["MessageType"] = "error";
            TempData["Message"] = "Error while saving transaction.";
        }

        return RedirectToAction("Index");
    }*/
    public async Task<IActionResult> Execute(int orderId)
    {
        var order = await context.Orders
            .Include(o => o.Account)
            .Include(o => o.Security)
            .FirstOrDefaultAsync(o => o.OrderID == orderId);

        if (order == null)
        {
            _logger.LogError($"Order with ID {orderId} not found.");
            TempData["MessageType"] = "error";
            TempData["Message"] = "Order not found.";
            return RedirectToAction("Index");
        }

        if (order.OrderStatus == "executed")
        {
            _logger.LogWarning($"Order with ID {orderId} has already been executed.");
            TempData["MessageType"] = "warning";
            TempData["Message"] = "Order has already been executed.";
            return RedirectToAction("Index");
        }

        var securityShare = await context.SecurityShares
            .FirstOrDefaultAsync(ss => ss.AccountId == order.AccountID && ss.SecurityId == order.SecurityId);

        // Fetch the current price of the security
        var security = await context.securities
            .FirstOrDefaultAsync(s => s.SecurityId == order.SecurityId);

        if (security == null)
        {
            _logger.LogError($"Security with ID {order.SecurityId} not found.");
            TempData["MessageType"] = "error";
            TempData["Message"] = "Security not found.";
            return RedirectToAction("Index");
        }

        decimal currentPrice = security.currentPrice;

        if (order.OrderType == "buy")
        {
            if (securityShare != null)
            {
                securityShare.Quantite += order.Quantity;
                _logger.LogInformation($"Increased quantity of SecurityShare for AccountID {order.AccountID} and SecurityId {order.SecurityId}.");
            }
            else
            {
                var newShare = new SecurityShare
                {
                    AccountId = order.AccountID,
                    SecurityId = order.SecurityId,
                    Quantite = order.Quantity
                };
                context.SecurityShares.Add(newShare);
                _logger.LogInformation($"Created new SecurityShare for AccountID {order.AccountID} and SecurityId {order.SecurityId}.");
            }
        }
        else if (order.OrderType == "sell")
        {
            if (securityShare == null || securityShare.Quantite < order.Quantity)
            {
                _logger.LogError("Not enough shares to sell or SecurityShare not found.");
                TempData["MessageType"] = "error";
                TempData["Message"] = "Not enough shares to sell or SecurityShare not found.";
                return RedirectToAction("Index");
            }
            securityShare.Quantite -= order.Quantity;
            _logger.LogInformation($"Decreased quantity of SecurityShare for AccountID {order.AccountID} and SecurityId {order.SecurityId}.");
            if (securityShare.Quantite == 0)
            {
                context.SecurityShares.Remove(securityShare);
                _logger.LogInformation($"Removed SecurityShare for AccountID {order.AccountID} and SecurityId {order.SecurityId} as quantity is zero.");
            }
        }
        else
        {
            _logger.LogError("Unknown order type.");
            TempData["MessageType"] = "error";
            TempData["Message"] = "Unknown order type.";
            return RedirectToAction("Index");
        }

        order.OrderStatus = "executed";
        order.ExecutionDate = DateTime.Now;

        var transaction = new Transaction
        {
            OrderID = order.OrderID,
            SecurityId = order.SecurityId,
            AccountID = order.AccountID,
            Quantite = order.Quantity,
            Price = currentPrice, // Set the price to the current price of the security
            TransactionDate = order.ExecutionDate // Use the order's execution date as the transaction date
        };

        _logger.LogInformation($"Creating transaction for OrderID {order.OrderID}.");
        context.Transactions.Add(transaction);

        try
        {
            await context.SaveChangesAsync();
            _logger.LogInformation("Transaction saved successfully.");
            TempData["MessageType"] = "success";
            TempData["Message"] = "Order executed and transaction saved successfully.";
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error saving transaction: {ex.Message}");
            TempData["MessageType"] = "error";
            TempData["Message"] = "Error while saving transaction.";
        }

        return RedirectToAction("Index");
    }


}