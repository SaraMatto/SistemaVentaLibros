using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SistemaABM_Libros_Data.Repository
{
    public interface IGenericRepository<T> where T : class
    {
     
            Task<T> GetByIdAsync(int id);
            Task<IEnumerable<T>> GetAllAsync();
            Task AddAsync(T entity);
            Task UpdateAsync(T entity);
            Task DeleteAsync(int id);


    }
}
