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
    public partial class BookViewModel : ObservableObject
    {
        private readonly BookRepository _bookRepository;
        private readonly UserRepository _userRepository;

        [ObservableProperty]
        private ObservableCollection<Libro> _books = new();

        [ObservableProperty]
        private ObservableCollection<Usuarios> _users = new();

        [ObservableProperty]
        private Libro currentBook = new();

        private readonly SemaphoreSlim _semaphore = new(1, 1);

        // Constructor con inyección de dependencias
        public BookViewModel(BookRepository bookRepository, UserRepository userRepository)
        {
            _bookRepository = bookRepository ?? throw new ArgumentNullException(nameof(bookRepository));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }
        public BookViewModel()
        {
            // Este constructor es necesario si usas XAML para instanciar BookViewModel
        }

        // Método para cargar datos (libros y usuarios)
        [RelayCommand]
        public async Task LoadDataAsync()
        {
            Debug.WriteLine("Cargando libros y usuarios...");

            // Carga los libros desde la base de datos
            var libros = await _bookRepository.GetAllAsync();
            Books.Clear();
            foreach (var libro in libros)
            {
                Books.Add(libro);
            }


            // Carga los usuarios desde la base de datos
            var usuarios = await _userRepository.GetAllAsync();
            Users = new ObservableCollection<Usuarios>(usuarios);

            Debug.WriteLine($"Libros cargados: {Books.Count}");
            foreach (var libro in Books)
            {
                Debug.WriteLine($"Libro: Id={libro.Id}, Titulo={libro.Titulo}, UsuarioId={libro.UsuarioId}");
            }

            Debug.WriteLine($"Usuarios cargados: {Users.Count}");
        }

        // Método para agregar o actualizar un libro
        [RelayCommand]
        private async Task SaveData()
        {
            if (CurrentBook != null)
            {
                if (CurrentBook.Id == 0)
                {
                    Debug.WriteLine("Añadiendo nuevo libro...");
                    await _bookRepository.AddAsync(CurrentBook);
                }
                else
                {
                    Debug.WriteLine("Actualizando libro existente...");
                    await _bookRepository.UpdateAsync(CurrentBook);
                }

                await LoadDataAsync();
                await Shell.Current.GoToAsync("..");
            }
        }



        // Método para agregar un nuevo libro (navega a la página de agregar libro)
        [RelayCommand]
        public async Task AddBookAsync()
        {
            CurrentBook = new Libro(); // Reinicia el libro actual
            await Shell.Current.GoToAsync(nameof(Add_Book)); // Navega a la página de agregar libro
        }

        [RelayCommand]
        public async Task DeleteBookAsync(Libro libro)
        {
            if (libro != null)
            {
                try
                {
                    await _semaphore.WaitAsync();
                    Debug.WriteLine($"Eliminando usuario: Id={libro}.Id");
                    await _bookRepository.DeleteAsync(libro);
                    Debug.WriteLine("Usuario eliminado correctamente");

                    await LoadDataAsync();
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

        public async Task LoadBookByIdAsync(int bookId)
        {
            try
            {
                var book = await _bookRepository.GetByIdAsync(bookId);
                if (book != null)
                {
                    Debug.WriteLine($"Libro encontrado: Id={book.Id}, Titulo={book.Titulo}, Genero={book.Genero}");
                    CurrentBook = new Libro
                    {
                        Id = book.Id,
                        Titulo = book.Titulo,
                        Genero = book.Genero,
                        FechaPublicacion = book.FechaPublicacion,
                        UsuarioId = book.UsuarioId
                    };
                }
                else
                {
                    Debug.WriteLine($"Libro con Id={bookId} no encontrado.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error en LoadBookByIdAsync: {ex.Message}");
            }
        }


        [RelayCommand]
        public async Task EditBookAsync(Libro libro)
        {
            if (libro == null)
            {
                Debug.WriteLine("El libro es nulo, no se puede editar.");
                return;
            }

            Debug.WriteLine($"Editando libro: Id={libro.Id}, Titulo={libro.Titulo}, Genero={libro.Genero}");

            // Navegar a la página de edición con el ID del libro como parámetro
            var navigationParameters = new Dictionary<string, object>
    {
        { "BookId", libro.Id }
    };

            await Shell.Current.GoToAsync(nameof(Add_Book), navigationParameters);
        }

        // Método de depuración para imprimir las tablas y libros en la base de datos
        public async Task DebugDatabaseAsync()
        {
            var libros = (await _bookRepository.GetAllAsync()).ToList(); // Convierte a lista para acceder a Count
            Debug.WriteLine($"Libros encontrados: {libros.Count}");
            foreach (var libro in libros)
            {
                Debug.WriteLine($"Libro: Id={libro.Id}, Titulo={libro.Titulo}, Genero={libro.Genero}, FechaPublicacion={libro.FechaPublicacion}, UsuarioId={libro.UsuarioId}");
            }
        }

    }
}
