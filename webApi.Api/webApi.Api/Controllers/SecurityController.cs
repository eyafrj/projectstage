using BLL.core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BLL.core.Models;

namespace webApi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SecurityController : ControllerBase
    {
        private readonly IBaseRepository<Security> _securityRepository;
        public SecurityController(IBaseRepository<Security> securityRepository)
        {
            _securityRepository = securityRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var securities = await _securityRepository.GetAllAsync();
            return Ok(securities);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var security = await _securityRepository.GetByIdAsync(id);
            if (security == null)
            {
                return NotFound();
            }
            return Ok(security);
        }
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Security security)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _securityRepository.AddAsync(security);
            return CreatedAtAction(nameof(GetById), new { id = security.SecurityId }, security);
        }
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Security security)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingSecurity = await _securityRepository.GetByIdAsync(id);
            if (existingSecurity == null)
            {
                return NotFound();
            }

            existingSecurity.SecurityName = security.SecurityName;
            existingSecurity.symbol = security.symbol;
            existingSecurity.SecurityType = security.SecurityType;
            existingSecurity.currentPrice = security.currentPrice;
            existingSecurity.MarketValue = security.MarketValue;
            existingSecurity.Lastupdate = security.Lastupdate;

            await _securityRepository.UpdateAsync(existingSecurity);
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var security = await _securityRepository.GetByIdAsync(id);
            if (security == null)
            {
                return NotFound();
            }

            await _securityRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}
