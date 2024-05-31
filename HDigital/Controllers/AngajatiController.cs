using HDigital.Context;
using HDigital.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HDigital.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AngajatiController : ControllerBase
    {
        private readonly AppDbContext _context;

        private bool AngajatiExists(int id)
        {
            return _context.Angajati.Any(e => e.Id == id);
        }

        public AngajatiController(AppDbContext context)
        {
            _context = context;
        }

       

        [Authorize]
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<Angajati>>> GetAngajati()
        {
            var userIdentity = HttpContext.User.Identity as ClaimsIdentity;
            var userIdClaim = userIdentity.FindFirst("userId");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            {
                return BadRequest("Unable to retrieve user ID.");
            }

            var angajati = await _context.Angajati
                                        .Where(d => d.UserId == userId)
                                        .ToListAsync();

            return angajati;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Angajati>> GetAngajat(int id)
        {
            var angajat = await _context.Angajati.FindAsync(id);

            if (angajat == null)
            {
                return NotFound();
            }

            return angajat;
        }

        [HttpPost("create")]
        public async Task<ActionResult<Angajati>> PostAngajati(Angajati angajati)
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

            angajati.UserId = userId;

            _context.Angajati.Add(angajati);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAngajati", new { id = angajati.Id }, angajati);
        }

        [Authorize]
        [HttpPut("update")]
        public async Task<IActionResult> PutAngajati([FromBody]Angajati updatedAngajati)
        {
            var userIdentity = HttpContext.User.Identity as ClaimsIdentity;
            var userIdClaim = userIdentity.FindFirst("userId");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            {
                return Unauthorized("Acces neautorizat");
            }
            var existingAngajati = await _context.Angajati.FindAsync(updatedAngajati.Id);
            if (existingAngajati == null || existingAngajati.UserId != userId)
            {
                return NotFound("Angajatul nu exista in baza de date/Acces neautorizat");
            }
            existingAngajati.Nume = updatedAngajati.Nume;
            existingAngajati.Pozitie = updatedAngajati.Pozitie;
            existingAngajati.Salariu = updatedAngajati.Salariu;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AngajatiExists(updatedAngajati.Id))
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
        public async Task<IActionResult> DeleteAngajati(int id)
        {
            var userIdentity = HttpContext.User.Identity as ClaimsIdentity;
            var userIdClaim = userIdentity.FindFirst("userId");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId)) 
            {
                return Unauthorized("Acces neautorizat.");
            }
           var angajati = await _context.Angajati.FindAsync(id);
           if(angajati == null || angajati.UserId != userId)
            {
                return NotFound("Angajatul nu este in data de baze");
            }
           _context.Angajati.Remove(angajati);
            await _context.SaveChangesAsync();

            return NoContent(); 
            
        }
    }
}
