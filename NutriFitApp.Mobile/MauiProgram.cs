// Archivo: MauiProgram.cs (en el proyecto NutriFitApp.Mobile)

// Usings necesarios para la configuración de la aplicación MAUI.
using Microsoft.Extensions.Logging; // Para logging si es necesario.
using NutriFitApp.Mobile.Services;  // Namespace de tus servicios (IAuthService, AuthService, IDietaService, DietaService).
using NutriFitApp.Mobile.ViewModels; // Namespace de tus ViewModels (LoginViewModel, DietasViewModel).
using NutriFitApp.Mobile.Views;      // Namespace de tus Vistas (LoginView, DietasView).
using NutriFitApp.Mobile.Handlers;   // Namespace donde reside tu AuthTokenHandler.
                                     // ¡Asegúrate de que este namespace sea correcto según donde guardaste AuthTokenHandler.cs!

namespace NutriFitApp.Mobile
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder(); // Crea el constructor de la aplicación MAUI.
            builder
                .UseMauiApp<App>() // Especifica la clase principal de la aplicación (App.xaml.cs).
                .ConfigureFonts(fonts => // Configura las fuentes personalizadas para la aplicación.
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    // Puedes añadir más fuentes aquí si las necesitas.
                });

            // --- Registro de Servicios para Inyección de Dependencias ---

            // 1. Registrar el AuthTokenHandler como un servicio transitorio.
            builder.Services.AddTransient<AuthTokenHandler>();

            // 2. HttpClient:
            //    Configuramos el HttpClient principal de la aplicación como Singleton.
            //    Esta instancia de HttpClient utilizará nuestro AuthTokenHandler.
            builder.Services.AddSingleton<HttpClient>(sp =>
            {
                var authTokenHandler = sp.GetRequiredService<AuthTokenHandler>();
                authTokenHandler.InnerHandler = new HttpClientHandler();
                var httpClient = new HttpClient(authTokenHandler);
                // La URL base de tu API se define en cada servicio (AuthService, DietaService).
                return httpClient;
            });

            // 3. Servicios de la Aplicación:
            builder.Services.AddSingleton<IAuthService, AuthService>();

            // *** AÑADIDO/CORREGIDO AQUÍ: Registrar IDietaService y DietaService ***
            builder.Services.AddSingleton<IDietaService, DietaService>();
            // Si tienes más servicios, regístralos aquí.

            // 4. ViewModels:
            builder.Services.AddTransient<LoginViewModel>();

            // *** AÑADIDO/CORREGIDO AQUÍ: Registrar DietasViewModel ***
            builder.Services.AddTransient<DietasViewModel>();
            // Registra otros ViewModels aquí (ej. DashboardViewModel).

            // 5. Vistas (Páginas):
            builder.Services.AddTransient<LoginView>();

            // *** AÑADIDO/CORREGIDO AQUÍ: Registrar DietasView ***
            builder.Services.AddTransient<DietasView>();
            // Registra otras Vistas aquí (ej. DashboardView).


            // Configuración de Logging (opcional, pero útil para depuración).
#if DEBUG
            builder.Logging.AddDebug(); // Habilita el logging a la ventana de depuración en modo DEBUG.
#endif

            // Construye y devuelve la aplicación MAUI configurada.
            return builder.Build();
        }
    }
}
