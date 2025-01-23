using Microsoft.Maui.Controls;
using Crud.ViewModels;
using System.Diagnostics;

namespace Crud
{
    public partial class NewPage1 : ContentPage
    {
        public NewPage1(UserViewModel viewModel)
        {


            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
