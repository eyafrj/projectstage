using BLL.core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BLL.core.Models;

namespace webApi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IBaseRepository<Account> _accountRepository;
        public AccountsController(IBaseRepository<Account> accountRepository)
        {
            _accountRepository = accountRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var accounts = await _accountRepository.GetAllAsync();
            return Ok(accounts);
        }

        // GET: api/Account/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var account = await _accountRepository.GetByIdAsync(id);
            if (account == null)
            {
                return NotFound();
            }
            return Ok(account);
        }

        // POST: api/Account
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Account account)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _accountRepository.AddAsync(account);
            return CreatedAtAction(nameof(GetById), new { id = account.AccountID }, account);
        }

        // PUT: api/Account/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Account account)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingAccount = await _accountRepository.GetByIdAsync(id);
            if (existingAccount == null)
            {
                return NotFound();
            }

            existingAccount.AccountName = account.AccountName;
            existingAccount.AccountType = account.AccountType;
            existingAccount.TotalMarketValue = account.TotalMarketValue;
            existingAccount.CreatedDate = account.CreatedDate;
            existingAccount.ModifiedDate = account.ModifiedDate;

            await _accountRepository.UpdateAsync(existingAccount);
            return NoContent();
        }

        // DELETE: api/Account/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var account = await _accountRepository.GetByIdAsync(id);
            if (account == null)
            {
                return NotFound();
            }

            await _accountRepository.DeleteAsync(id);
            return NoContent();
        }
    
}
}
