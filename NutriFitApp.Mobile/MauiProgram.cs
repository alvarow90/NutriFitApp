// Archivo: MauiProgram.cs (en el proyecto NutriFitApp.Mobile)
using Microsoft.Extensions.Logging;
using NutriFitApp.Mobile.Services;
using NutriFitApp.Mobile.ViewModels;
using NutriFitApp.Mobile.Views;
using NutriFitApp.Mobile.Handlers; // Asegúrate que el namespace del AuthTokenHandler es correcto
using System.Net.Http; // Para HttpClient y HttpClientHandler
using System.Diagnostics; // Para Debug.WriteLine
using System; // Para StringComparison

namespace NutriFitApp.Mobile
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // --- Registro de Servicios para Inyección de Dependencias ---

            builder.Services.AddTransient<AuthTokenHandler>();

            builder.Services.AddSingleton<HttpClient>(sp =>
            {
                var authTokenHandler = sp.GetRequiredService<AuthTokenHandler>();

                // --- INICIO DE CONFIGURACIÓN PARA BYPASS SSL EN DESARROLLO ---
#if DEBUG
                // Este HttpClientHandler personalizado solo se usará en modo DEBUG (desarrollo)
                // para omitir la validación del certificado SSL para localhost.
                // ¡NO USAR EN PRODUCCIÓN!
                var httpClientHandler = new HttpClientHandler();
                httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
                {
                    // Confiar en el certificado si el host es localhost.
                    // Esto es para el certificado de desarrollo de ASP.NET Core.
                    if (message.RequestUri.Host.Equals("localhost", StringComparison.OrdinalIgnoreCase))
                    {
                        Debug.WriteLine($"[HttpClientHandler DEBUG] Bypassing SSL validation for localhost: {message.RequestUri}");
                        return true;
                    }
                    Debug.WriteLine($"[HttpClientHandler DEBUG] Standard SSL validation for: {message.RequestUri.Host}, Errors: {errors}");
                    return errors == System.Net.Security.SslPolicyErrors.None;
                };
                authTokenHandler.InnerHandler = httpClientHandler; // Usar el handler personalizado en DEBUG
#else
                // En modo Release (producción), usa el HttpClientHandler por defecto (que valida certificados).
                authTokenHandler.InnerHandler = new HttpClientHandler();
#endif
                // --- FIN DE CONFIGURACIÓN PARA BYPASS SSL EN DESARROLLO ---

                var httpClient = new HttpClient(authTokenHandler);
                // La URL base se define en cada servicio (AuthService, PerfilService, etc.).
                return httpClient;
            });

            // Registrar todos tus servicios, ViewModels y Vistas
            builder.Services.AddSingleton<IAuthService, AuthService>();
            builder.Services.AddSingleton<IDietaService, DietaService>();
            builder.Services.AddSingleton<IRutinaService, RutinaService>();
            builder.Services.AddSingleton<IPerfilService, PerfilService>();

            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<DashboardViewModel>(); // Asumiendo que tienes o tendrás un DashboardViewModel
            builder.Services.AddTransient<DietasViewModel>();
            builder.Services.AddTransient<RutinasViewModel>();
            builder.Services.AddTransient<PerfilViewModel>();

            builder.Services.AddTransient<LoginView>();
            builder.Services.AddTransient<DashboardView>(); // Asumiendo que tienes o tendrás un DashboardView
            builder.Services.AddTransient<DietasView>();
            builder.Services.AddTransient<RutinasView>();
            builder.Services.AddTransient<PerfilView>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
