namespace StudentBase.MAUI
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(StudentPage), typeof(StudentPage));
        }
    }
}
