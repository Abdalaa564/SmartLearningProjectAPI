
using SmartLearning.Application.GenericInterfaces;

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

        public IQueryable<T> GetAll(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet.AsNoTracking();
            if (includes != null)
            {
                foreach (var include in includes)
                    query = query.Include(include);
            }
            return query;
        }

        public T GetById(int id)
        {
            return _dbSet.Find(id);
        }

        public IEnumerable<T> Find(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet.Where(predicate);
            if (includes != null)
            {
                foreach (var include in includes)
                    query = query.Include(include);
            }
            return query.ToList();
        }

        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }
    }
}
