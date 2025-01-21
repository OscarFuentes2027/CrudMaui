using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Crud.Models;
using Crud.Repositories;

namespace Crud.ViewModels
{
    public partial class UserViewModel : ObservableObject
    {
        private readonly IRepository<Usuarios> _repository;

        [ObservableProperty]
        private ObservableCollection<Usuarios> _users;

        [ObservableProperty]
        private Usuarios _newUserName;

        // Constructor sin parámetros público
        public UserViewModel()
        {
            // Constructor vacío necesario para XAML
        }

        // Constructor con parámetros para inyección de dependencias
        public UserViewModel(IRepository<Usuarios> repository)
        {
            _repository = repository;
            _users = new ObservableCollection<Usuarios>();
        }

        [RelayCommand]
        public async Task LoadUsersAsync()
        {
            var users = await _repository.GetAllAsync();
            Users = new ObservableCollection<Usuarios>(users);
        }

        [RelayCommand]
        public async Task AddUserAsync()
        {
            NewUserName = new Usuarios(); 
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
            NewUserName = user; 
            await Shell.Current.GoToAsync(nameof(NewPage1));
        }

        [RelayCommand]
        public async Task DeleteUserAsync(Usuarios user)
        {
            await _repository.DeleteAsync(user);
            await LoadUsersAsync();
        }

        [RelayCommand]
        public async Task CancelUserAsync()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
