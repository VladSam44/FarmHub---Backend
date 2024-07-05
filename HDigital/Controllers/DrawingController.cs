using HDigital.Context;
using HDigital.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HDigital.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]

    public class DrawingController : ControllerBase
    {
        private readonly AppDbContext _context;
        private bool DrawingExists(int id)
        {
            return _context.Drawings.Any(e => e.Id == id);
        }
        public DrawingController(AppDbContext context)
        {
            _context = context;
        }
        [Authorize]
        [HttpGet("GET")]
        public async Task<ActionResult<IEnumerable<Drawing>>> GetDrawings()
        {
            var userIdentity = HttpContext.User.Identity as ClaimsIdentity;
            var userIdClaim = userIdentity.FindFirst("userId");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            {
                return BadRequest("Nu se poate prelua ID-ul utilizatorului.");
            }
            Console.WriteLine(userId);
            var drawings = await _context.Drawings
                                        .Where(d => d.UserId == userId)
                                        .ToListAsync();

            return drawings;
        }

        [HttpGet("{id}")] //read
        public async Task<ActionResult<Drawing>> GetDrawing(int id)
        {
            var drawing = await _context.Drawings.FindAsync(id);

            if (drawing == null)
            {
                return NotFound();
            }

            return drawing;
        }

        [Authorize]
        [HttpPost("POST")]
        public async Task<ActionResult<Drawing>> PostDrawing([FromBody] Drawing drawing)
        {

            if (User == null)
            {
                return Unauthorized();
            }
            var userIdentity = HttpContext.User.Identity as ClaimsIdentity;
            var userIdClaim = userIdentity.FindFirst("userId");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            {
                return BadRequest("Unable to retrieve user ID.");
            }


            drawing.UserId = userId;


            _context.Drawings.Add(drawing);
            await _context.SaveChangesAsync();


            return CreatedAtAction("GetDrawing", new { id = drawing.Id }, drawing);
        }


        [Authorize]
        [HttpPut("UPDATE")] //actualizare
        public async Task<IActionResult> PutDrawing([FromBody]Drawing updatedDrawing)
        {
            var userIdentity = HttpContext.User.Identity as ClaimsIdentity;
            var userIdClaim = userIdentity.FindFirst("userId");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            {
                return Unauthorized("Unauthorized access.");
            }
            var existingDrawing = await _context.Drawings.FindAsync(updatedDrawing.Id);
            if (existingDrawing == null || existingDrawing.UserId != userId)
            {
                return NotFound("Drawing not found or unauthorized access.");
            }

            existingDrawing.StareTeren = updatedDrawing.StareTeren;
            existingDrawing.TipCultura = updatedDrawing.TipCultura;
            existingDrawing.Area = updatedDrawing.Area;
            existingDrawing.UltimaCultura = updatedDrawing.UltimaCultura;
            existingDrawing.ProprietarArenda = updatedDrawing.ProprietarArenda;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DrawingExists(updatedDrawing.Id))
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

        [HttpDelete("{id}")] // stergere
        public async Task<IActionResult> DeleteDrawing(int id)
        {
           
            var userIdentity = HttpContext.User.Identity as ClaimsIdentity;

            
            var userIdClaim = userIdentity.FindFirst("userId");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            {
                return Unauthorized("Unauthorized access.");
            }

           
            var drawing = await _context.Drawings.FindAsync(id);
            if (drawing == null || drawing.UserId != userId)
            {
                return NotFound("Drawing not found or unauthorized access.");
            }

           
            _context.Drawings.Remove(drawing);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}