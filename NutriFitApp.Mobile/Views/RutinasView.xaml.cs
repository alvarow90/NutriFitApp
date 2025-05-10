// Archivo: Views/RutinasView.xaml.cs
// Ubicación: Proyecto NutriFitApp.Mobile
using NutriFitApp.Mobile.ViewModels; // Namespace de tu RutinasViewModel
using System.Diagnostics;          // Para Debug.WriteLine

namespace NutriFitApp.Mobile.Views
{
    public partial class RutinasView : ContentPage
    {
        // Campo privado para mantener una referencia al ViewModel.
        private readonly RutinasViewModel _viewModel;

        // Constructor de la vista.
        // Recibe RutinasViewModel mediante inyección de dependencias (configurado en MauiProgram.cs).
        public RutinasView(RutinasViewModel viewModel)
        {
            InitializeComponent(); // Carga los componentes definidos en RutinasView.xaml.

            _viewModel = viewModel; // Guarda la referencia al ViewModel.
            BindingContext = _viewModel; // Establece el BindingContext de la vista al ViewModel.
                                         // Esto es crucial para que los enlaces de datos ({Binding ...}) en el XAML funcionen.

            Debug.WriteLine("[RutinasView] Constructor ejecutado y BindingContext establecido a RutinasViewModel.");
        }

        // Método que se ejecuta cuando la página está a punto de mostrarse en la pantalla.
        protected override async void OnAppearing()
        {
            base.OnAppearing(); // Llamar a la implementación base.
            Debug.WriteLine("[RutinasView] OnAppearing llamado.");

            // Llama al método OnAppearingAsync del ViewModel para que pueda cargar
            // los datos necesarios (las rutinas) si aún no se han cargado.
            if (_viewModel != null)
            {
                // El ViewModel se encargará de la lógica de IsBusy y de si necesita recargar.
                await _viewModel.OnAppearingAsync();
            }
            else
            {
                Debug.WriteLine("[RutinasView] Advertencia: _viewModel es null en OnAppearing.");
            }
        }
    }
}
