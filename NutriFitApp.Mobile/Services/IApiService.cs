// IApiService.cs
using System.Net.Http;
using System.Threading.Tasks;

namespace NutriFitApp.Mobile.Services
{
    public interface IApiService
    {
        Task<T?> GetAsync<T>(string uri);
        Task<TResponse?> PostAsync<TRequest, TResponse>(string uri, TRequest data);
        Task<bool> DeleteAsync(string uri);
    }
}
