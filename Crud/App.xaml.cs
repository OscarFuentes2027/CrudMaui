using Microsoft.EntityFrameworkCore;
using Crud.Data;
using Crud.ViewModels;

namespace Crud
{
    public partial class App : Application
    {
        private readonly UserViewModel _userViewModel;
        private readonly AppDbContext _dbContext;

        public App(AppDbContext dbContext, UserViewModel userViewModel)
        {
            InitializeComponent();
            _dbContext = dbContext;
            _userViewModel = userViewModel;
            dbContext.Database.EnsureCreated();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            
            var appShell = new AppShell();
            var navigationPage = new NavigationPage(new NewPage1(_userViewModel));
            var window = new Window(appShell);

            return window;
        }
    }
}
