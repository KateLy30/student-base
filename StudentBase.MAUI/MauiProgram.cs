using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using StudentBase.Application.Interfaces;
using StudentBase.Domain.Repositories;
using StudentBase.Domain.Services;
using StudentBase.Infrastructure.EntityFramework;
using StudentBase.Infrastructure.EntityFramework.Repositories;
using StudentBase.Infrastructure.Services;
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

            builder.Services.AddScoped<IExcelImportService, ExcelImportService>();

            builder.Services.AddScoped<IStudentRepository, StudentRepository>();
            builder.Services.AddScoped<IProgramRepository, ProgramRepository>();
            builder.Services.AddScoped<IGroupRepository, GroupRepository>();
            builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
            builder.Services.AddScoped<IStudentTransferRepository, StudentTransferRepository>();
            builder.Services.AddScoped<ICustomFieldRepository, CustomFieldRepository>();
            builder.Services.AddScoped<IDynamicFieldRepository,  DynamicFieldRepository>();
            builder.Services.AddScoped<IStudentTemplateRepository, StudentTemplateRepository>();
            builder.Services.AddScoped<IStudentTemplateColumnRepository, StudentTemplateColumnRepository>();

            builder.Services.AddSingleton<IDataService, DataService>();

            builder.Services.AddTransient<StudentPageViewModel>(p => new StudentPageViewModel(p.GetRequiredService<IDataService>(),
                () => p.GetRequiredService<NewStudentPage>(),
                () => p.GetRequiredService<StudentCardPage>(),
                () => p.GetRequiredService<NewStudentTransferPage>(),
                () => p.GetRequiredService<NewPaymentPage>(),
                () => p.GetRequiredService<TemplateManagementPage>()));

            builder.Services.AddTransient<GroupPageViewModel>(g => new GroupPageViewModel(g.GetRequiredService<IDataService>(),
                () => g.GetRequiredService<NewGroupPage>(),
                () => g.GetRequiredService<GroupCardPage>()));

            builder.Services.AddTransient<ProgramPageViewModel>(p => new ProgramPageViewModel(p.GetRequiredService<IDataService>(),
                () => p.GetRequiredService<NewProgramPage>(),
                () => p.GetRequiredService<ProgramCardPage>()));

            builder.Services.AddTransient<MainPageViewModel>(p => new MainPageViewModel(p.GetRequiredService<IDataService>(),
                () => p.GetRequiredService<NewStudentTransferPage>(),
                () => p.GetRequiredService<NewPaymentPage>()));

            builder.Services.AddTransient<StudentTransferViewModel>(p => new StudentTransferViewModel(p.GetRequiredService<IDataService>(), 
                () => p.GetRequiredService<NewStudentTransferPage>(),
                () => p.GetRequiredService<StudentCardPage>()));

            builder.Services.AddTransient<PaymentPageViewModel>(p => new PaymentPageViewModel(p.GetRequiredService<IDataService>(),
                () => p.GetRequiredService<NewPaymentPage>()));

            builder.Services.AddTransient<SettingsPageViewModel>(p => new SettingsPageViewModel(p.GetRequiredService<ICustomFieldRepository>(), 
                () => p.GetRequiredService<NewCustomFieldPage>()));

            builder.Services.AddTransient<TemplateManagementViewModel>(p => new TemplateManagementViewModel(p.GetRequiredService<IStudentTemplateRepository>(),
                () => p.GetRequiredService<TemplateEditorPage>(),
                p.GetRequiredService<IExcelImportService>()));

            builder.Services.AddTransient<NewStudentViewModel>();
            builder.Services.AddTransient<NewGroupViewModel>();
            builder.Services.AddTransient<NewProgramViewModel>();
            builder.Services.AddTransient<ProgramCardViewModel>();
            builder.Services.AddTransient<GroupCardViewModel>();
            builder.Services.AddTransient<NewStudentTransferViewModel>();
            builder.Services.AddTransient<StudentCardViewModel>();
            builder.Services.AddTransient<NewPaymentViewModel>();
            builder.Services.AddTransient<NewCustomFieldViewModel>();
            builder.Services.AddTransient<NewTemplateViewModel>();

            builder.Services.AddTransient<MainPage>();

            builder.Services.AddTransient<NewStudentPage>();
            builder.Services.AddTransient<NewGroupPage>();
            builder.Services.AddTransient<NewProgramPage>();
            builder.Services.AddTransient<ProgramCardPage>();
            builder.Services.AddTransient<GroupCardPage>();
            builder.Services.AddTransient<NewStudentTransferPage>();
            builder.Services.AddTransient<StudentCardPage>();
            builder.Services.AddTransient<NewPaymentPage>();
            builder.Services.AddTransient<NewCustomFieldPage>();
            builder.Services.AddTransient<NewCustomFieldPage>();
            builder.Services.AddTransient<TemplateEditorPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
