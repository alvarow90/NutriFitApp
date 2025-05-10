// Archivo: ViewModels/DietasViewModel.cs
// Ubicación: Proyecto NutriFitApp.Mobile

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NutriFitApp.Mobile.Services;
using NutriFitApp.Shared.DTOs;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Diagnostics;

namespace NutriFitApp.Mobile.ViewModels
{
    public partial class DietasViewModel : ObservableObject
    {
        private readonly IDietaService _dietaService;

        [ObservableProperty]
        private ObservableCollection<DietaDTO> dietas;

        [ObservableProperty]
        private string errorMessage;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotBusy))]
        private bool isBusy;

        public bool IsNotBusy => !IsBusy;

        public IAsyncRelayCommand LoadDietasCommand { get; }

        public DietasViewModel(IDietaService dietaService)
        {
            _dietaService = dietaService;
            Dietas = new ObservableCollection<DietaDTO>();
            LoadDietasCommand = new AsyncRelayCommand(ExecuteLoadDietasAsync); // Renombrado el método interno para claridad
            Debug.WriteLine("[DietasViewModel] Constructor ejecutado. IDietaService inyectado.");
        }

        // Método privado que es ejecutado por el comando LoadDietasCommand.
        private async Task ExecuteLoadDietasAsync()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            ErrorMessage = string.Empty;
            Debug.WriteLine("[DietasViewModel] ExecuteLoadDietasAsync llamado.");

            try
            {
                if (Dietas.Count > 0)
                {
                    Dietas.Clear();
                }

                // Llamar al método actualizado del servicio para obtener "mis dietas".
                var dietasList = await _dietaService.GetMisDietasAsync();

                if (dietasList != null && dietasList.Any())
                {
                    foreach (var dieta in dietasList)
                    {
                        Dietas.Add(dieta);
                    }
                    Debug.WriteLine($"[DietasViewModel] {Dietas.Count} dietas cargadas en la colección.");
                }
                else
                {
                    Debug.WriteLine("[DietasViewModel] GetMisDietasAsync devolvió null o una lista vacía.");
                    // ErrorMessage = "No tienes dietas asignadas actualmente."; // Mensaje opcional para la UI
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[DietasViewModel] Excepción en ExecuteLoadDietasAsync: {ex.ToString()}");
                ErrorMessage = "Ocurrió un error al cargar las dietas.";
            }
            finally
            {
                IsBusy = false;
            }
        }

        // Método para ser llamado desde el OnAppearing de la vista.
        public async Task OnAppearingAsync()
        {
            Debug.WriteLine("[DietasViewModel] OnAppearingAsync llamado.");
            // Cargar solo si la colección está vacía para evitar recargas innecesarias,
            // o si quieres refrescar siempre, quita la condición Dietas.Count == 0.
            if (Dietas.Count == 0)
            {
                if (LoadDietasCommand.CanExecute(null))
                {
                    await LoadDietasCommand.ExecuteAsync(null);
                }
            }
        }
    }
}
