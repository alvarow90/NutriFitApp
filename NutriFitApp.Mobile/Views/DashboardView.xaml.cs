// Archivo: Views/DashboardView.xaml.cs
// Ubicaci�n: Proyecto NutriFitApp.Mobile
using NutriFitApp.Mobile.Services; // Para IAuthService si manejas logout aqu�
using System.Diagnostics;

namespace NutriFitApp.Mobile.Views
{
    public partial class DashboardView : ContentPage
    {
        // Podr�as inyectar IAuthService si necesitas hacer logout desde aqu�
        // private readonly IAuthService _authService;

        // public DashboardView(IAuthService authService) // Descomenta si inyectas IAuthService
        public DashboardView() // Constructor actual
        {
            InitializeComponent();
            // _authService = authService; // Descomenta si inyectas IAuthService

            // Si tienes un DashboardViewModel, deber�as inyectarlo y asignarlo al BindingContext:
            // BindingContext = viewModel; 
            Debug.WriteLine("[DashboardView] Constructor ejecutado.");
        }

        // Ejemplo de manejador de evento para un bot�n "Ir a Mis Dietas"
        private async void OnGoToDietasClicked(object sender, EventArgs e)
        {
            Debug.WriteLine("[DashboardView] Bot�n 'Ir a Mis Dietas' presionado.");
            // Aseg�rate de que la ruta "Dietas" est� registrada en tu AppShell.xaml.cs
            // y que AppShell.xaml tenga un ShellContent o FlyoutItem con Route="Dietas"
            await Shell.Current.GoToAsync("dietas");
        }

        // Ejemplo de manejador de evento para un bot�n "Cerrar Sesi�n"
        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            Debug.WriteLine("[DashboardView] Bot�n 'Cerrar Sesi�n' presionado.");

            // L�gica para cerrar sesi�n:
            // 1. Limpiar el token guardado (esto deber�a hacerlo tu AuthService)
            // 2. Navegar de vuelta a la p�gina de Login

            // Opci�n A: Si tienes IAuthService inyectado
            // if (_authService != null)
            // {
            //     await _authService.LogoutAsync();
            // }

            // Opci�n B: Si AuthService tiene un m�todo est�tico o accedes de otra forma
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

            // Navegar a la p�gina de Login. El prefijo "//" asegura que se navegue
            // desde la ra�z de la jerarqu�a de Shell, limpiando la pila de navegaci�n.
            await Shell.Current.GoToAsync("//login");
        }
    }
}
