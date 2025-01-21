namespace Crud;

using Crud.ViewModels;
public partial class UserListPage : ContentPage
{
	public UserListPage(UserViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}