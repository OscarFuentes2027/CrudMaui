using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Crud.Core.Entities;
using Crud.Infrastructure.Repositories;

namespace Crud.ViewModels
{
    public partial class UserViewModel : ObservableObject
    {
        private readonly UserRepository _userRepository;

        [ObservableProperty]
        private ObservableCollection<Usuarios> _users = new(); // Inicializamos directamente para evitar valores nulos

        [ObservableProperty]
        private Usuarios _newUserName = new(); // Inicializamos con una nueva instancia para evitar valores nulos

        private readonly SemaphoreSlim _semaphore = new(1, 1); // Garantiza acceso exclusivo a la base de datos

        // Constructor con inyección de dependencias
        public UserViewModel(UserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public UserViewModel()
        {
        }

        // Método para cargar usuarios desde la base de datos
        [RelayCommand]
        public async Task LoadUsersAsync()
        {
            try
            {
                await _semaphore.WaitAsync(); // Garantiza acceso exclusivo
                Debug.WriteLine("Cargando usuarios desde la base de datos...");
                var users = await _userRepository.GetAllAsync();
                if (users != null && users.Any()) // Comprueba si hay datos válidos
                {
                    Users = new ObservableCollection<Usuarios>(users);
                }
                Debug.WriteLine($"Usuarios cargados: {Users.Count}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error en LoadUsersAsync: {ex.Message}");
            }
            finally
            {
                _semaphore.Release();
            }
        }

        // Método para agregar un nuevo usuario
        [RelayCommand]
        public async Task AddUserAsync()
        {
            NewUserName = new Usuarios(); // Reinicia la instancia del nuevo usuario
            await Shell.Current.GoToAsync(nameof(NewPage1));
        }

        // Método para guardar (crear o actualizar) un usuario
        [RelayCommand]
        public async Task SaveUserAsync()
        {
            if (NewUserName != null)
            {
                try
                {
                    await _semaphore.WaitAsync();
                    Debug.WriteLine("Inicio de SaveUserAsync");
                    if (NewUserName.Id == 0)
                    {
                        Debug.WriteLine("Añadiendo usuario...");
                        await _userRepository.AddAsync(NewUserName);
                        Debug.WriteLine("Usuario añadido correctamente");
                    }
                    else
                    {
                        Debug.WriteLine("Actualizando usuario...");
                        await _userRepository.UpdateAsync(NewUserName);
                        Debug.WriteLine("Usuario actualizado correctamente");
                    }

                    await LoadUsersAsync();
                    Debug.WriteLine("Usuarios cargados después de guardar");

                    await Shell.Current.GoToAsync("..");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error en SaveUserAsync: {ex.Message}");
                }
                finally
                {
                    _semaphore.Release();
                }
            }
        }

        // Método para editar un usuario (prepararlo para edición)
        [RelayCommand]
        public async Task EditUserAsync(Usuarios user)
        {
            if (user == null)
            {
                Debug.WriteLine("El usuario es nulo, no se puede editar.");
                return;
            }

            Debug.WriteLine($"Editando usuario: Id={user.Id}, Name={user.Name}, Email={user.Email}");

            NewUserName = user; // Asigna el usuario seleccionado para edición
            await Shell.Current.GoToAsync(nameof(NewPage1));
        }

        // Método para eliminar un usuario
        [RelayCommand]
        public async Task DeleteUserAsync(Usuarios user)
        {
            if (user != null)
            {
                try
                {
                    await _semaphore.WaitAsync();
                    Debug.WriteLine($"Eliminando usuario: Id={user.Id}");
                    await _userRepository.DeleteAsync(user);
                    Debug.WriteLine("Usuario eliminado correctamente");

                    await LoadUsersAsync();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error al eliminar usuario: {ex.Message}");
                }
                finally
                {
                    _semaphore.Release();
                }
            }
        }

        // Método para cancelar la acción actual
        [RelayCommand]
        public async Task CancelUserAsync()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
