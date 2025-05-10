// Archivo: Views/DashboardView.xaml.cs
// Ubicación: Proyecto NutriFitApp.Mobile
using NutriFitApp.Mobile.Services; // Para IAuthService si manejas logout aquí
using System.Diagnostics;

namespace NutriFitApp.Mobile.Views
{
    public partial class DashboardView : ContentPage
    {
        // Podrías inyectar IAuthService si necesitas hacer logout desde aquí
        // private readonly IAuthService _authService;

        // public DashboardView(IAuthService authService) // Descomenta si inyectas IAuthService
        public DashboardView() // Constructor actual
        {
            InitializeComponent();
            // _authService = authService; // Descomenta si inyectas IAuthService

            // Si tienes un DashboardViewModel, deberías inyectarlo y asignarlo al BindingContext:
            // BindingContext = viewModel; 
            Debug.WriteLine("[DashboardView] Constructor ejecutado.");
        }

        // Ejemplo de manejador de evento para un botón "Ir a Mis Dietas"
        private async void OnGoToDietasClicked(object sender, EventArgs e)
        {
            Debug.WriteLine("[DashboardView] Botón 'Ir a Mis Dietas' presionado.");
            // Asegúrate de que la ruta "Dietas" esté registrada en tu AppShell.xaml.cs
            // y que AppShell.xaml tenga un ShellContent o FlyoutItem con Route="Dietas"
            await Shell.Current.GoToAsync("dietas");
        }

        // Ejemplo de manejador de evento para un botón "Cerrar Sesión"
        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            Debug.WriteLine("[DashboardView] Botón 'Cerrar Sesión' presionado.");

            // Lógica para cerrar sesión:
            // 1. Limpiar el token guardado (esto debería hacerlo tu AuthService)
            // 2. Navegar de vuelta a la página de Login

            // Opción A: Si tienes IAuthService inyectado
            // if (_authService != null)
            // {
            //     await _authService.LogoutAsync();
            // }

            // Opción B: Si AuthService tiene un método estático o accedes de otra forma
            // (Asumiendo que AuthService.LogoutAsync() es accesible y maneja la limpieza del token)
            var authService = Application.Current.MainPage.Handler.MauiContext.Services.GetService<IAuthService>();
            if (authService != null)
            {
                await authService.LogoutAsync();
            }
            else
            {
                Debug.WriteLine("[DashboardView] Error: No se pudo obtener IAuthService para logout.");
                // Considera mostrar un error al usuario si esto falla.
            }

            // Navegar a la página de Login. El prefijo "//" asegura que se navegue
            // desde la raíz de la jerarquía de Shell, limpiando la pila de navegación.
            await Shell.Current.GoToAsync("//login");
        }
    }
}
