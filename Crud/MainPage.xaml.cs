using Crud.Infrastructure.Data;
using Dapper;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Crud
{
    public partial class MainPage : ContentPage
    {
        private readonly DatabaseConnection _databaseConnection;

        public MainPage(DatabaseConnection databaseConnection)
        {
            _databaseConnection = databaseConnection;

            InitializeComponent();
            VerificarUsuarios();
        }

        private async void VerificarUsuarios()
        {
            try
            {
                // Usamos la conexión con Dapper para acceder a la base de datos
                using (var connection = _databaseConnection.CreateConnection())
                {
                    var usuarios = await connection.QueryAsync<dynamic>("SELECT * FROM Usuarios");
                    var libros = await connection.QueryAsync<dynamic>("SELECT * FROM Libros");

                    Debug.WriteLine($"Usuarios encontrados: {usuarios.Count()}");
                    Debug.WriteLine($"Libros encontrados: {libros.Count()}");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error al verificar usuarios y libros: {ex.Message}");
            }
        }
    }
}
