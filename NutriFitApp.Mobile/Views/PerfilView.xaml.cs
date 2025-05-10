// Archivo: Views/PerfilView.xaml.cs
// Ubicación: Proyecto NutriFitApp.Mobile
using NutriFitApp.Mobile.ViewModels; // Namespace de tu PerfilViewModel
using System.Diagnostics;          // Para Debug.WriteLine

namespace NutriFitApp.Mobile.Views
{
    public partial class PerfilView : ContentPage
    {
        private readonly PerfilViewModel _viewModel;

        // Constructor que recibe PerfilViewModel mediante inyección de dependencias
        public PerfilView(PerfilViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = _viewModel; // Establecer el BindingContext de la vista al ViewModel
            Debug.WriteLine("[PerfilView] Constructor ejecutado y BindingContext establecido.");
        }

        // Método que se ejecuta cuando la página está a punto de mostrarse
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            Debug.WriteLine("[PerfilView] OnAppearing llamado.");
            // Llama al método OnAppearingAsync del ViewModel para cargar datos si es necesario.
            if (_viewModel != null && _viewModel.LoadPerfilCommand.CanExecute(null))
            {
                // Podrías llamar directamente a _viewModel.OnAppearingAsync() si prefieres
                // que la lógica de si cargar o no esté completamente en el ViewModel.
                // Aquí estamos llamando directamente al comando de carga.
                await _viewModel.LoadPerfilCommand.ExecuteAsync(null);
            }
            else if (_viewModel == null)
            {
                Debug.WriteLine("[PerfilView] Advertencia: _viewModel es null en OnAppearing.");
            }
        }
    }
}
