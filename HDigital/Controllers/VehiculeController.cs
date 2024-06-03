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
        [HttpPut("update")]
        public async Task<IActionResult> PutVehicule([FromBody]Vehicule updatedVehicule)
        {
            var userIdentity = HttpContext.User.Identity as ClaimsIdentity;
            var userIdClaim = userIdentity.FindFirst("userId");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            {
                return Unauthorized("Acces neautorizat");
            }
            var existingVehicule = await _context.Vehicule.FindAsync(updatedVehicule.Id);
            if (existingVehicule == null || existingVehicule.UserId != userId)
            {
                return NotFound("Vehiculul nu exista in baza de date/Acces neautorizat");
            }
            existingVehicule.Categorie = existingVehicule.Categorie;
            existingVehicule.Brand = existingVehicule.Brand;
            existingVehicule.Model = existingVehicule.Model;
            existingVehicule.Putere = existingVehicule.Putere;
            existingVehicule.OreFunctionare = existingVehicule.OreFunctionare;
            existingVehicule.AnFabricatie  = existingVehicule.AnFabricatie;
            existingVehicule.DataAchizitie = existingVehicule.DataAchizitie;
            existingVehicule.PretAchizitie = existingVehicule.PretAchizitie;
            existingVehicule.UltimaMentenanta = existingVehicule.UltimaMentenanta;







            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VehiculeExist(updatedVehicule.Id))
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
            var userIdentity = HttpContext.User.Identity as ClaimsIdentity;
            var userIdClaim = userIdentity.FindFirst("userId");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            {
                return Unauthorized("Acces neautorizat.");
            }
            var vehicule = await _context.Vehicule.FindAsync(id);
            if (vehicule == null || vehicule.UserId != userId)
            {
                return NotFound("Angajatul nu este in data de baze");
            }
            _context.Vehicule.Remove(vehicule);
            await _context.SaveChangesAsync();

            return NoContent();


        }
    }
}
