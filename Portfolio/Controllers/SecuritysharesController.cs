using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using Portfolio.Models;
using Portfolio.Services;
using Portfolio.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;

public class SecuritysharesController : Controller
{
    private readonly ApplicationDbContext context;

    public SecuritysharesController(ApplicationDbContext context)
    {
        this.context = context;
    }

    public IActionResult ViewSecurityShares(int accountId)
    {
        var securityShares = context.SecurityShares
                                     .Include(ss => ss.Account)
                                     .Include(ss => ss.Security)
                                     .Where(ss => ss.AccountId == accountId)
                                     .ToList();

        if (securityShares == null || !securityShares.Any())
        {
            return View(new List<SecurityShare>());
        }

        var accountName = context.Accounts
                                 .Where(a => a.AccountID == accountId)
                                 .Select(a => a.AccountName)
                                 .FirstOrDefault();

        ViewData["AccountName"] = accountName;

        return View(securityShares);
    }

    [HttpGet]
    public IActionResult CreateSecurityshares(int accountId)
    {
        var account = context.Accounts.Find(accountId);
        if (account == null)
        {
            return NotFound();
        }

        var viewModel = new SecurityShareCreateViewModel
        {
            AccountId = accountId,
            AccountName = account.AccountName,
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
    public async Task<IActionResult> CreateSecurityshares(SecurityShareCreateViewModel viewModel)
    {
        // Log the incoming data for debugging
        Console.WriteLine($"AccountId: {viewModel.AccountId}, SecurityId: {viewModel.SecurityId}, Quantite: {viewModel.Quantite}");

        if (viewModel.Quantite == 0)
        {
            ModelState.AddModelError("", "Quantite cannot be zero");
            viewModel.Securities = context.securities.Select(s => new SelectListItem
            {
                Value = s.SecurityId.ToString(),
                Text = s.SecurityName
            }).ToList();
            return View(viewModel);
        }

        // Create a new instance of SecurityShare with the form values
        var securityShare = new SecurityShare
        {
            AccountId = viewModel.AccountId,
            SecurityId = viewModel.SecurityId,
            Quantite = viewModel.Quantite
        };

        // Add the new SecurityShare to the database
        context.SecurityShares.Add(securityShare);

        // Save changes to the database
        await context.SaveChangesAsync();

        // Update the TotalMarketValue for the account
        await context.UpdateTotalMarketValueAsync(viewModel.AccountId);

        // Redirect to ViewSecurityShares after creation
        return RedirectToAction("ViewSecurityShares", new { accountId = viewModel.AccountId });
    }
    [HttpGet]
    public IActionResult Edit(int accountId, int securityId)
    {
        var securityShare = context.SecurityShares
            .FirstOrDefault(ss => ss.AccountId == accountId && ss.SecurityId == securityId);

        if (securityShare == null)
        {
            return NotFound();
        }

        var viewModel = new SecurityShareCreateViewModel
        {
            SecurityshareId = securityShare.SecurityshareId,
            AccountId = securityShare.AccountId,
            SecurityId = securityShare.SecurityId,
            Quantite = securityShare.Quantite
        };

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(SecurityShareCreateViewModel viewModel)
    {
        var existingSecurityShare = context.SecurityShares
            .FirstOrDefault(ss => ss.SecurityshareId == viewModel.SecurityshareId);

        if (existingSecurityShare != null)
        {
            existingSecurityShare.Quantite = viewModel.Quantite;
            await context.SaveChangesAsync();

            // Update the TotalMarketValue for the account
            await context.UpdateTotalMarketValueAsync(viewModel.AccountId);
        }

        return RedirectToAction("ViewSecurityShares", new { accountId = viewModel.AccountId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int accountId, int securityId)
    {
        var securityShare = context.SecurityShares
            .FirstOrDefault(ss => ss.AccountId == accountId && ss.SecurityId == securityId);

        if (securityShare == null)
        {
            return RedirectToAction("ViewSecurityShares", new { accountId });
        }

        context.SecurityShares.Remove(securityShare);
        await context.SaveChangesAsync();

        // Update the TotalMarketValue for the account
        await context.UpdateTotalMarketValueAsync(accountId);

        return RedirectToAction("ViewSecurityShares", new { accountId });
    }
}
