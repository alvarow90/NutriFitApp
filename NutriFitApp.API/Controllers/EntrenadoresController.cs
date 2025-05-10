using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NutriFitApp.API.Data;
using NutriFitApp.Shared.Models;

namespace NutriFitApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Administrador")]
    public class EntrenadoresController : ControllerBase
    {
        private readonly NutriFitDbContext _context;

        public EntrenadoresController(NutriFitDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> Get()
        {
            return await _context.Users
                .Where(u => u.Rol == "Entrenador")
                .ToListAsync();
        }
    }
}
