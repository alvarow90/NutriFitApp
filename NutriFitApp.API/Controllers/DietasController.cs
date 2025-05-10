// Archivo: Controllers/DietasController.cs
// Ubicación: Proyecto NutriFitApp.API

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NutriFitApp.API.Data;        // Para NutriFitDbContext
using NutriFitApp.Shared.Models;   // Para la entidad Dieta
using NutriFitApp.Shared.DTOs;     // *** AÑADIDO/VERIFICADO: Para AsignacionDietaDTO y DietaDTO ***
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;      // Para User.FindFirstValue
using System.Threading.Tasks;

namespace NutriFitApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DietasController : ControllerBase
    {
        private readonly NutriFitDbContext _context;

        public DietasController(NutriFitDbContext context)
        {
            _context = context;
        }

        // Endpoint para que un Nutriólogo asigne una dieta a un usuario.
        [HttpPost("asignar")]
        [Authorize(Roles = "Nutriologo")]
        public async Task<IActionResult> AsignarDieta([FromBody] AsignacionDietaDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var nutriologoIdString = User.FindFirstValue("userId");

            if (string.IsNullOrEmpty(nutriologoIdString) || !int.TryParse(nutriologoIdString, out int nutriologoId))
            {
                return Unauthorized("No se pudo identificar al nutriólogo desde el token.");
            }

            var usuarioExiste = await _context.Users.AnyAsync(u => u.Id == dto.UsuarioId);
            if (!usuarioExiste)
            {
                return NotFound($"El usuario con ID {dto.UsuarioId} no fue encontrado.");
            }

            var dieta = new Dieta
            {
                UsuarioId = dto.UsuarioId,
                NutriologoId = nutriologoId,
                Descripcion = dto.Descripcion,
                FechaInicio = dto.FechaInicio,
                FechaFin = dto.FechaFin
            };

            _context.Dietas.Add(dieta);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Dieta asignada exitosamente.", DietaId = dieta.Id });
        }

        [HttpGet]
        [Authorize(Roles = "Nutriologo")]
        public async Task<ActionResult<IEnumerable<DietaDTO>>> GetTodasLasDietas()
        {
            var dietas = await _context.Dietas
                                .Select(d => new DietaDTO
                                {
                                    Id = d.Id,
                                    UsuarioId = d.UsuarioId,
                                    Descripcion = d.Descripcion,
                                    FechaInicio = d.FechaInicio,
                                    FechaFin = d.FechaFin
                                })
                                .ToListAsync();
            return Ok(dietas);
        }

        [HttpGet("misdietas")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<DietaDTO>>> GetMisDietas()
        {
            var usuarioIdString = User.FindFirstValue("userId");
            if (string.IsNullOrEmpty(usuarioIdString) || !int.TryParse(usuarioIdString, out int usuarioIdAutenticado))
            {
                return Unauthorized("No se pudo identificar al usuario desde el token.");
            }

            var dietasUsuario = await _context.Dietas
                                        .Where(d => d.UsuarioId == usuarioIdAutenticado)
                                        .Select(d => new DietaDTO
                                        {
                                            Id = d.Id,
                                            UsuarioId = d.UsuarioId,
                                            Descripcion = d.Descripcion,
                                            FechaInicio = d.FechaInicio,
                                            FechaFin = d.FechaFin
                                        })
                                        .ToListAsync();

            return Ok(dietasUsuario);
        }
    }
}
