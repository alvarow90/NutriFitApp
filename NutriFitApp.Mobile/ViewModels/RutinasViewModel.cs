// Archivo: ViewModels/RutinasViewModel.cs
// Ubicación: Proyecto NutriFitApp.Mobile

using CommunityToolkit.Mvvm.ComponentModel; // Para ObservableObject y ObservableProperty
using CommunityToolkit.Mvvm.Input;         // Para AsyncRelayCommand
using NutriFitApp.Mobile.Services;         // Para IRutinaService
using NutriFitApp.Shared.DTOs;           // Para RutinaDTO
using System.Collections.ObjectModel;      // Para ObservableCollection
using System.Threading.Tasks;              // Para Task
using System.Diagnostics;                  // Para Debug.WriteLine
// using Microsoft.Maui.ApplicationModel;     // Para MainThread, si es necesario (AsyncRelayCommand usualmente lo maneja)

namespace NutriFitApp.Mobile.ViewModels
{
    public partial class RutinasViewModel : ObservableObject
    {
        private readonly IRutinaService _rutinaService; // Servicio para obtener datos de rutinas

        [ObservableProperty]
        private ObservableCollection<RutinaDTO> rutinas; // Colección de rutinas para mostrar en la UI

        [ObservableProperty]
        private string errorMessage; // Para mostrar mensajes de error

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotBusy))]
        private bool isBusy; // Indica si se está cargando datos

        public bool IsNotBusy => !IsBusy;

        // Comando para ejecutar desde la UI, enlazado al método ExecuteLoadRutinasAsync
        public IAsyncRelayCommand LoadRutinasCommand { get; }

        // Constructor que recibe IRutinaService por inyección de dependencias
        public RutinasViewModel(IRutinaService rutinaService)
        {
            _rutinaService = rutinaService;
            Rutinas = new ObservableCollection<RutinaDTO>(); // Inicializar la colección
            LoadRutinasCommand = new AsyncRelayCommand(ExecuteLoadRutinasAsync);
            Debug.WriteLine("[RutinasViewModel] Constructor ejecutado. IRutinaService inyectado.");
        }

        // Método privado que es ejecutado por el comando LoadRutinasCommand
        private async Task ExecuteLoadRutinasAsync()
        {
            if (IsBusy) // Evitar múltiples cargas simultáneas
                return;

            IsBusy = true;
            ErrorMessage = string.Empty;
            Debug.WriteLine("[RutinasViewModel] ExecuteLoadRutinasAsync llamado.");

            try
            {
                // Limpiar la colección actual antes de cargar nuevas rutinas
                if (Rutinas.Count > 0)
                {
                    Rutinas.Clear();
                }

                var rutinasList = await _rutinaService.GetMisRutinasAsync(); // Llama al servicio

                if (rutinasList != null && rutinasList.Any())
                {
                    foreach (var rutina in rutinasList)
                    {
                        Rutinas.Add(rutina);
                    }
                    Debug.WriteLine($"[RutinasViewModel] {Rutinas.Count} rutinas cargadas en la colección.");
                }
                else
                {
                    Debug.WriteLine("[RutinasViewModel] GetMisRutinasAsync devolvió null o una lista vacía.");
                    // ErrorMessage = "No tienes rutinas asignadas actualmente."; // Mensaje opcional para la UI
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[RutinasViewModel] Excepción en ExecuteLoadRutinasAsync: {ex.ToString()}");
                ErrorMessage = "Ocurrió un error al cargar las rutinas.";
            }
            finally
            {
                IsBusy = false; // Asegurar que IsBusy se restablezca
            }
        }

        // Método que se puede llamar cuando la página/vista que usa este ViewModel aparece
        public async Task OnAppearingAsync()
        {
            Debug.WriteLine("[RutinasViewModel] OnAppearingAsync llamado.");
            // Cargar las rutinas automáticamente cuando la vista aparece,
            // solo si la colección está vacía para evitar recargas innecesarias.
            if (Rutinas.Count == 0)
            {
                if (LoadRutinasCommand.CanExecute(null))
                {
                    await LoadRutinasCommand.ExecuteAsync(null);
                }
            }
        }
    }
}
