// Archivo: ViewModels/PerfilViewModel.cs
// Ubicación: Proyecto NutriFitApp.Mobile

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NutriFitApp.Mobile.Services;         // Para IPerfilService y ActualizarPerfilResult
using NutriFitApp.Shared.DTOs;           // Para UsuarioPerfilDTO y ActualizarUsuarioPerfilDTO
using System.Threading.Tasks;
using System.Diagnostics;
using System;                              // Para DateTime

namespace NutriFitApp.Mobile.ViewModels
{
    public partial class PerfilViewModel : ObservableObject
    {
        private readonly IPerfilService _perfilService;
        private UsuarioPerfilDTO? _perfilActual; // Para mantener una copia del perfil cargado

        // --- Propiedades Observables para la UI ---
        [ObservableProperty]
        private string? nombre;

        [ObservableProperty]
        private string? apellido;

        [ObservableProperty]
        private string? email; // Generalmente no editable, solo para mostrar

        [ObservableProperty]
        private string? rol; // Generalmente no editable, solo para mostrar

        [ObservableProperty]
        private DateTime? fechaNacimiento;

        [ObservableProperty]
        private double? alturaCm;

        [ObservableProperty]
        private double? pesoKg;

        [ObservableProperty]
        private string? objetivos;

        [ObservableProperty]
        private string? errorMessage;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotBusy))]
        private bool isBusy;

        public bool IsNotBusy => !IsBusy;

        // --- Comandos ---
        public IAsyncRelayCommand LoadPerfilCommand { get; }
        public IAsyncRelayCommand GuardarPerfilCommand { get; }

        public PerfilViewModel(IPerfilService perfilService)
        {
            _perfilService = perfilService;
            LoadPerfilCommand = new AsyncRelayCommand(ExecuteLoadPerfilAsync);
            GuardarPerfilCommand = new AsyncRelayCommand(ExecuteGuardarPerfilAsync);
            Debug.WriteLine("[PerfilViewModel] Constructor ejecutado.");
        }

        private async Task ExecuteLoadPerfilAsync()
        {
            if (IsBusy) return;
            IsBusy = true;
            ErrorMessage = null;
            Debug.WriteLine("[PerfilViewModel] ExecuteLoadPerfilAsync llamado.");

            try
            {
                _perfilActual = await _perfilService.GetMiPerfilAsync();
                if (_perfilActual != null)
                {
                    Nombre = _perfilActual.Nombre;
                    Apellido = _perfilActual.Apellido;
                    Email = _perfilActual.Email;
                    Rol = _perfilActual.Rol;
                    FechaNacimiento = _perfilActual.FechaNacimiento;
                    AlturaCm = _perfilActual.AlturaCm;
                    PesoKg = _perfilActual.PesoKg;
                    Objetivos = _perfilActual.Objetivos;
                    Debug.WriteLine($"[PerfilViewModel] Perfil cargado para usuario ID: {_perfilActual.Id}");
                }
                else
                {
                    ErrorMessage = "No se pudo cargar la información del perfil.";
                    Debug.WriteLine("[PerfilViewModel] GetMiPerfilAsync devolvió null.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[PerfilViewModel] Excepción en ExecuteLoadPerfilAsync: {ex.ToString()}");
                ErrorMessage = "Error al cargar el perfil.";
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task ExecuteGuardarPerfilAsync()
        {
            if (IsBusy) return;
            IsBusy = true;
            ErrorMessage = null;
            Debug.WriteLine("[PerfilViewModel] ExecuteGuardarPerfilAsync llamado.");

            try
            {
                // Crear el DTO para la actualización con los valores actuales de las propiedades observables
                var perfilParaActualizar = new ActualizarUsuarioPerfilDTO
                {
                    Nombre = this.Nombre ?? string.Empty,
                    Apellido = this.Apellido ?? string.Empty,
                    FechaNacimiento = this.FechaNacimiento,
                    AlturaCm = this.AlturaCm,
                    PesoKg = this.PesoKg,
                    Objetivos = this.Objetivos
                };

                var resultado = await _perfilService.ActualizarMiPerfilAsync(perfilParaActualizar);

                if (resultado.IsSuccess)
                {
                    Debug.WriteLine("[PerfilViewModel] Perfil actualizado exitosamente.");
                    // Opcional: Recargar el perfil para asegurar consistencia o mostrar un mensaje de éxito.
                    // await ExecuteLoadPerfilAsync(); // Podría ser redundante si la API devuelve el objeto actualizado
                    // O mostrar un mensaje al usuario:
                    // await Application.Current.MainPage.DisplayAlert("Éxito", "Perfil actualizado correctamente.", "OK");
                }
                else
                {
                    ErrorMessage = resultado.ErrorMessage ?? "No se pudo actualizar el perfil.";
                    Debug.WriteLine($"[PerfilViewModel] Error al actualizar perfil: {ErrorMessage}");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[PerfilViewModel] Excepción en ExecuteGuardarPerfilAsync: {ex.ToString()}");
                ErrorMessage = "Error al guardar el perfil.";
            }
            finally
            {
                IsBusy = false;
            }
        }

        // Método para ser llamado desde el OnAppearing de la vista
        public async Task OnAppearingAsync()
        {
            Debug.WriteLine("[PerfilViewModel] OnAppearingAsync llamado.");
            // Cargar el perfil si aún no se ha cargado o si se quiere refrescar.
            // Por ahora, lo cargamos siempre que la vista aparece si no está ya ocupado.
            if (LoadPerfilCommand.CanExecute(null))
            {
                await LoadPerfilCommand.ExecuteAsync(null);
            }
        }
    }
}
