// Archivo: LoginView.xaml.cs
using NutriFitApp.Mobile.ViewModels; // Asegúrate de que el namespace del ViewModel es correcto

namespace NutriFitApp.Mobile.Views // Asegúrate de que el namespace de la Vista es correcto
{
    public partial class LoginView : ContentPage
    {
        // El LoginViewModel se inyecta aquí gracias a la configuración que hiciste en MauiProgram.cs
        public LoginView(LoginViewModel viewModel) // El ViewModel se pasa como parámetro al constructor
        {
            InitializeComponent();

            // Esta es la línea crucial que faltaba:
            // Asigna la instancia del ViewModel (que contiene LoginCommand, Email, Password, etc.)
            // al BindingContext de la Vista. Sin esto, los enlaces {Binding ...} en el XAML no funcionarán.
            BindingContext = viewModel;
        }
    }
}
