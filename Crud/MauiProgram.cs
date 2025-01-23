using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Crud.Data;
using Crud.Repositories;
using Crud.ViewModels;
using Crud.Models;

namespace Crud
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // Configuración de la base de datos
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "MauiCrudApp.db");
            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                var dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..", "MauiCrudApp.db");
                options.UseSqlite($"Filename={dbPath}");
            });


            // Repositorios y ViewModels
            builder.Services.AddTransient<IRepository<Usuarios>, Repository<Usuarios>>();  // Usamos el repositorio genérico para Usuarios
            builder.Services.AddTransient<UserViewModel>();
            builder.Services.AddTransient<UserListPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
