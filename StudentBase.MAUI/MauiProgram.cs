
using Microsoft.Extensions.Logging;
using StudentBase.Application.Interfaces;
using StudentBase.Domain.Repositories;
using StudentBase.Infrastructure.EntityFramework;
using StudentBase.Infrastructure.EntityFramework.Repositories;
using StudentBase.MAUI.ViewModels;
using StudentBase.MAUI.Views;

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

            builder.Services.AddSingleton<IDataService, DataService>();
            builder.Services.AddScoped<AppDbContext>();

            builder.Services.AddTransient<StudentPageViewModel>(p => new StudentPageViewModel(p.GetRequiredService<IDataService>(),
                () => p.GetRequiredService<NewStudentModalWindow>()));

            builder.Services.AddTransient<GroupPageViewModel>(g => new GroupPageViewModel(g.GetRequiredService<IDataService>(),
                () => g.GetRequiredService<NewGroupModalWindow>()));

            builder.Services.AddTransient<ProgramPageViewModel>(p => new ProgramPageViewModel(p.GetRequiredService<IDataService>(),
                () => p.GetRequiredService<NewProgramModalWindow>(),
                () => p.GetRequiredService<ProgramCardModalWindow>()));

            builder.Services.AddTransient<NewStudentViewModel>();
            builder.Services.AddTransient<NewGroupViewModel>();
            builder.Services.AddTransient<NewProgramViewModel>();
            builder.Services.AddTransient<ProgramCardViewModel>();

            builder.Services.AddTransient<MainPage>();

            builder.Services.AddTransient<NewStudentModalWindow >();
            builder.Services.AddTransient<NewGroupModalWindow>();
            builder.Services.AddTransient<NewProgramModalWindow>();
            builder.Services.AddTransient<ProgramCardModalWindow>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
