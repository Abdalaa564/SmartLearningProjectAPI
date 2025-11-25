
using Microsoft.EntityFrameworkCore;

namespace SmartLearning.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly ITIEntity _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(ITIEntity context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<IReadOnlyList<T>> GetAllAsync(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet.AsNoTracking();
            if (includes != null)
            {
                foreach (var include in includes)
                    query = query.Include(include);
            }
            return await query.ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<IReadOnlyList<T>> FindAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet.Where(predicate);
            if (includes != null)
            {
                foreach (var include in includes)
                    query = query.Include(include);
            }
            return await query.ToListAsync();
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }

        public async Task<IReadOnlyList<T>> GetAllAsync(Func<IQueryable<T>, IQueryable<T>> includeFunc)
        {
            IQueryable<T> query = _dbSet.AsNoTracking();
            query = includeFunc(query);
            return await query.ToListAsync();
        }

        public async Task<IReadOnlyList<T>> FindAsync(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IQueryable<T>> includeFunc = null)
        {
            IQueryable<T> query = _dbSet.Where(predicate);

            if (includeFunc != null)
                query = includeFunc(query);
            return await query.ToListAsync();
        }
    }
}


// اضافه بعد المراجعه مع الشباب 

//public async Task<T?> GetSingleAsync(
//    Expression<Func<T, bool>> predicate,
//    params Expression<Func<T, object>>[] includes)
//{
//    IQueryable<T> query = _dbSet.AsNoTracking().Where(predicate);

//    if (includes != null)
//    {
//        foreach (var include in includes)
//            query = query.Include(include);
//    }

//    return await query.FirstOrDefaultAsync();
//}

//public async Task<T?> GetSingleAsync(
//    Expression<Func<T, bool>> predicate,
//    Func<IQueryable<T>, IQueryable<T>>? includeFunc = null)
//{
//    IQueryable<T> query = _dbSet.AsNoTracking().Where(predicate);

//    if (includeFunc != null)
//        query = includeFunc(query);

//    return await query.FirstOrDefaultAsync();
//}