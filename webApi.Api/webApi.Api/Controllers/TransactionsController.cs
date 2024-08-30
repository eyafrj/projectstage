using DAL.EF;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace webApi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TransactionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Transactions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactions()
        {
            // Récupère toutes les transactions avec leurs comptes, titres et commandes associés
            var transactions = await _context.Transactions
                .Include(t => t.Account)
                .Include(t => t.Security)
                .Include(t => t.Order)
                .ToListAsync();

            // Retourne la liste des transactions
            return Ok(transactions);
        }

        // GET: api/Transactions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Transaction>> GetTransaction(int id)
        {
            // Recherche une transaction par ID avec ses détails associés
            var transaction = await _context.Transactions
                .Include(t => t.Account)
                .Include(t => t.Security)
                .Include(t => t.Order)
                .FirstOrDefaultAsync(t => t.TransactionID == id);

            if (transaction == null)
            {
                return NotFound(); // Retourne 404 si la transaction n'existe pas
            }

            // Retourne la transaction trouvée
            return Ok(transaction);
        }
    }

}