using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project40_API_Dot_NET.Data;
using Project40_API_Dot_NET.Models;

namespace Project40_API_Dot_NET.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CameraBoxController : ControllerBase
    {
        private readonly PlantContext _context;

        public CameraBoxController(PlantContext context)
        {
            _context = context;
        }

        // GET: api/CameraBox
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CameraBox>>> GetCameraBoxes()
        {
            return await _context.CameraBoxes
                .Include(c => c.User)
                .ToListAsync();
        }

        // GET: api/CameraBox/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CameraBox>> GetCameraBox(int id)
        {
            var cameraBox = await _context.CameraBoxes
                .Include(c => c.User)
                .Where(c => c.Id == id)
                .FirstAsync();

            if (cameraBox == null)
            {
                return NotFound();
            }

            return cameraBox;
        }

        // GET: api/CameraBox/User/5
        [HttpGet("User/{userId}")]
        public async Task<ActionResult<IEnumerable<CameraBox>>> GetCameraBoxFromUser(int userId)
        {
            var cameraBoxes = await _context.CameraBoxes
                .Include(c => c.User)
                .Where(c => c.UserId == userId)
                .ToListAsync();

            if (cameraBoxes == null)
            {
                return NotFound();
            }

            return cameraBoxes;
        }

        // PUT: api/CameraBox/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCameraBox(int id, CameraBox cameraBox)
        {
            if (id != cameraBox.Id)
            {
                return BadRequest();
            }

            _context.Entry(cameraBox).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CameraBoxExists(id))
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

        // POST: api/CameraBox
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CameraBox>> PostCameraBox(CameraBox cameraBox)
        {
            _context.CameraBoxes.Add(cameraBox);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCameraBox", new { id = cameraBox.Id }, cameraBox);
        }

        // DELETE: api/CameraBox/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCameraBox(int id)
        {
            var cameraBox = await _context.CameraBoxes.FindAsync(id);
            if (cameraBox == null)
            {
                return NotFound();
            }

            _context.CameraBoxes.Remove(cameraBox);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CameraBoxExists(int id)
        {
            return _context.CameraBoxes.Any(e => e.Id == id);
        }
    }
}
