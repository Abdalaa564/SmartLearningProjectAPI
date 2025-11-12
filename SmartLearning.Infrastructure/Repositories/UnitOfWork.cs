


namespace SmartLearning.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ITIEntity _context;
        private Hashtable _repositories;
        private IDbContextTransaction _transaction;

        public UnitOfWork(ITIEntity context)
        {
            _context = context;
        }

        public IGenericRepository<T> Repository<T>() where T : class
        {
            if (_repositories == null)
                _repositories = new Hashtable();

            var type = typeof(T).Name;

            if (!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(GenericRepository<>);
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(T)), _context);
                _repositories.Add(type, repositoryInstance);
            }

            return (IGenericRepository<T>)_repositories[type];
        }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
        }

        public void BeginTransaction()
        {
            _transaction = _context.Database.BeginTransaction();
        }

        public void CommitTransaction()
        {
            _transaction?.Commit();
            _transaction?.Dispose();
        }

        public void RollbackTransaction()
        {
            _transaction?.Rollback();
            _transaction?.Dispose();
        }
    }
}
