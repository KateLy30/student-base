
using StudentBase.Infrastructure.EntityFramework;

namespace StudentBase.MAUI
{
    public partial class App : Microsoft.Maui.Controls.Application
    {
        public static IServiceProvider? Services { get; private set; }
        public App(IServiceProvider serviceProvider)
        {
            InitializeComponent();
            Services = serviceProvider;
            using (var scope = Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                dbContext.Database.EnsureCreated();
            }
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}