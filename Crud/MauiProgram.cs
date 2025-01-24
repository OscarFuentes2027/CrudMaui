using Microsoft.Extensions.Logging;
using Crud.Infrastructure.Data;
using Crud.Infrastructure.Repositories;
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

            // Registrar DatabaseConnection
            builder.Services.AddSingleton<DatabaseConnection>();

            // Registrar UserViewModel y otros servicios necesarios
            builder.Services.AddTransient<UserViewModel>();

            // Registrar BookViewModel (si se usa)
            builder.Services.AddTransient<BookViewModel>();

            // Registrar repositorios (si usas Dapper o algún otro repositorio específico)
            builder.Services.AddTransient<UserRepository>();
            builder.Services.AddTransient<BookRepository>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
