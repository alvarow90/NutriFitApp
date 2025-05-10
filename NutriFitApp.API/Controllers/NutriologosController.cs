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
    public class NutriologosController : ControllerBase
    {
        private readonly NutriFitDbContext _context;

        public NutriologosController(NutriFitDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> Get()
        {
            return await _context.Users
                .Where(u => u.Rol == "Nutriologo")
                .ToListAsync();
        }
    }
}
