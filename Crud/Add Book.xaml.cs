using Crud.Core.Entities;
using Crud.ViewModels;

namespace Crud;

public partial class Add_Book : ContentPage
{
    public Add_Book()
    {
        InitializeComponent();
    }

    
    private void OnUserSelected(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        var selectedUser = (Usuarios)picker.SelectedItem;

        
        if (selectedUser != null)
        {
            var viewModel = (BookViewModel)BindingContext;
            viewModel.CurrentBook.UsuarioId = selectedUser.Id;  
        }
    }
}
