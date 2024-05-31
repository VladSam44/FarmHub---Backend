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
    public class VehiculeController : ControllerBase
    {
        private readonly AppDbContext _context;
        public VehiculeController(AppDbContext context)
        {
            _context = context;
        }
        private bool VehiculeExist(int id)
        {
            return _context.Vehicule.Any(e => e.Id == id);
        }
        [Authorize]
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<Vehicule>>> GetVehicule()
        {
            var userIdentity = HttpContext.User.Identity as ClaimsIdentity;
            var userIdClaim = userIdentity.FindFirst("userId");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            {
                return BadRequest("Unable to retrieve user Id");
            }
            Console.WriteLine(userId);
            var vehicule = await _context.Vehicule
                .Where(e => e.UserId == userId)
                .ToListAsync();

            return vehicule;
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Vehicule>> GetVehicule(int id)
        {
            var vehicule = await _context.Vehicule.FindAsync(id);

            if (vehicule == null)
            {
                return NotFound();
            }

            return vehicule;
        }
        [HttpPost("create")]
        public async Task<ActionResult<Vehicule>> PostVehicule(Vehicule vehicule)
        {
            var userIdentity = HttpContext.User.Identity as ClaimsIdentity;
            var userIdClaim = userIdentity.FindFirst("userId");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            {
                return Unauthorized();
            }

            vehicule.UserId = userId;

            _context.Vehicule.Add(vehicule);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVehicule", new { id = vehicule.Id }, vehicule);
        }
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVehicule(int id, Vehicule vehicule)
        {
            if (id != vehicule.Id)
            {
                return BadRequest();
            }

            _context.Entry(vehicule).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VehiculeExist(id))
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
        public async Task<IActionResult> DeleteVehicule(int id)
        {
            var vehicule = await _context.Vehicule.FindAsync(id);
            if (vehicule == null)
            {
                return NotFound();
            }

            _context.Vehicule.Remove(vehicule);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
