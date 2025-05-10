using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NutriFitApp.API.Helpers;
using NutriFitApp.Shared.DTOs;
using NutriFitApp.Shared.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NutriFitApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserHelper _userHelper;
        private readonly IConfiguration _config;

        public AuthController(IUserHelper userHelper, IConfiguration config)
        {
            _userHelper = userHelper;
            _config = config;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (await _userHelper.UserExistsAsync(model.Email))
                return BadRequest("El usuario ya existe.");

            var user = new User
            {
                Email = model.Email,
                UserName = model.Email,
                Nombre = model.Nombre,
                Rol = model.Rol
            };

            var result = await _userHelper.AddUserAsync(user, model.Password!);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            await _userHelper.AddUserToRoleAsync(user, model.Rol);

            return Ok();
        }

        [HttpPost("login")]
        public async Task<ActionResult<TokenDTO>> Login(LoginDTO model)
        {
            var user = await _userHelper.GetUserByEmailAsync(model.Email);
            if (user == null || !await _userHelper.CheckPasswordAsync(user, model.Password!))
                return Unauthorized("Credenciales incorrectas");

            var roles = await _userHelper.GetRolesAsync(user);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Email!),
                new Claim(ClaimTypes.Role, roles.FirstOrDefault() ?? ""),
                new Claim("userId", user.Id.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["jwtKey"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiration = DateTime.UtcNow.AddDays(7);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: expiration,
                signingCredentials: creds
            );

            return Ok(new TokenDTO
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration
            });
        }
    }
}
