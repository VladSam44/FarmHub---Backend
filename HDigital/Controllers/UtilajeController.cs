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
    public class UtilajeController : ControllerBase
    {
        private readonly AppDbContext _context;
        public UtilajeController(AppDbContext context)
        {
            _context = context;
        }
        private bool UtilajeExist(int id)
        {
            return _context.Utilaje.Any(e => e.Id == id);
        }
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Utilaje>>> GetUtilaje()
        {
            var userIdentity = HttpContext.User.Identity as ClaimsIdentity;
            var userIdClaim = userIdentity.FindFirst("userId");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            {
                return BadRequest("Unable to retrieve user Id");
            }
            Console.WriteLine(userId);
            var utilaje = await _context.Utilaje
                .Where(e => e.UserId == userId)
                .ToListAsync();

            return utilaje;
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Utilaje>> GetUtilaje(int id)
        {
            var utilaje = await _context.Utilaje.FindAsync(id);

            if (utilaje == null)
            {
                return NotFound();
            }

            return utilaje;
        }
        [HttpPost]
        public async Task<ActionResult<Utilaje>> PostUtilaje(Utilaje utilaje)
        {
            var userIdentity = HttpContext.User.Identity as ClaimsIdentity;
            var userIdClaim = userIdentity.FindFirst("userId");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            {
                return Unauthorized();
            }

            utilaje.UserId = userId;

            _context.Utilaje.Add(utilaje);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUtilaje", new { id = utilaje.Id }, utilaje);
        }
        [Authorize]
        [HttpPut("update")]
        public async Task<IActionResult> PutUtilaje([FromBody]Utilaje updatedUtilaje)
        {
            var userIdentity = HttpContext.User.Identity as ClaimsIdentity;
            var userIdClaim = userIdentity.FindFirst("userId");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            {
                return Unauthorized("Acces neautorizat");
            }
            var existingUtilaje = await _context.Utilaje.FindAsync(updatedUtilaje.Id);
            if (existingUtilaje == null || existingUtilaje.UserId != userId)
            {
                return NotFound("Utilajul nu exista in baza de date/Acces neautorizat");
            }
            existingUtilaje.Categorie = updatedUtilaje.Categorie;
            existingUtilaje.Brand = updatedUtilaje.Brand;
            existingUtilaje.Model = updatedUtilaje.Model;
            existingUtilaje.PutereNecesara = updatedUtilaje.PutereNecesara;
            existingUtilaje.Greutate = updatedUtilaje.Greutate;
            existingUtilaje.AnFabricatie = updatedUtilaje.AnFabricatie;
            existingUtilaje.OreFunctionare = updatedUtilaje.OreFunctionare;
            existingUtilaje.DataAchizitie = updatedUtilaje.DataAchizitie;
            existingUtilaje.PretAchizitie = updatedUtilaje.PretAchizitie;
            existingUtilaje.UltimaMentenanta = updatedUtilaje.UltimaMentenanta;


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UtilajeExist(updatedUtilaje.Id))
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
        public async Task<IActionResult> DeleteUtilaje(int id)
        {
            var userIdentity = HttpContext.User.Identity as ClaimsIdentity;
            var userIdClaim = userIdentity.FindFirst("userId");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            {
                return Unauthorized("Acces neautorizat.");
            }
            var utilaje = await _context.Utilaje.FindAsync(id);
            if (utilaje == null || utilaje.UserId != userId)
            {
                return NotFound("Angajatul nu este in data de baze");
            }
            _context.Utilaje.Remove(utilaje);
            await _context.SaveChangesAsync();

            return NoContent();

        }
    }
}
