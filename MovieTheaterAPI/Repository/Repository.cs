using Microsoft.EntityFrameworkCore;
using MovieTheaterAPI.DAL;

namespace MovieTheaterAPI.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        public readonly MovieTheaterDbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(MovieTheaterDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task Add(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task Delete(T entity)
        {
           await Task.Run(() => _dbSet.Remove(entity));
        }

        public Task<bool> IsExists(int id)
        {
            return _dbSet.FindAsync(id) != null ? Task.FromResult(true) : Task.FromResult(false);
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T> GetById(int id)
        {
           return await _dbSet.FindAsync(id);
        }

        public async Task Update(T entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }
    }
}
