using BLL.core.Interfaces;
using BLL.core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace webApi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class SecuritySharesController : ControllerBase
    {
        private readonly IBaseRepository<SecurityShare> _securityShareRepository;

        public SecuritySharesController(IBaseRepository<SecurityShare> securityShareRepository)
        {
            _securityShareRepository = securityShareRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var securityShares = await _securityShareRepository.GetAllAsync();
            return Ok(securityShares);
        }

        // GET: api/SecurityShares/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var securityShare = await _securityShareRepository.GetByIdAsync(id);
            if (securityShare == null)
            {
                return NotFound();
            }
            return Ok(securityShare);
        }

        // POST: api/SecurityShares
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] SecurityShare securityShare)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _securityShareRepository.AddAsync(securityShare);
            return CreatedAtAction(nameof(GetById), new { id = securityShare.SecurityshareId }, securityShare);
        }

        // PUT: api/SecurityShares/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] SecurityShare securityShare)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingSecurityShare = await _securityShareRepository.GetByIdAsync(id);
            if (existingSecurityShare == null)
            {
                return NotFound();
            }

            existingSecurityShare.AccountId = securityShare.AccountId;
            existingSecurityShare.SecurityId = securityShare.SecurityId;
            existingSecurityShare.Quantite = securityShare.Quantite;

            await _securityShareRepository.UpdateAsync(existingSecurityShare);
            return NoContent();
        }

        // DELETE: api/SecurityShares/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var securityShare = await _securityShareRepository.GetByIdAsync(id);
            if (securityShare == null)
            {
                return NotFound();
            }

            await _securityShareRepository.DeleteAsync(id);
            return NoContent();
        }
    }

}
