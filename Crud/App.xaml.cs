using Crud.Infrastructure.Data;
using Crud.ViewModels;
using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Controls;  // Asegúrate de tener este using

namespace Crud
{
    public partial class App : Application
    {
        private readonly UserViewModel _userViewModel;
        private readonly DatabaseConnection _databaseConnection;

        public App(IServiceProvider serviceProvider)
        {
            InitializeComponent();

            // Obtén las dependencias necesarias
            _databaseConnection = serviceProvider.GetRequiredService<DatabaseConnection>();
            _userViewModel = serviceProvider.GetRequiredService<UserViewModel>();

            // Verifica la conexión con la base de datos
            using (var connection = _databaseConnection.CreateConnection())
            {
                try
                {
                    connection.Open();
                    Debug.WriteLine("Conexión a la base de datos establecida correctamente.");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error al conectar con la base de datos: {ex.Message}");
                }
            }
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            // Configurar la página inicial
            var appShell = new AppShell();
            var window = new Window(appShell);
            return window;
        }
    }
}
