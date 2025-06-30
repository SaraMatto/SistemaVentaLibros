using System.Linq.Expressions;

namespace SistemaABM_Libros_Data.Repository
{
    public interface IGenericRepository<T> where T : class
    {
     
            Task<T> GetByIdAsync(int id);
            Task<IEnumerable<T>> GetAllAsync();
            Task AddAsync(T entity);
            Task UpdateAsync(T entity);
            Task DeleteAsync(int id);
            Task<T?> GetByEmailAsync(string email);
            Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filtro);
            Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filtro, params Expression<Func<T, object>>[] includes);
    }
}
