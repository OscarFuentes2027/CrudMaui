using System.Collections.ObjectModel;
using System.Diagnostics;
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
            Books = new ObservableCollection<Libro>(libros);

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
                    // Agregar un nuevo libro
                    await _bookRepository.AddAsync(CurrentBook);
                }
                else
                {
                    // Actualizar un libro existente
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
