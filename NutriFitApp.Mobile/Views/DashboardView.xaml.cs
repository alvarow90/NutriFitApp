// Archivo: Views/DashboardView.xaml.cs
// Ubicaci�n: Proyecto NutriFitApp.Mobile
using NutriFitApp.Mobile.Services; // Para IAuthService si manejas logout aqu�
using System.Diagnostics;          // Para Debug.WriteLine
using System;                     // Para EventArgs

namespace NutriFitApp.Mobile.Views
{
    public partial class DashboardView : ContentPage
    {
        // Podr�as inyectar IAuthService si necesitas hacer logout directamente desde aqu�
        // y no a trav�s de un ViewModel o un servicio est�tico.
        // private readonly IAuthService _authService;

        // Si fueras a usar inyecci�n de dependencias para IAuthService aqu�:
        // public DashboardView(IAuthService authService) 
        // {
        //     InitializeComponent();
        //     _authService = authService;
        //     Debug.WriteLine("[DashboardView] Constructor ejecutado con IAuthService.");
        // }

        // Constructor actual sin inyecci�n directa de IAuthService en esta vista.
        // El logout se maneja obteniendo el servicio del contenedor de DI.
        public DashboardView()
        {
            InitializeComponent(); // Carga los componentes definidos en DashboardView.xaml

            // Si tuvieras un DashboardViewModel, lo inyectar�as aqu� (o en el constructor)
            // y lo asignar�as al BindingContext:
            // var viewModel = Application.Current.MainPage.Handler.MauiContext.Services.GetService<DashboardViewModel>();
            // BindingContext = viewModel; 

            Debug.WriteLine("[DashboardView] Constructor ejecutado.");
        }

        // Manejador de evento para el bot�n "Ir a Mis Dietas"
        private async void OnGoToDietasClicked(object sender, EventArgs e)
        {
            Debug.WriteLine("[DashboardView] Bot�n 'Ir a Mis Dietas' presionado.");
            // Aseg�rate de que la ruta "dietas" (en min�sculas, seg�n tu AppShell.xaml.cs)
            // est� registrada y que AppShell.xaml tenga un elemento de navegaci�n para ella.
            await Shell.Current.GoToAsync("dietas");
        }

        // Manejador de evento para un bot�n "Ir a Mis Rutinas" (ejemplo)
        private async void OnGoToRutinasClicked(object sender, EventArgs e)
        {
            Debug.WriteLine("[DashboardView] Bot�n 'Ir a Mis Rutinas' presionado.");
            // Aseg�rate de que la ruta "rutinas" (en min�sculas, seg�n tu AppShell.xaml.cs)
            // est� registrada y que AppShell.xaml tenga un elemento de navegaci�n para ella.
            await Shell.Current.GoToAsync("rutinas");
        }

        // Manejador de evento para el bot�n "Cerrar Sesi�n"
        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            Debug.WriteLine("[DashboardView] Bot�n 'Cerrar Sesi�n' presionado.");

            // Obtener IAuthService del contenedor de servicios de .NET MAUI.
            // Esto es necesario si no inyectaste IAuthService directamente en el constructor de esta vista.
            var authService = Application.Current?.MainPage?.Handler?.MauiContext?.Services.GetService<IAuthService>();

            if (authService != null)
            {
                await authService.LogoutAsync(); // Llama al m�todo Logout del servicio.
                Debug.WriteLine("[DashboardView] LogoutAsync llamado desde AuthService.");
            }
            else
            {
                Debug.WriteLine("[DashboardView] Error: No se pudo obtener IAuthService para logout.");
                // Considera mostrar un mensaje de error al usuario si esto falla.
            }

            // Navegar de vuelta a la p�gina de Login.
            // El prefijo "//" asegura una navegaci�n absoluta desde la ra�z de la jerarqu�a de Shell,
            // lo que efectivamente limpia la pila de navegaci�n de las p�ginas anteriores (Dashboard, Dietas, etc.).
            // Aseg�rate de que la ruta "login" est� registrada en AppShell.xaml.cs.
            await Shell.Current.GoToAsync("//login");
        }
    }
}
