// Archivo: Views/DietasView.xaml.cs
// Ubicación: Proyecto NutriFitApp.Mobile
using NutriFitApp.Mobile.ViewModels; // Namespace de tu DietasViewModel
using System.Diagnostics;          // Para Debug.WriteLine

namespace NutriFitApp.Mobile.Views
{
    public partial class DietasView : ContentPage
    {
        // Campo privado para mantener una referencia al ViewModel.
        // Esto es útil si necesitas llamar a métodos del ViewModel directamente desde el code-behind,
        // aunque la mayoría de las interacciones se harán a través de comandos y bindings.
        private readonly DietasViewModel _viewModel;

        // Constructor de la vista.
        // Recibe DietasViewModel mediante inyección de dependencias (configurado en MauiProgram.cs).
        public DietasView(DietasViewModel viewModel)
        {
            InitializeComponent(); // Este método carga los componentes definidos en DietasView.xaml.

            _viewModel = viewModel; // Guardar la referencia al ViewModel inyectado.
            BindingContext = _viewModel; // Establecer el BindingContext de la vista al ViewModel.
                                         // Esto es crucial para que los enlaces de datos ({Binding ...}) en el XAML funcionen.

            Debug.WriteLine("[DietasView] Constructor ejecutado y BindingContext establecido a DietasViewModel.");
        }

        // Método que se ejecuta cuando la página está a punto de mostrarse en la pantalla.
        // Es un buen lugar para iniciar la carga de datos o realizar otras acciones de inicialización de la vista.
        protected override async void OnAppearing()
        {
            base.OnAppearing(); // Llamar a la implementación base del método.
            Debug.WriteLine("[DietasView] OnAppearing llamado.");

            // Llama al método OnAppearingAsync del ViewModel para que pueda cargar
            // los datos necesarios (en este caso, las dietas) si aún no se han cargado.
            if (_viewModel != null)
            {
                // El ViewModel se encargará de la lógica de IsBusy y de si necesita recargar.
                await _viewModel.OnAppearingAsync();
            }
            else
            {
                Debug.WriteLine("[DietasView] Advertencia: _viewModel es null en OnAppearing.");
            }
        }

        // Podrías añadir aquí otros manejadores de eventos de UI si fueran necesarios,
        // aunque la mayoría de la lógica de interacción debería estar en el ViewModel
        // y manejarse a través de Comandos.
    }
}
