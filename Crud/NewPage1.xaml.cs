using Microsoft.Maui.Controls;
using Crud.ViewModels;
using System.Diagnostics;

namespace Crud
{

    [QueryProperty(nameof(UserId), "UserId")]
    public partial class NewPage1 : ContentPage
    {
        private readonly UserViewModel _viewModel;

        public int UserId { get; set; } // Parámetro que se recibe desde la navegación

        public NewPage1(UserViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = _viewModel = viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (UserId > 0) // Verifica si se pasó un UserId válido
            {
                Debug.WriteLine($"Cargando datos para el usuario con Id={UserId}");
                await _viewModel.LoadUserByIdAsync(UserId);
            }
        }
    }
}
