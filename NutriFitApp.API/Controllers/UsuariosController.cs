// Archivo: Controllers/UsuariosController.cs
// Ubicación: Proyecto NutriFitApp.API

// Usings necesarios
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;    // Si usas UserManager<User> directamente
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;     // Para DbUpdateConcurrencyException y ToListAsync, etc.
using NutriFitApp.API.Data;          // Para NutriFitDbContext
using NutriFitApp.API.Helpers;       // Para IUserHelper (si lo sigues usando para algunas operaciones)
using NutriFitApp.Shared.DTOs;       // Para UsuarioPerfilDTO y ActualizarUsuarioPerfilDTO
using NutriFitApp.Shared.Models;     // Para la entidad User
using System.Security.Claims;        // Para User.FindFirstValue
using System.Threading.Tasks;
using System.Linq;                   // Para proyecciones Select
using Microsoft.AspNetCore.Http;     // Para StatusCodes

namespace NutriFitApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly NutriFitDbContext _context;
        private readonly IUserHelper _userHelper; // Asumiendo que todavía usas IUserHelper para algunas cosas
                                                  // Si no, podrías inyectar UserManager<User> directamente
                                                  // para operaciones de Identity más complejas si fuera necesario.

        // Constructor del controlador
        public UsuariosController(NutriFitDbContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }

        // --- ENDPOINTS PARA EL PERFIL DE USUARIO ---

        // GET: api/Usuarios/perfil
        // Obtiene el perfil del usuario actualmente autenticado.
        [HttpGet("perfil")]
        [Authorize] // Solo para usuarios autenticados
        public async Task<ActionResult<UsuarioPerfilDTO>> GetMiPerfil()
        {
            // Obtener el ID del usuario desde los claims del token JWT.
            var userIdString = User.FindFirstValue("userId"); // O ClaimTypes.NameIdentifier, según cómo generes el token.
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
            {
                return Unauthorized("No se pudo identificar al usuario desde el token.");
            }

            // Buscar el usuario en la base de datos.
            var usuario = await _context.Users.FindAsync(userId);

            if (usuario == null)
            {
                return NotFound("Usuario no encontrado.");
            }

            // Mapear la entidad User a UsuarioPerfilDTO.
            // Este DTO se envía al cliente.
            var perfilDto = new UsuarioPerfilDTO
            {
                Id = usuario.Id,
                Email = usuario.Email ?? string.Empty,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Rol = usuario.Rol, // Asumiendo que la propiedad Rol en la entidad User contiene el rol principal.
                                   // Si los roles se manejan exclusivamente a través de IdentityRoles,
                                   // necesitarías obtenerlos usando _userHelper.GetRolesAsync(usuario) o UserManager.

                // Mapear los campos adicionales del perfil
                FechaNacimiento = usuario.FechaNacimiento,
                AlturaCm = usuario.AlturaCm,
                PesoKg = usuario.PesoKg,
                Objetivos = usuario.Objetivos
            };

            return Ok(perfilDto);
        }

        // PUT: api/Usuarios/perfil
        // Actualiza el perfil del usuario actualmente autenticado.
        [HttpPut("perfil")]
        [Authorize] // Solo para usuarios autenticados
        public async Task<IActionResult> ActualizarMiPerfil([FromBody] ActualizarUsuarioPerfilDTO actualizarDto)
        {
            if (!ModelState.IsValid) // Validar el DTO de entrada.
            {
                return BadRequest(ModelState);
            }

            // Obtener el ID del usuario desde los claims del token.
            var userIdString = User.FindFirstValue("userId"); // O ClaimTypes.NameIdentifier.
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
            {
                return Unauthorized("No se pudo identificar al usuario desde el token.");
            }

            // Buscar el usuario en la base de datos.
            var usuario = await _context.Users.FindAsync(userId);

            if (usuario == null)
            {
                return NotFound("Usuario no encontrado.");
            }

            // Actualizar las propiedades del usuario con los valores del DTO.
            // Solo se actualizan los campos que están en ActualizarUsuarioPerfilDTO.
            usuario.Nombre = actualizarDto.Nombre;
            usuario.Apellido = actualizarDto.Apellido;

            // Actualizar campos adicionales del perfil
            usuario.FechaNacimiento = actualizarDto.FechaNacimiento; // Si es nullable, se asigna directamente.
            usuario.AlturaCm = actualizarDto.AlturaCm;
            usuario.PesoKg = actualizarDto.PesoKg;
            usuario.Objetivos = actualizarDto.Objetivos;

            try
            {
                _context.Users.Update(usuario); // Marcar la entidad como modificada.
                await _context.SaveChangesAsync(); // Guardar los cambios en la base de datos.
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // Manejar posibles excepciones de concurrencia si otro proceso modificó el usuario
                // al mismo tiempo. Podrías necesitar una estrategia más robusta aquí.
                // Loggear el error es importante.
                // El logging no está implementado aquí, pero deberías añadirlo.
                // Serilog, NLog, o el logging integrado de ASP.NET Core son opciones.
                System.Diagnostics.Debug.WriteLine($"Error de concurrencia al actualizar perfil: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al actualizar el perfil del usuario debido a un conflicto de concurrencia.");
            }
            catch (Exception ex)
            {
                // Capturar otras posibles excepciones durante el guardado.
                System.Diagnostics.Debug.WriteLine($"Error al guardar perfil: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error al guardar los cambios en el perfil.");
            }

            return NoContent(); // HTTP 204 No Content indica que la operación fue exitosa y no hay contenido que devolver.
                                // Alternativamente, podrías devolver Ok(new { Message = "Perfil actualizado exitosamente." });
        }

        // --- Mantén aquí tus otros endpoints existentes de UsuariosController ---
        // Por ejemplo, los que vi en tu Swagger:
        // GET /api/Usuarios/test
        // GET /api/Usuarios/list
        // GET /api/Usuarios
        // GET /api/Usuarios/{id}
        // PUT /api/Usuarios/{id} (Este podría necesitar revisión si es para administradores vs. el usuario actualizando su propio perfil)
        // DELETE /api/Usuarios/{id}
    }
}
