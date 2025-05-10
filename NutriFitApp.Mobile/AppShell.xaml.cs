namespace NutriFitApp.Mobile;
using NutriFitApp.Mobile.Views;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        // Opcional: registrar rutas si usas navegación programática
        Routing.RegisterRoute("login", typeof(Views.LoginView));
        Routing.RegisterRoute("dashboard", typeof(Views.DashboardView));
        Routing.RegisterRoute("dietas", typeof(Views.DietasView));
        Routing.RegisterRoute("rutinas", typeof(Views.RutinasView));
        Routing.RegisterRoute("chat", typeof(Views.ChatView));
        Routing.RegisterRoute("perfil", typeof(PerfilView));
    }
}
