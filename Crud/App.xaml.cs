using Microsoft.EntityFrameworkCore;
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
