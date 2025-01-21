namespace Crud;
using Crud.ViewModels;
using Microsoft.Maui.Controls;
public partial class NewPage1 : ContentPage
{
	public NewPage1()
	{
		InitializeComponent();
     
    }
    public NewPage1(UserViewModel viewModel) 
    {
        InitializeComponent(); BindingContext = viewModel; 
    }
}