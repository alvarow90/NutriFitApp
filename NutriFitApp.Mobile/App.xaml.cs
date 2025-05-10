// Archivo: App.xaml.cs
// Asegúrate de tener los usings necesarios, por ejemplo:
// using NutriFitApp.Mobile.Views; // Si tus vistas están aquí

namespace NutriFitApp.Mobile
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // Simplemente establece AppShell como la página principal.
            // AppShell se encargará de la navegación inicial.
            MainPage = new AppShell();
        }

        // Puedes dejar los métodos OnStart, OnSleep, OnResume si los necesitas para otra lógica.
    }
}
