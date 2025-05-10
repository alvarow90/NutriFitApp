using Microsoft.AspNetCore.Identity;
using NutriFitApp.Shared.Models;

namespace NutriFitApp.API.Helpers
{
    public interface IUserHelper
    {
        Task<IdentityResult> AddUserAsync(User user, string password);
        Task AddUserToRoleAsync(User user, string roleName);
        Task<User?> GetUserByEmailAsync(string email);
        Task<bool> CheckPasswordAsync(User user, string password);
        Task<bool> UserExistsAsync(string email);
        Task<IList<string>> GetRolesAsync(User user);
    }
}
