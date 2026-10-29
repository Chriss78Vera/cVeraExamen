using Microsoft.Extensions.DependencyInjection;

namespace cVeraExamen
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            // NavigationPage permite mantener el flujo Login -> Registro -> Resumen.
            return new Window(new NavigationPage(new Views.vLogin()));
        }
    }
}
