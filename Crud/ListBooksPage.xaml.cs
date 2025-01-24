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
        await _viewModel.DebugDatabaseAsync();
        // Solo carga los datos si no se han cargado previamente
        if (_viewModel.Books == null || !_viewModel.Books.Any())
        {
            await _viewModel.LoadDataAsync();
        }
    }
}
