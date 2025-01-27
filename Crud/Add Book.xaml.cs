using Crud.Core.Entities;
using Crud.ViewModels;
using System.Diagnostics;

namespace Crud;

    [QueryProperty(nameof(BookId), "BookId")]
    public partial class Add_Book : ContentPage
    {
        private readonly BookViewModel _viewModel;

        public int BookId { get; set; } // Parámetro que se recibe desde la navegación

        public Add_Book(BookViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = _viewModel = viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (BookId > 0) // Verifica si se pasó un BookId válido
            {
                Debug.WriteLine($"Cargando datos para el libro con Id={BookId}");
                await _viewModel.LoadBookByIdAsync(BookId);
            }
        }
    }
