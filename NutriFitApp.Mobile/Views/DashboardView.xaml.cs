// Archivo: Views/DashboardView.xaml.cs
// Ubicación: Proyecto NutriFitApp.Mobile
using NutriFitApp.Mobile.Services; // Para IAuthService si manejas logout aquí
using System.Diagnostics;          // Para Debug.WriteLine
using System;                     // Para EventArgs

namespace NutriFitApp.Mobile.Views
{
    public partial class DashboardView : ContentPage
    {
        // Podrías inyectar IAuthService si necesitas hacer logout directamente desde aquí
        // y no a través de un ViewModel o un servicio estático.
        // private readonly IAuthService _authService;

        // Si fueras a usar inyección de dependencias para IAuthService aquí:
        // public DashboardView(IAuthService authService) 
        // {
        //     InitializeComponent();
        //     _authService = authService;
        //     Debug.WriteLine("[DashboardView] Constructor ejecutado con IAuthService.");
        // }

        // Constructor actual sin inyección directa de IAuthService en esta vista.
        // El logout se maneja obteniendo el servicio del contenedor de DI.
        public DashboardView()
        {
            InitializeComponent(); // Carga los componentes definidos en DashboardView.xaml

            // Si tuvieras un DashboardViewModel, lo inyectarías aquí (o en el constructor)
            // y lo asignarías al BindingContext:
            // var viewModel = Application.Current.MainPage.Handler.MauiContext.Services.GetService<DashboardViewModel>();
            // BindingContext = viewModel; 

            Debug.WriteLine("[DashboardView] Constructor ejecutado.");
        }

        // Manejador de evento para el botón "Ir a Mis Dietas"
        private async void OnGoToDietasClicked(object sender, EventArgs e)
        {
            Debug.WriteLine("[DashboardView] Botón 'Ir a Mis Dietas' presionado.");
            // Asegúrate de que la ruta "dietas" (en minúsculas, según tu AppShell.xaml.cs)
            // esté registrada y que AppShell.xaml tenga un elemento de navegación para ella.
            await Shell.Current.GoToAsync("dietas");
        }

        // Manejador de evento para un botón "Ir a Mis Rutinas" (ejemplo)
        private async void OnGoToRutinasClicked(object sender, EventArgs e)
        {
            Debug.WriteLine("[DashboardView] Botón 'Ir a Mis Rutinas' presionado.");
            // Asegúrate de que la ruta "rutinas" (en minúsculas, según tu AppShell.xaml.cs)
            // esté registrada y que AppShell.xaml tenga un elemento de navegación para ella.
            await Shell.Current.GoToAsync("rutinas");
        }

        // Manejador de evento para el botón "Cerrar Sesión"
        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            Debug.WriteLine("[DashboardView] Botón 'Cerrar Sesión' presionado.");

            // Obtener IAuthService del contenedor de servicios de .NET MAUI.
            // Esto es necesario si no inyectaste IAuthService directamente en el constructor de esta vista.
            var authService = Application.Current?.MainPage?.Handler?.MauiContext?.Services.GetService<IAuthService>();

            if (authService != null)
            {
                await authService.LogoutAsync(); // Llama al método Logout del servicio.
                Debug.WriteLine("[DashboardView] LogoutAsync llamado desde AuthService.");
            }
            else
            {
                Debug.WriteLine("[DashboardView] Error: No se pudo obtener IAuthService para logout.");
                // Considera mostrar un mensaje de error al usuario si esto falla.
            }

            // Navegar de vuelta a la página de Login.
            // El prefijo "//" asegura una navegación absoluta desde la raíz de la jerarquía de Shell,
            // lo que efectivamente limpia la pila de navegación de las páginas anteriores (Dashboard, Dietas, etc.).
            // Asegúrate de que la ruta "login" esté registrada en AppShell.xaml.cs.
            await Shell.Current.GoToAsync("//login");
        }
    }
}
