using Microsoft.EntityFrameworkCore;
using Crud.Data;
using Crud.ViewModels;
using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace Crud
{
    public partial class App : Application
    {
        private readonly UserViewModel _userViewModel;
        private readonly AppDbContext _dbContext;

        public App(IServiceProvider serviceProvider)
        {
            InitializeComponent();

            _dbContext = serviceProvider.GetRequiredService<AppDbContext>();
            _userViewModel = serviceProvider.GetRequiredService<UserViewModel>();

            if (_dbContext.Database.EnsureCreated())
            {
                Debug.WriteLine("Base de datos creada correctamente.");
            }

            // Se elimina la línea de MainPage, ya que se configura en CreateWindow.
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
