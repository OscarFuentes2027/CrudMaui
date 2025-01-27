namespace Crud;

using Crud.ViewModels;

public partial class ListBooksPage : ContentPage
{
    private readonly BookViewModel _viewModel;

    public ListBooksPage(BookViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await _viewModel.DebugDatabaseAsync(); // Esto es opcional si es solo para debug
        await _viewModel.LoadDataAsync(); // Siempre recarga los datos
    }
}
