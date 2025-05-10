// Archivo: Views/DietasView.xaml.cs
// Ubicaci�n: Proyecto NutriFitApp.Mobile
using NutriFitApp.Mobile.ViewModels; // Namespace de tu DietasViewModel
using System.Diagnostics;          // Para Debug.WriteLine

namespace NutriFitApp.Mobile.Views
{
    public partial class DietasView : ContentPage
    {
        // Campo privado para mantener una referencia al ViewModel.
        // Esto es �til si necesitas llamar a m�todos del ViewModel directamente desde el code-behind,
        // aunque la mayor�a de las interacciones se har�n a trav�s de comandos y bindings.
        private readonly DietasViewModel _viewModel;

        // Constructor de la vista.
        // Recibe DietasViewModel mediante inyecci�n de dependencias (configurado en MauiProgram.cs).
        public DietasView(DietasViewModel viewModel)
        {
            InitializeComponent(); // Este m�todo carga los componentes definidos en DietasView.xaml.

            _viewModel = viewModel; // Guardar la referencia al ViewModel inyectado.
            BindingContext = _viewModel; // Establecer el BindingContext de la vista al ViewModel.
                                         // Esto es crucial para que los enlaces de datos ({Binding ...}) en el XAML funcionen.

            Debug.WriteLine("[DietasView] Constructor ejecutado y BindingContext establecido a DietasViewModel.");
        }

        // M�todo que se ejecuta cuando la p�gina est� a punto de mostrarse en la pantalla.
        // Es un buen lugar para iniciar la carga de datos o realizar otras acciones de inicializaci�n de la vista.
        protected override async void OnAppearing()
        {
            base.OnAppearing(); // Llamar a la implementaci�n base del m�todo.
            Debug.WriteLine("[DietasView] OnAppearing llamado.");

            // Llama al m�todo OnAppearingAsync del ViewModel para que pueda cargar
            // los datos necesarios (en este caso, las dietas) si a�n no se han cargado.
            if (_viewModel != null)
            {
                // El ViewModel se encargar� de la l�gica de IsBusy y de si necesita recargar.
                await _viewModel.OnAppearingAsync();
            }
            else
            {
                Debug.WriteLine("[DietasView] Advertencia: _viewModel es null en OnAppearing.");
            }
        }

        // Podr�as a�adir aqu� otros manejadores de eventos de UI si fueran necesarios,
        // aunque la mayor�a de la l�gica de interacci�n deber�a estar en el ViewModel
        // y manejarse a trav�s de Comandos.
    }
}
