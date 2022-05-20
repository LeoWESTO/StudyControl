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

namespace StudyControlWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PointsController : ControllerBase
    {
        private readonly DataContext _context;
        public PointsController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Points>>> GetPoints()
        {
            return await _context.Point.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Points>> GetPoints(int id)
        {
            var points = await _context.Point.FindAsync(id);

            if (points == null)
            {
                return NotFound();
            }

            return points;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPoints(int id, Points points)
        {
            if (id != points.Id)
            {
                return BadRequest();
            }

            _context.Entry(points).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PointsExists(id))
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
        public async Task<ActionResult<Points>> PostPoints(Points points)
        {
            _context.Point.Add(points);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPoints", new { id = points.Id }, points);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePoints(int id)
        {
            var points = await _context.Point.FindAsync(id);
            if (points == null)
            {
                return NotFound();
            }

            _context.Point.Remove(points);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PointsExists(int id)
        {
            return _context.Point.Any(e => e.Id == id);
        }
    }
}
