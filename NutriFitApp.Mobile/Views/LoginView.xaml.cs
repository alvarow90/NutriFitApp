// Archivo: LoginView.xaml.cs
using NutriFitApp.Mobile.ViewModels; // Aseg�rate de que el namespace del ViewModel es correcto

namespace NutriFitApp.Mobile.Views // Aseg�rate de que el namespace de la Vista es correcto
{
    public partial class LoginView : ContentPage
    {
        // El LoginViewModel se inyecta aqu� gracias a la configuraci�n que hiciste en MauiProgram.cs
        public LoginView(LoginViewModel viewModel) // El ViewModel se pasa como par�metro al constructor
        {
            InitializeComponent();

            // Esta es la l�nea crucial que faltaba:
            // Asigna la instancia del ViewModel (que contiene LoginCommand, Email, Password, etc.)
            // al BindingContext de la Vista. Sin esto, los enlaces {Binding ...} en el XAML no funcionar�n.
            BindingContext = viewModel;
        }
    }
}
