using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Crud.Core.Entities;
using Crud.Infrastructure.Data;
using Dapper;
using System.Data;

namespace Crud.Infrastructure.Repositories
{
    public class BookRepository
    {
        private readonly DatabaseConnection _databaseConnection;

        public BookRepository(DatabaseConnection databaseConnection)
        {
            _databaseConnection = databaseConnection;
        }

        public async Task<IEnumerable<Libro>> GetAllAsync()
        {
            using (var connection = _databaseConnection.CreateConnection())
            {
                string sql = "SELECT * FROM Libros";
                return await connection.QueryAsync<Libro>(sql);
            }
        }

        public async Task AddAsync(Libro libro)
        {
            using (var connection = _databaseConnection.CreateConnection())
            {
                string sql = "INSERT INTO Libros (Titulo, Genero, FechaPublicacion, UsuarioId) VALUES (@Titulo, @Genero, @FechaPublicacion, @UsuarioId)";
                await connection.ExecuteAsync(sql, libro);
            }
        }

        public async Task<Libro> GetByIdAsync(int id)
        {
            using (var connection = _databaseConnection.CreateConnection())
            {
                string sql = "SELECT * FROM Libros WHERE Id = @Id";
                return await connection.QueryFirstOrDefaultAsync<Libro>(sql, new { Id = id });
            }
        }


        public async Task DeleteAsync(Libro libro)
        {
            using (IDbConnection connection = _databaseConnection.CreateConnection())
            {
                const string query = "DELETE FROM Libros WHERE Id = @Id";
                await connection.ExecuteAsync(query, libro);
            }
        }
        public async Task UpdateAsync(Libro libro)
        {
            using (var connection = _databaseConnection.CreateConnection())
            {
                string sql = @"
                    UPDATE Libros
                    SET Titulo = @Titulo,
                        Genero = @Genero,
                        FechaPublicacion = @FechaPublicacion,
                        UsuarioId = @UsuarioId
                    WHERE Id = @Id";
                await connection.ExecuteAsync(sql, libro);
            }
        }
    }
}
