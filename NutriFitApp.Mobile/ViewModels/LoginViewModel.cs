// Archivo: ViewModels/LoginViewModel.cs
// Usings necesarios para el ViewModel
using CommunityToolkit.Mvvm.ComponentModel; // Para ObservableObject y ObservableProperty
using CommunityToolkit.Mvvm.Input;         // Para RelayCommand
using NutriFitApp.Mobile.Services;         // Para IAuthService y MobileLoginResult (tu clase de resultado)
using NutriFitApp.Shared.DTOs;           // Para LoginDTO (el objeto que envías a la API)
using System.Threading.Tasks;              // Para Task (operaciones asíncronas)
using System.Diagnostics;                  // Para Debug.WriteLine (mensajes de depuración)
using Microsoft.Maui.ApplicationModel;     // Para MainThread (asegurar ejecución en hilo UI)

namespace NutriFitApp.Mobile.ViewModels
{
    // LoginViewModel hereda de ObservableObject para notificar cambios en propiedades a la UI
    public partial class LoginViewModel : ObservableObject
    {
        // Servicio para manejar la lógica de autenticación. Se inyecta en el constructor.
        private readonly IAuthService _authService;

        // Propiedades Observables: La UI (XAML) se enlaza a estas propiedades.
        // Cuando su valor cambia, la UI se actualiza automáticamente.

        [ObservableProperty]
        private string email; // Almacena el correo electrónico ingresado por el usuario

        [ObservableProperty]
        private string password; // Almacena la contraseña ingresada por el usuario

        [ObservableProperty]
        private string errorMessage; // Muestra mensajes de error en la UI

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotBusy))] // Cuando IsBusy cambie, también notifica que IsNotBusy cambió
        private bool isBusy; // Indica si una operación (como el login) está en curso. Útil para mostrar un indicador de carga o deshabilitar botones.

        // Propiedad computada que es la inversa de IsBusy.
        // Útil para enlazar la propiedad IsEnabled de un botón (IsEnabled="{Binding IsNotBusy}")
        public bool IsNotBusy => !IsBusy;

        // Constructor del ViewModel.
        // Recibe IAuthService mediante inyección de dependencias (configurado en MauiProgram.cs).
        public LoginViewModel(IAuthService authService)
        {
            _authService = authService;
            Debug.WriteLine("[LoginViewModel] Constructor ejecutado. IAuthService inyectado.");
        }

        // Comando que se enlaza al botón "Iniciar Sesión" en la UI.
        // Se ejecuta de forma asíncrona cuando el usuario presiona el botón.
        [RelayCommand]
        public async Task LoginAsync()
        {
            // Si ya hay una operación de login en curso, no hacer nada para evitar múltiples envíos.
            if (IsBusy)
                return;

            IsBusy = true; // Indicar que la operación de login ha comenzado.
            ErrorMessage = string.Empty; // Limpiar cualquier mensaje de error previo de la UI.
            Debug.WriteLine($"[LoginViewModel] LoginAsync llamado. Email: {Email}");

            // Validación básica de los campos de entrada.
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Por favor, ingresa correo y contraseña.";
                IsBusy = false; // La operación terminó (con error).
                return;
            }

            // Crear el objeto LoginDTO que se enviará al servicio (y luego a la API).
            var loginRequest = new LoginDTO
            {
                Email = this.Email, // Usar el valor de la propiedad Email
                Password = this.Password // Usar el valor de la propiedad Password
            };

            try
            {
                // Llamar al método LoginAsync del servicio de autenticación.
                // Este método es el que realmente hace la llamada HTTP a tu API.
                MobileLoginResult result = await _authService.LoginAsync(loginRequest);

                // Verificar si el resultado del servicio es nulo (esto no debería pasar si el servicio está bien).
                if (result == null)
                {
                    ErrorMessage = "No se pudo obtener respuesta del servicio de autenticación.";
                    Debug.WriteLine("[LoginViewModel] Error: _authService.LoginAsync devolvió null.");
                    IsBusy = false;
                    return;
                }

                // Si el login no fue exitoso (según la propiedad IsSuccess del resultado).
                if (!result.IsSuccess)
                {
                    ErrorMessage = result.ErrorMessage; // Mostrar el mensaje de error devuelto por el servicio/API.
                    Debug.WriteLine($"[LoginViewModel] Login fallido: {result.ErrorMessage}");
                    IsBusy = false;
                    return;
                }

                // Si el login fue exitoso:
                // El token JWT ya debería haber sido guardado por el AuthService.
                Debug.WriteLine($"[LoginViewModel] Login exitoso. Token (si aplica): {result.TokenData?.Token}. Navegando a //dashboard...");

                // Navegar a la página de dashboard.
                // Se usa MainThread.InvokeOnMainThreadAsync para asegurar que la navegación
                // (que es una operación de UI) se ejecute en el hilo principal de la UI.
                await MainThread.InvokeOnMainThreadAsync(async () =>
                {
                    await Shell.Current.GoToAsync("//dashboard"); // Navegación usando Shell de .NET MAUI
                });
            }
            catch (Exception ex) // Capturar cualquier otra excepción inesperada durante el proceso.
            {
                Debug.WriteLine($"[LoginViewModel] Excepción no controlada en LoginAsync: {ex.ToString()}");
                ErrorMessage = "Ocurrió un error inesperado durante el inicio de sesión.";
            }
            finally
            {
                // Este bloque se ejecuta siempre, haya habido éxito o error.
                // Asegura que el estado 'IsBusy' se restablezca a false.
                IsBusy = false;
            }
        }
    }
}
