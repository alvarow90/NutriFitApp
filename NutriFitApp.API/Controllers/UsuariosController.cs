using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NutriFitApp.API.Data;
using NutriFitApp.Shared.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NutriFitApp.API.Controllers
{
    [ApiController]
    [Route("api/Usuarios")] // Ruta explícita para evitar problemas de case-sensitive
   // [Authorize(Roles = "Administrador")]
    public class UsuariosController : ControllerBase
    {
        private readonly NutriFitDbContext _context;

        public UsuariosController(NutriFitDbContext context)
        {
            _context = context;
        }

        // ✅ TEST endpoint - verifica que el controlador funciona
        [AllowAnonymous]
        [HttpGet("test")]
        public IActionResult Test() => Ok("Controlador funcionando correctamente");

        // ✅ Lista básica - sin errores de navegación
        [HttpGet("list")]
        public async Task<IActionResult> GetBasicList()
        {
            var users = await _context.Users
                .Select(u => new { u.Id, u.Email, u.Rol })
                .ToListAsync();

            return Ok(users);
        }

        // 🔁 GET completo original (usa con cuidado si hay navegación compleja)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> Get() =>
            await _context.Users.ToListAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> Get(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound();

            return user;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, User user)
        {
            if (id != user.Id) return BadRequest();
            _context.Update(user);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();
            _context.Remove(user);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
