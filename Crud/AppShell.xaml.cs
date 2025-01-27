namespace Crud
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            // Registra la página NewPage1
            Routing.RegisterRoute("NewPage1", typeof(NewPage1));
            Routing.RegisterRoute("UserListPage", typeof(UserListPage));
            Routing.RegisterRoute("Add_Book", typeof(Add_Book));
            Routing.RegisterRoute("ListBooksPage", typeof(ListBooksPage));
        }
    }
}
