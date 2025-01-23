using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Crud.Core.Entities;
using Crud.Core.Interfaces;


namespace Crud.ViewModels
{
    public partial class UserViewModel : ObservableObject
    {
        private readonly IRepository<Usuarios> _repository;

        [ObservableProperty]
        private ObservableCollection<Usuarios> _users = new(); // Inicializamos directamente para evitar valores nulos

        [ObservableProperty]
        private Usuarios _newUserName = new(); // Inicializamos con una nueva instancia para evitar valores nulos

        // Constructor sin parámetros público
        public UserViewModel()
        {
            // Constructor vacío necesario para XAML
        }

        // Constructor con parámetros para inyección de dependencias
        public UserViewModel(IRepository<Usuarios> repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [RelayCommand]
        public async Task LoadUsersAsync()
        {
            var users = await _repository.GetAllAsync();
            if (users != null && users.Any()) // Comprueba si hay datos válidos
            {
                Users = new ObservableCollection<Usuarios>(users);
            }
        }

        [RelayCommand]
        public async Task AddUserAsync()
        {
            NewUserName = new Usuarios(); // Reinicia la instancia del nuevo usuario
            await Shell.Current.GoToAsync(nameof(NewPage1));
        }

        [RelayCommand]
        public async Task SaveUserAsync()
        {
            if (NewUserName != null)
            {
                if (NewUserName.Id == 0)
                {
                    await _repository.AddAsync(NewUserName);
                }
                else
                {
                    await _repository.UpdateAsync(NewUserName);
                }
                await LoadUsersAsync();
                await Shell.Current.GoToAsync("..");
            }
        }

        [RelayCommand]
        public async Task EditUserAsync(Usuarios user)
        {
            if (user != null)
            {
                NewUserName = user; // Asigna el usuario seleccionado para edición
                await Shell.Current.GoToAsync(nameof(NewPage1));
            }
        }

        [RelayCommand]
        public async Task DeleteUserAsync(Usuarios user)
        {
            if (user != null)
            {
                await _repository.DeleteAsync(user);
                await LoadUsersAsync();
            }
        }

        [RelayCommand]
        public async Task CancelUserAsync()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
