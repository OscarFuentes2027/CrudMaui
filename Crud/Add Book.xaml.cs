using Crud.Core.Entities;
using Crud.ViewModels;

namespace Crud;

public partial class Add_Book : ContentPage
{

   
        private readonly BookViewModel _viewModel;

        public Add_Book(BookViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = _viewModel = viewModel;
        }
   

}
