using Microsoft.EntityFrameworkCore;
using SistemaABM_Libros_Data.Models;
using System.Linq.Expressions;

namespace SistemaABM_Libros_Data.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly BDSistemaLibrosContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(BDSistemaLibrosContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);

            if (entity == null)
            {
                return null;
            }

            return entity;
        }


        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<T?> GetByEmailAsync(string email)
        {
            if (typeof(T) == typeof(Usuario))
            {
                return await _context.Set<Usuario>()
                    .FirstOrDefaultAsync(u => u.Email == email) as T;
            }

            return null;
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filtro)
        {
            return await _dbSet.Where(filtro).ToListAsync();
        }
    }
}