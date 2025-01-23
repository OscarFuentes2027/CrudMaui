using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Crud.Core.Interfaces
{
    public interface IRepository<T> where T : class
    {
        // Obtiene todos los elementos
        Task<IEnumerable<T>> GetAllAsync();
        // Obtiene un elemento por su id
        Task<T> GetByIdAsync(int id);
        // Obtiene un elemento por una condicion
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        // Agrega un elemento
        Task AddAsync(T entity);
        // Actualiza un elemento
        Task UpdateAsync(T entity);
        // Elimina un elemento    
        Task DeleteAsync(T entity);

    }
}
