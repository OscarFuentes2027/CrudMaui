using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Crud.Core.Entities;
using Crud.Core.Interfaces;

namespace Crud.ViewModels
{
    public partial class BookViewModel : ObservableObject
    {
        public IRepository<Libro> BookRepository { get; }
        public IRepository<Usuarios> UserRepository { get; }

        [ObservableProperty]
        private ObservableCollection<Libro> books;

        [ObservableProperty]
        private ObservableCollection<Usuarios> users;

        [ObservableProperty]
        private Libro currentBook;

        // Constructor sin parámetros
        public BookViewModel()
        {
            CurrentBook = new Libro();
        }

        // Constructor con inyección de dependencias
        public BookViewModel(IRepository<Libro> bookRepository, IRepository<Usuarios> userRepository)
        {
            BookRepository = bookRepository;
            UserRepository = userRepository;
            CurrentBook = new Libro();

            // Cargar datos asíncronamente
            _ = LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            // Carga los libros
            Books = new ObservableCollection<Libro>(await BookRepository.GetAllAsync());
            // Libros
            Books = new ObservableCollection<Libro>(await BookRepository.GetAllAsync());
            // Usuarios (para seleccionarlos en Picker)
            Users = new ObservableCollection<Usuarios>(await UserRepository.GetAllAsync());

            // Asegúrate de que cada libro tenga el Usuario cargado
            foreach (var libro in Books)
            {
                libro.Usuario = await UserRepository.GetByIdAsync(libro.UsuarioId);
            }
        }





        [RelayCommand]
        public async Task AddBookAsync()
        {
            if (CurrentBook.UsuarioId <= 0)
            {
                Console.WriteLine("El ID del usuario no es válido.");
                return;
            }

            try
            {
                await BookRepository.AddAsync(CurrentBook);
                CurrentBook = new Libro(); // Reinicia el libro actual
                await LoadDataAsync();
                Console.WriteLine("Libro agregado correctamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al agregar libro: {ex.Message}");
            }
        }


    }
}
