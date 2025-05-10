// Archivo: Controllers/DietasController.cs
// Ubicación: Proyecto NutriFitApp.API

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NutriFitApp.API.Data;        // Para NutriFitDbContext
using NutriFitApp.Shared.Models;   // Para la entidad Dieta
using NutriFitApp.Shared.DTOs;     // Para AsignacionDietaDTO y DietaDTO
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;      // Para User.FindFirstValue
using System.Threading.Tasks;

namespace NutriFitApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    // El [Authorize] a nivel de clase se ha movido a nivel de método para mayor granularidad.
    public class DietasController : ControllerBase
    {
        private readonly NutriFitDbContext _context;

        public DietasController(NutriFitDbContext context)
        {
            _context = context;
        }

        // Endpoint para que un Nutriólogo asigne una dieta a un usuario.
        [HttpPost("asignar")]
        [Authorize(Roles = "Nutriologo")] // Solo los nutriólogos pueden asignar dietas.
        public async Task<IActionResult> AsignarDieta([FromBody] AsignacionDietaDTO dto) // AsignacionDietaDTO ya no debe tener NutriologoId
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Obtener el ID del Nutriólogo autenticado desde los claims del token JWT.
            // Asegúrate de que el claim "userId" (o el que uses para el ID del usuario en el token)
            // se esté añadiendo correctamente cuando generas el token en AuthController.
            var nutriologoIdString = User.FindFirstValue("userId"); // O ClaimTypes.NameIdentifier si es el que usas para el ID.

            if (string.IsNullOrEmpty(nutriologoIdString) || !int.TryParse(nutriologoIdString, out int nutriologoId))
            {
                // Esto no debería pasar si el atributo [Authorize] funciona y el token es válido.
                return Unauthorized("No se pudo identificar al nutriólogo desde el token.");
            }

            // Verificar si el usuario al que se le asigna la dieta existe (opcional pero recomendado)
            var usuarioExiste = await _context.Users.AnyAsync(u => u.Id == dto.UsuarioId);
            if (!usuarioExiste)
            {
                return NotFound($"El usuario con ID {dto.UsuarioId} no fue encontrado.");
            }

            var dieta = new Dieta // Entidad de base de datos
            {
                UsuarioId = dto.UsuarioId,
                NutriologoId = nutriologoId, // Usar el ID del nutriólogo autenticado.
                Descripcion = dto.Descripcion,
                FechaInicio = dto.FechaInicio,
                FechaFin = dto.FechaFin
            };

            _context.Dietas.Add(dieta);
            await _context.SaveChangesAsync();

            // Devolver una respuesta más informativa, como el objeto creado o un mensaje con el ID.
            return Ok(new { Message = "Dieta asignada exitosamente.", DietaId = dieta.Id });
        }

        // Endpoint para que un Nutriólogo vea todas las dietas (o las que él ha creado/gestiona).
        // Podrías añadir más filtros si es necesario.
        [HttpGet] // Ruta: GET /api/Dietas
        [Authorize(Roles = "Nutriologo")] // Solo los Nutriólogos pueden ver esta lista general.
        public async Task<ActionResult<IEnumerable<DietaDTO>>> GetTodasLasDietas()
        {
            var dietas = await _context.Dietas
                                .Select(d => new DietaDTO // Proyectar a DietaDTO
                                {
                                    Id = d.Id,
                                    UsuarioId = d.UsuarioId,
                                    // NutriologoId = d.NutriologoId, // Podrías incluirlo si es relevante para el nutriólogo
                                    Descripcion = d.Descripcion,
                                    FechaInicio = d.FechaInicio,
                                    FechaFin = d.FechaFin
                                    // Considera añadir NombreUsuario y NombreNutriologo si haces Joins
                                })
                                .ToListAsync();
            return Ok(dietas);
        }

        // NUEVO Endpoint para que un usuario (cliente o nutriólogo) vea SUS PROPIAS dietas asignadas.
        [HttpGet("misdietas")] // Ruta: GET /api/Dietas/misdietas
        [Authorize] // Autorizado para cualquier usuario autenticado.
        public async Task<ActionResult<IEnumerable<DietaDTO>>> GetMisDietas()
        {
            var usuarioIdString = User.FindFirstValue("userId"); // O ClaimTypes.NameIdentifier.
            if (string.IsNullOrEmpty(usuarioIdString) || !int.TryParse(usuarioIdString, out int usuarioIdAutenticado))
            {
                return Unauthorized("No se pudo identificar al usuario desde el token.");
            }

            var dietasUsuario = await _context.Dietas
                                        .Where(d => d.UsuarioId == usuarioIdAutenticado) // Filtrar por el ID del usuario autenticado.
                                        .Select(d => new DietaDTO // Proyectar a DietaDTO
                                        {
                                            Id = d.Id,
                                            UsuarioId = d.UsuarioId,
                                            // NutriologoId = d.NutriologoId, // Podrías incluirlo
                                            Descripcion = d.Descripcion,
                                            FechaInicio = d.FechaInicio,
                                            FechaFin = d.FechaFin
                                            // Considera añadir NombreNutriologo si haces un Join con la tabla de Users (nutriólogos)
                                        })
                                        .ToListAsync();

            return Ok(dietasUsuario);
        }
    }
}
