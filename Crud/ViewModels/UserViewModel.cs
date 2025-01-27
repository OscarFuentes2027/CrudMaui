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
        public async Task LoadUserByIdAsync(int userId)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(userId);
                if (user != null)
                {
                    Debug.WriteLine($"Usuario encontrado: Id={user.Id}, Name={user.Name}, Email={user.Email}");
                    NewUserName = new Usuarios
                    {
                        Id = user.Id,
                        Name = user.Name,
                        Email = user.Email,
                        Password = user.Password
                    };
                }
                else
                {
                    Debug.WriteLine($"Usuario con Id={userId} no encontrado.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error en LoadUserByIdAsync: {ex.Message}");
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
            if (NewUserName == null)
            {
                Debug.WriteLine("No hay datos de usuario para guardar.");
                return;
            }

            try
            {
                await _semaphore.WaitAsync();
                Debug.WriteLine("Inicio de SaveUserAsync");

                if (NewUserName.Id > 0) // Actualizar si el Id ya existe
                {
                    Debug.WriteLine("Actualizando usuario...");
                    await _userRepository.UpdateAsync(NewUserName);
                    Debug.WriteLine("Usuario actualizado correctamente");
                }
                else // Crear nuevo usuario si Id es 0
                {
                    Debug.WriteLine("Añadiendo usuario...");
                    await _userRepository.AddAsync(NewUserName);
                    Debug.WriteLine("Usuario añadido correctamente");
                }

                // Recargar la lista de usuarios
                await LoadUsersAsync();
                Debug.WriteLine("Usuarios cargados después de guardar");

                // Regresar a la lista
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


        [RelayCommand]
        public async Task EditUserAsync(Usuarios user)
        {
            if (user == null)
            {
                Debug.WriteLine("El usuario es nulo, no se puede editar.");
                return;
            }

            Debug.WriteLine($"Editando usuario: Id={user.Id}, Name={user.Name}, Email={user.Email}");

            // Copia el usuario a NewUserName para editar
            NewUserName = new Usuarios
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Password = user.Password
            };

            // Navega a NewPage1 con el parámetro UserId
            var navigationParameters = new Dictionary<string, object>
    {
        { "UserId", user.Id }
    };

            await Shell.Current.GoToAsync(nameof(NewPage1), navigationParameters);
        }


        [RelayCommand]
        public async Task LoadUsersAsync()
        {
            try
            {
                Debug.WriteLine("Cargando usuarios desde la base de datos...");
                var users = await _userRepository.GetAllAsync(); // Obtén todos los usuarios desde el repositorio

                if (users != null && users.Any()) // Comprueba si hay datos válidos
                {
                    Users = new ObservableCollection<Usuarios>(users); // Asigna los usuarios a la propiedad ObservableCollection
                }

                Debug.WriteLine($"Usuarios cargados: {Users.Count}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error en LoadUsersAsync: {ex.Message}");
            }
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
