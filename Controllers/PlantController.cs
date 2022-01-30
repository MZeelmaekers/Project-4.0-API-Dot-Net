using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project40_API_Dot_NET.Data;
using Project40_API_Dot_NET.Models;
using Microsoft.AspNetCore.Authorization;

namespace Project40_API_Dot_NET.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlantController : ControllerBase
    {
        private readonly PlantContext _context;

        public PlantController(PlantContext context)
        {
            _context = context;
        }

        // GET: api/Plant
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Plant>>> GetPlants()
        {
            return await _context.Plants
                .Include(p => p.User)
                .Include(p => p.Result)
                .ToListAsync();
        }

        // GET: api/Plant/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Plant>> GetPlant(int id)
        {
            var plant = await _context.Plants
                .Include(p => p.User)
                .Include(p => p.Result)
                .Where(p => p.Id == id)
                .FirstAsync();

            if (plant == null)
            {
                return NotFound();
            }

            return plant;
        }

        // GET: api/Plant/User/5
        [Authorize]
        [HttpGet("User/{id}")]
        public async Task<ActionResult<IEnumerable<Plant>>> GetPlantFromUser(int id)
        {
            var plants = await _context.Plants
                .Include(p => p.User)
                .Include(p => p.Result)
                .Where(p => p.UserId == id)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();

            if (plants == null)
            {
                return NotFound();
            }

            return plants;
        }

        // PUT: api/Plant/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPlant(int id, Plant plant)
        {
            if (id != plant.Id)
            {
                return BadRequest();
            }

            _context.Entry(plant).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlantExists(id))
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

        // POST: api/Plant
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Plant>> PostPlant(Plant plant)
        {
            _context.Plants.Add(plant);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPlant", new { id = plant.Id }, plant);
        }

        // DELETE: api/Plant/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlant(int id)
        {
            var plant = await _context.Plants.FindAsync(id);
            if (plant == null)
            {
                return NotFound();
            }

            _context.Plants.Remove(plant);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PlantExists(int id)
        {
            return _context.Plants.Any(e => e.Id == id);
        }
    }
}
