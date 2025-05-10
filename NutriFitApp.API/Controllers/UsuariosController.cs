// Archivo: Controllers/UsuariosController.cs
// Ubicación: Proyecto NutriFitApp.API

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NutriFitApp.Shared.DTOs;
using NutriFitApp.Shared.Models;
using System.Security.Claims;    // Aunque no lo usemos directamente con Authorize comentado
using System.Threading.Tasks;
using NutriFitApp.API.Data;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using NutriFitApp.API.Helpers;

namespace NutriFitApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly NutriFitDbContext _context;
        private readonly IUserHelper _userHelper;

        public UsuariosController(NutriFitDbContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
            Debug.WriteLine("[API UsuariosController] Constructor ejecutado.");
        }

        [HttpGet("ping")]
        public IActionResult Ping()
        {
            Debug.WriteLine("[API UsuariosController] Endpoint GET /api/Usuarios/ping ALCANZADO.");
            return Ok("Pong desde UsuariosController!");
        }

        // --- ENDPOINTS PARA EL PERFIL DE USUARIO (CON AUTHORIZE COMENTADO TEMPORALMENTE) ---

        [HttpGet("myprofile")] // Usando la ruta que funcionó en pruebas anteriores
        // [Authorize] // TEMPORALMENTE COMENTADO PARA AVANZAR EN LA APP MÓVIL
        public async Task<ActionResult<UsuarioPerfilDTO>> GetMiPerfil()
        {
            Debug.WriteLine("[API UsuariosController] Intentando GET /api/Usuarios/myprofile (Authorize COMENTADO)");

            // IMPORTANTE: Como [Authorize] está comentado, User.FindFirstValue("userId") DEVOLVERÁ NULL.
            // Para que esta prueba funcione y puedas ver datos en la app móvil,
            // necesitas simular un ID de usuario.
            // ¡CAMBIA ESTE ID POR EL DE UN USUARIO REAL EN TU BASE DE DATOS PARA PROBAR!
            int userIdParaPrueba = 1; // <--- ¡CAMBIA ESTO AL ID DE UN USUARIO EXISTENTE!

            // var userIdString = User.FindFirstValue("userId"); 
            // if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
            // {
            //     // Esta lógica no se ejecutará como se espera sin [Authorize]
            // }
            // else 
            // {
            //    userIdParaPrueba = userId; 
            // }
            Debug.WriteLine($"[API UsuariosController] GetMiPerfil - Usando UserID de prueba: {userIdParaPrueba}");

            var usuario = await _context.Users.FindAsync(userIdParaPrueba);
            if (usuario == null)
            {
                Debug.WriteLine($"[API UsuariosController] GetMiPerfil - NotFound: Usuario de prueba con ID {userIdParaPrueba} no encontrado.");
                return NotFound($"Usuario de prueba con ID {userIdParaPrueba} no encontrado. Asegúrate de que este ID exista en tu base de datos.");
            }

            var perfilDto = new UsuarioPerfilDTO
            {
                Id = usuario.Id,
                Email = usuario.Email ?? string.Empty,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Rol = usuario.Rol,
                FechaNacimiento = usuario.FechaNacimiento,
                AlturaCm = usuario.AlturaCm,
                PesoKg = usuario.PesoKg,
                Objetivos = usuario.Objetivos
            };
            Debug.WriteLine($"[API UsuariosController] GetMiPerfil - Perfil de prueba encontrado y devuelto para Usuario ID: {userIdParaPrueba}.");
            return Ok(perfilDto);
        }

        [HttpPut("myprofile")]
        // [Authorize] // TEMPORALMENTE COMENTADO PARA AVANZAR EN LA APP MÓVIL
        public async Task<IActionResult> ActualizarMiPerfil([FromBody] ActualizarUsuarioPerfilDTO actualizarDto)
        {
            Debug.WriteLine("[API UsuariosController] Intentando PUT /api/Usuarios/myprofile (Authorize COMENTADO)");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // IMPORTANTE: Usar un ID de prueba para la actualización.
            // ¡CAMBIA ESTE ID POR EL DE UN USUARIO REAL EN TU BASE DE DATOS PARA PROBAR!
            int userIdParaPrueba = 1; // <--- ¡CAMBIA ESTO AL ID DEL USUARIO QUE QUIERES ACTUALIZAR!
            // var userIdString = User.FindFirstValue("userId"); 
            // ... (lógica de obtener userId del token comentada) ...
            Debug.WriteLine($"[API UsuariosController] ActualizarMiPerfil - Usando UserID de prueba: {userIdParaPrueba}");

            var usuario = await _context.Users.FindAsync(userIdParaPrueba);
            if (usuario == null)
            {
                Debug.WriteLine($"[API UsuariosController] ActualizarMiPerfil - NotFound: Usuario de prueba con ID {userIdParaPrueba} no encontrado.");
                return NotFound($"Usuario de prueba con ID {userIdParaPrueba} no encontrado.");
            }

            usuario.Nombre = actualizarDto.Nombre;
            usuario.Apellido = actualizarDto.Apellido;
            usuario.FechaNacimiento = actualizarDto.FechaNacimiento;
            usuario.AlturaCm = actualizarDto.AlturaCm;
            usuario.PesoKg = actualizarDto.PesoKg;
            usuario.Objetivos = actualizarDto.Objetivos;
            try
            {
                _context.Users.Update(usuario);
                await _context.SaveChangesAsync();
                Debug.WriteLine($"[API UsuariosController] ActualizarMiPerfil - Perfil de prueba actualizado para Usuario ID: {userIdParaPrueba}.");
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Debug.WriteLine($"[API UsuariosController] ActualizarMiPerfil - Error de concurrencia: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al actualizar el perfil del usuario debido a un conflicto de concurrencia.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[API UsuariosController] ActualizarMiPerfil - Error al guardar: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error al guardar los cambios en el perfil.");
            }
            return NoContent();
        }
    }
}
