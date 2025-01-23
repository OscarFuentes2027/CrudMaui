using Microsoft.Maui.Controls;
using Crud.ViewModels;

namespace Crud
{
    public partial class UserListPage : ContentPage
    {
        private readonly UserViewModel _viewModel;

        public UserListPage(UserViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = _viewModel = viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.LoadUsersAsync(); // Carga los datos al aparecer la página
        }
    }
}
