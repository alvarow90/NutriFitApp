// Archivo: Controllers/RutinasController.cs
// Ubicación: Proyecto NutriFitApp.API

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NutriFitApp.API.Data;        // Para NutriFitDbContext
using NutriFitApp.Shared.Models;   // Para la entidad Rutina (asumiendo que existe)
using NutriFitApp.Shared.DTOs;     // Para AsignacionRutinaDTO y RutinaDTO
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;      // Para User.FindFirstValue
using System.Threading.Tasks;
using System;                      // Para DateTime

namespace NutriFitApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RutinasController : ControllerBase
    {
        private readonly NutriFitDbContext _context;

        public RutinasController(NutriFitDbContext context)
        {
            _context = context;
        }

        // Endpoint para que un Entrenador asigne una rutina a un usuario.
        [HttpPost("asignar")]
        [Authorize(Roles = "Entrenador")] // Solo los entrenadores pueden asignar rutinas.
        public async Task<IActionResult> AsignarRutina([FromBody] AsignacionRutinaDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entrenadorIdString = User.FindFirstValue("userId"); // O ClaimTypes.NameIdentifier
            if (string.IsNullOrEmpty(entrenadorIdString) || !int.TryParse(entrenadorIdString, out int entrenadorId))
            {
                return Unauthorized("No se pudo identificar al entrenador desde el token.");
            }

            var usuarioExiste = await _context.Users.AnyAsync(u => u.Id == dto.UsuarioId);
            if (!usuarioExiste)
            {
                return NotFound($"El usuario con ID {dto.UsuarioId} no fue encontrado.");
            }

            var rutina = new Rutina // Asumiendo que tienes una entidad Rutina en NutriFitApp.Shared.Models
            {
                UsuarioId = dto.UsuarioId,
                EntrenadorId = entrenadorId, // ID del entrenador autenticado
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion,
                Ejercicios = dto.Ejercicios, // Si es un string. Si es List<Ejercicio>, necesitarías mapear.
                FechaAsignacion = dto.FechaAsignacion ?? DateTime.UtcNow // Usar fecha actual si no se provee
                // Otros campos de tu entidad Rutina
            };

            _context.Rutinas.Add(rutina);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Rutina asignada exitosamente.", RutinaId = rutina.Id });
        }

        // Endpoint para que un Entrenador vea todas las rutinas (o las que él ha creado/gestiona).
        [HttpGet] // Ruta: GET /api/Rutinas
        [Authorize(Roles = "Entrenador")]
        public async Task<ActionResult<IEnumerable<RutinaDTO>>> GetTodasLasRutinas()
        {
            var rutinas = await _context.Rutinas
                                .Select(r => new RutinaDTO // Proyectar a RutinaDTO
                                {
                                    Id = r.Id,
                                    UsuarioId = r.UsuarioId,
                                    // EntrenadorId = r.EntrenadorId, // Podrías incluirlo
                                    Nombre = r.Nombre,
                                    Descripcion = r.Descripcion,
                                    Ejercicios = r.Ejercicios,
                                    FechaAsignacion = r.FechaAsignacion
                                })
                                .ToListAsync();
            return Ok(rutinas);
        }

        // Endpoint para que un usuario (cliente o entrenador) vea SUS PROPIAS rutinas asignadas.
        [HttpGet("misrutinas")] // Ruta: GET /api/Rutinas/misrutinas
        [Authorize] // Autorizado para cualquier usuario autenticado.
        public async Task<ActionResult<IEnumerable<RutinaDTO>>> GetMisRutinas()
        {
            var usuarioIdString = User.FindFirstValue("userId"); // O ClaimTypes.NameIdentifier
            if (string.IsNullOrEmpty(usuarioIdString) || !int.TryParse(usuarioIdString, out int usuarioIdAutenticado))
            {
                return Unauthorized("No se pudo identificar al usuario desde el token.");
            }

            var rutinasUsuario = await _context.Rutinas
                                        .Where(r => r.UsuarioId == usuarioIdAutenticado) // Filtrar por el ID del usuario autenticado.
                                        .Select(r => new RutinaDTO // Proyectar a RutinaDTO
                                        {
                                            Id = r.Id,
                                            UsuarioId = r.UsuarioId,
                                            Nombre = r.Nombre,
                                            Descripcion = r.Descripcion,
                                            Ejercicios = r.Ejercicios,
                                            FechaAsignacion = r.FechaAsignacion
                                            // Considera añadir NombreEntrenador si haces un Join
                                        })
                                        .ToListAsync();

            return Ok(rutinasUsuario);
        }

        // Podrías añadir un endpoint para GET /api/Rutinas/{id} si es necesario.
    }
}
