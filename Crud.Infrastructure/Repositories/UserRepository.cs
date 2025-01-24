using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Crud.Core.Entities;
using Crud.Infrastructure.Data;
using Dapper;

namespace Crud.Infrastructure.Repositories
{
    public class UserRepository
    {
        private readonly DatabaseConnection _databaseConnection;

        public UserRepository(DatabaseConnection databaseConnection)
        {
            _databaseConnection = databaseConnection;
        }

        public async Task<IEnumerable<Usuarios>> GetAllAsync()
        {
            using (IDbConnection connection = _databaseConnection.CreateConnection())
            {
                const string query = "SELECT * FROM Usuarios";
                return await connection.QueryAsync<Usuarios>(query);
            }
        }

        private static readonly object _dbLock = new object();

        public async Task AddAsync(Usuarios usuario)
        {
            using (var connection = _databaseConnection.CreateConnection())
            {
                string sql = "INSERT INTO Usuarios (Name, Email, Password) VALUES (@Name, @Email, @Password)";
                await connection.ExecuteAsync(sql, usuario); // Usa await para evitar bloquear
            }
        }





        public async Task UpdateAsync(Usuarios user)
        {
            using (IDbConnection connection = _databaseConnection.CreateConnection())
            {
                const string query = "UPDATE Usuarios SET Name = @Name, Email = @Email, Password = @Password WHERE Id = @Id";
                await connection.ExecuteAsync(query, user);
            }
        }

        public async Task DeleteAsync(Usuarios user)
        {
            using (IDbConnection connection = _databaseConnection.CreateConnection())
            {
                const string query = "DELETE FROM Usuarios WHERE Id = @Id";
                await connection.ExecuteAsync(query, user);
            }
        }
    }
}
