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
    public class TransportController : ControllerBase
    {
        public readonly AppDbContext _context;
        public TransportController(AppDbContext context)
        {
            _context = context;
        }
        private bool TransportExists(int id)
        {
            return _context.Transport.Any(e => e.Id == id);
        }
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Transport>>> GetTransport()
        {
            var userIdentity = HttpContext.User.Identity as ClaimsIdentity;
            var userIdClaim = userIdentity.FindFirst("userId");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            {
                return BadRequest("Unable to retrieve user Id");
            }
            Console.WriteLine(userId);
            var transport = await _context.Transport
                .Where(e => e.UserId == userId)
                .ToListAsync();

            return transport;
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Transport>> GetTransport(int id)
        {
            var transport = await _context.Transport.FindAsync(id);

            if (transport == null)
            {
                return NotFound();
            }

            return transport;
        }
        [Authorize]
        [HttpPost("create")]
        public async Task<ActionResult<Transport>> PostTransport(Transport transport)
        {
            var userIdentity = HttpContext.User.Identity as ClaimsIdentity;
            var userIdClaim = userIdentity.FindFirst("userId");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            {
                return Unauthorized();
            }

            transport.UserId = userId;

            _context.Transport.Add(transport);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTransport", new { id = transport.Id }, transport);
        }
        [Authorize]
        [HttpPut("update")]
        public async Task<IActionResult> PutTransport([FromBody]Transport updatedTransport)
        {
            var userIdentity = HttpContext.User.Identity as ClaimsIdentity;
            var userIdClaim = userIdentity.FindFirst("userId");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            {
                return Unauthorized("Acces neautorizat");
            }
            var existingTransport = await _context.Transport.FindAsync(updatedTransport.Id);
            if (existingTransport == null || existingTransport.UserId != userId)
            {
                return NotFound("Transportul nu exista in baza de date/Acces neautorizat");
            }
            existingTransport.Categorie = updatedTransport.Categorie;
            existingTransport.Brand = updatedTransport.Brand;
            existingTransport.Capacitate = updatedTransport.Capacitate;
            existingTransport.UltimaMentenanta = updatedTransport.UltimaMentenanta;
            existingTransport.TipAtasament = updatedTransport.TipAtasament;
            existingTransport.AnFabricatie = updatedTransport.AnFabricatie;
            existingTransport.DataAchizitie = updatedTransport.DataAchizitie;
            existingTransport.PretAchizitie = updatedTransport.PretAchizitie;





            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransportExists(updatedTransport.Id))
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
        public async Task<IActionResult> DeleteTransport(int id)
        {
            var userIdentity = HttpContext.User.Identity as ClaimsIdentity;
            var userIdClaim = userIdentity.FindFirst("userId");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            {
                return Unauthorized("Acces neautorizat.");
            }
            var transport = await _context.Transport.FindAsync(id);
            if (transport == null || transport.UserId != userId)
            {
                return NotFound("Angajatul nu este in data de baze");
            }
            _context.Transport.Remove(transport);
            await _context.SaveChangesAsync();

            return NoContent();

        }
    }
}
