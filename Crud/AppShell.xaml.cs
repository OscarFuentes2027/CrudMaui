namespace Crud
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            // Registra la página NewPage1
            Routing.RegisterRoute(nameof(NewPage1), typeof(NewPage1));
        }
    }
}
