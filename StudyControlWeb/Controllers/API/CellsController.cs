#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudyControlWeb.Models;
using StudyControlWeb.Data;
using StudyControlWeb.Models.DBO;

namespace StudyControlWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CellsController : ControllerBase
    {
        private readonly DataContext _context;

        public CellsController(DataContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cell>>> GetCells()
        {
            return await _context.Cell.ToListAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Cell>> GetCell(int id)
        {
            var cell = await _context.Cell.FindAsync(id);

            if (cell == null)
            {
                return NotFound();
            }

            return cell;
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCell(int id, Cell cell)
        {
            if (id != cell.Id)
            {
                return BadRequest();
            }

            _context.Entry(cell).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CellExists(id))
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
        public async Task<ActionResult<Cell>> PostCell(Cell cell)
        {
            _context.Cell.Add(cell);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCell", new { id = cell.Id }, cell);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCell(int id)
        {
            var cell = await _context.Cell.FindAsync(id);
            if (cell == null)
            {
                return NotFound();
            }

            _context.Cell.Remove(cell);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        private bool CellExists(int id)
        {
            return _context.Cell.Any(e => e.Id == id);
        }
    }
}
