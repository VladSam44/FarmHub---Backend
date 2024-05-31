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
        [HttpPost]
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
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTransport(int id, Transport transport)
        {
            if (id != transport.Id)
            {
                return BadRequest();
            }

            _context.Entry(transport).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransportExists(id))
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
            var transport = await _context.Transport.FindAsync(id);
            if (transport == null)
            {
                return NotFound();
            }

            _context.Transport.Remove(transport);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
