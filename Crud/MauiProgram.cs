using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Crud.Data;
using Crud.Repositories;
using Crud.ViewModels;

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

            // Aqui estoy registrando el contexto de la base de datos

            builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite($"Filename={System.IO.Path.Combine(FileSystem.AppDataDirectory, "MauiCrudApp.db")}"));

            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            builder.Services.AddTransient<UserViewModel>();


#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
