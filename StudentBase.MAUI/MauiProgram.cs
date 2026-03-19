
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using StudentBase.Application.implementations;
using StudentBase.Application.Implementations;
using StudentBase.Application.Interfaces;
using StudentBase.Infrastructure.EntityFramework;
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
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            builder.Services.AddDbContext<AppDbContext>();
            builder.Services.AddSingleton<IDataService, DataService>();
            builder.Services.AddScoped<IProgramService, ProgramService>();
            builder.Services.AddScoped<IStudentService, StudentService>();

            builder.Services.AddTransient<StudentPageViewModel>(p => new StudentPageViewModel(p.GetRequiredService<IDataService>(),
                p.GetRequiredService<IStudentService>(),
                () => p.GetRequiredService<NewStudentPage>(),
                () => p.GetRequiredService<StudentCardPage>()));

            builder.Services.AddTransient<GroupPageViewModel>(g => new GroupPageViewModel(g.GetRequiredService<IDataService>(),
                () => g.GetRequiredService<NewGroupPage>(),
                () => g.GetRequiredService<GroupCardPage>()));

            builder.Services.AddTransient<ProgramPageViewModel>(p => new ProgramPageViewModel(p.GetRequiredService<IProgramService>(),
                () => p.GetRequiredService<NewProgramPage>(),
                () => p.GetRequiredService<ProgramCardPage>()));

            builder.Services.AddTransient<MainPageViewModel>(p => new MainPageViewModel(p.GetRequiredService<IDataService>(),
                () => p.GetRequiredService<NewStudentTransferPage>()));

            builder.Services.AddTransient<StudentTransferViewModel>(p => new StudentTransferViewModel(p.GetRequiredService<IDataService>(), 
                () => p.GetRequiredService<NewStudentTransferPage>(),
                () => p.GetRequiredService<StudentCardPage>()));

            builder.Services.AddTransient<PaymentPageViewModel>(p => new PaymentPageViewModel(p.GetRequiredService<IDataService>(),
                () => p.GetRequiredService<NewPaymentPage>()));

            builder.Services.AddTransient<NewStudentViewModel>();
            builder.Services.AddTransient<NewGroupViewModel>();
            builder.Services.AddTransient<NewProgramViewModel>();
            builder.Services.AddTransient<ProgramCardViewModel>();
            builder.Services.AddTransient<GroupCardViewModel>();
            builder.Services.AddTransient<NewStudentTransferViewModel>();
            builder.Services.AddTransient<StudentCardViewModel>();
            builder.Services.AddTransient<NewPaymentViewModel>();

            builder.Services.AddTransient<MainPage>();

            builder.Services.AddTransient<NewStudentPage>();
            builder.Services.AddTransient<NewGroupPage>();
            builder.Services.AddTransient<NewProgramPage>();
            builder.Services.AddTransient<ProgramCardPage>();
            builder.Services.AddTransient<GroupCardPage>();
            builder.Services.AddTransient<NewStudentTransferPage>();
            builder.Services.AddTransient<StudentCardPage>();
            builder.Services.AddTransient<NewPaymentPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
