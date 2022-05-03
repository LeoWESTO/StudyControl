#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SCLib.Models;
using StudyControlAPI.Data;

namespace StudyControlAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeansController : ControllerBase
    {
        private readonly DataContext _context;

        public DeansController(DataContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Dean>>> GetDeans()
        {
            return await _context.Deans.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Dean>> GetDean(int id)
        {
            var dean = await _context.Deans.FindAsync(id);

            if (dean == null)
            {
                return NotFound();
            }

            return dean;
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDean(int id, Dean dean)
        {
            if (id != dean.Id)
            {
                return BadRequest();
            }

            _context.Entry(dean).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DeanExists(id))
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
        [HttpPost]
        public async Task<ActionResult<Dean>> PostDean(Dean dean)
        {
            _context.Deans.Add(dean);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDean", new { id = dean.Id }, dean);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDean(int id)
        {
            var dean = await _context.Deans.FindAsync(id);
            if (dean == null)
            {
                return NotFound();
            }

            _context.Deans.Remove(dean);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        private bool DeanExists(int id)
        {
            return _context.Deans.Any(e => e.Id == id);
        }
    }
}
