using HDigital.Context;
using HDigital.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace HDigital.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ResurseController : ControllerBase
    {
        private readonly AppDbContext _context;

        private bool ResurseExists(int id)
        {
            return _context.Resurse.Any(e => e.Id == id);
        }

        public ResurseController(AppDbContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<Resurse>>> GetResurse()
        {
            var userIdentity = HttpContext.User.Identity as ClaimsIdentity;
            var userIdClaim = userIdentity.FindFirst("userId");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            {
                return BadRequest("Unable to retrieve user ID.");
            }

            var resurse = await _context.Resurse
                                        .Where(d => d.UserId == userId)
                                        .ToListAsync();

            return resurse;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Resurse>> GetResursa(int id)
        {
            var resursa = await _context.Resurse.FindAsync(id);

            if (resursa == null)
            {
                return NotFound();
            }

            return resursa;
        }

        [HttpPost("create")]
        public async Task<ActionResult<Resurse>> PostResurse(Resurse resurse)
        {
            if (User == null)
            {
                return Unauthorized();
            }
            var userIdentity = HttpContext.User.Identity as ClaimsIdentity;
            var userIdClaim = userIdentity.FindFirst("userId");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            {
                return Unauthorized("Unauthorized access.");
            }

            resurse.UserId = userId;

            _context.Resurse.Add(resurse);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetResurse", new { id = resurse.Id }, resurse);
        }

        [Authorize]
        [HttpPut("update")]
        public async Task<IActionResult> PutResurse([FromBody] Resurse updatedResurse)
        {
            var userIdentity = HttpContext.User.Identity as ClaimsIdentity;
            var userIdClaim = userIdentity.FindFirst("userId");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            {
                return Unauthorized("Acces neautorizat");
            }
            var existingResurse = await _context.Resurse.FindAsync(updatedResurse.Id);
            if (existingResurse == null || existingResurse.UserId != userId)
            {
                return NotFound("Resursa nu exista in baza de date/Acces neautorizat");
            }
            existingResurse.Nume = updatedResurse.Nume;
            existingResurse.Tip = updatedResurse.Tip;
            existingResurse.Cantitate = updatedResurse.Cantitate;
            existingResurse.UnitateDeMasura = updatedResurse.UnitateDeMasura;
            existingResurse.DataAchizitie = updatedResurse.DataAchizitie;
            existingResurse.PretAchizitie = updatedResurse.PretAchizitie;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ResurseExists(updatedResurse.Id))
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteResurse(int id)
        {
            var userIdentity = HttpContext.User.Identity as ClaimsIdentity;
            var userIdClaim = userIdentity.FindFirst("userId");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            {
                return Unauthorized("Acces neautorizat.");
            }
            var resursa = await _context.Resurse.FindAsync(id);
            if (resursa == null || resursa.UserId != userId)
            {
                return NotFound("Resursa nu este in baza de date");
            }
            _context.Resurse.Remove(resursa);
            await _context.SaveChangesAsync();

            return NoContent();

        }
    }
}
