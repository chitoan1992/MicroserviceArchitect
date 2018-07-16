using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AccountingService.Models;

namespace AccountingService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountingsController : ControllerBase
    {
        private readonly AccountingContext _context;

        public AccountingsController(AccountingContext context)
        {
            _context = context;
        }

        // GET: api/Accountings
        [HttpGet]
        public IEnumerable<Accounting> GetAccounting()
        {
            return _context.Accounting;
        }

        // GET: api/Accountings/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAccounting([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var accounting = await _context.Accounting.FindAsync(id);

            if (accounting == null)
            {
                return NotFound();
            }

            return Ok(accounting);
        }

        // PUT: api/Accountings/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAccounting([FromRoute] int id, [FromBody] Accounting accounting)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != accounting.Id)
            {
                return BadRequest();
            }

            _context.Entry(accounting).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccountingExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Accountings
        [HttpPost]
        public async Task<IActionResult> PostAccounting([FromBody] Accounting accounting)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Accounting.Add(accounting);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAccounting", new { id = accounting.Id }, accounting);
        }

        // DELETE: api/Accountings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccounting([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var accounting = await _context.Accounting.FindAsync(id);
            if (accounting == null)
            {
                return NotFound();
            }

            _context.Accounting.Remove(accounting);
            await _context.SaveChangesAsync();

            return Ok(accounting);
        }

        private bool AccountingExists(int id)
        {
            return _context.Accounting.Any(e => e.Id == id);
        }
    }
}