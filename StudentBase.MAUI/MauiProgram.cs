
using Microsoft.Extensions.Logging;
using StudentBase.Domain.Repositories;
using StudentBase.Infrastructure.EntityFramework;
using StudentBase.Infrastructure.EntityFramework.Repositories;

namespace StudentBase.MAUI
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

            builder.Services.AddSingleton<IStudentRepository, StudentRepository>();
            builder.Services.AddScoped<AppDbContext>();


#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
