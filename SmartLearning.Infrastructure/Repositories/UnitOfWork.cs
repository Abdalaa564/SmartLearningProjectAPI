
namespace SmartLearning.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ITIEntity _context;
        private readonly ConcurrentDictionary<string, object> _repositories;
        private IDbContextTransaction _transaction;

        public UnitOfWork(ITIEntity context)
        {
            _context = context;
            _repositories = new ConcurrentDictionary<string, object>();
        }

        public IGenericRepository<T> Repository<T>() where T : class
        {
            var typeName = typeof(T).Name;

            var repository = _repositories.GetOrAdd(typeName, (t) =>
            {
                var repoType = typeof(GenericRepository<>).MakeGenericType(typeof(T));
                return Activator.CreateInstance(repoType, _context)!;
            });

            return (IGenericRepository<T>)repository;
        }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
        }

        public void BeginTransaction()
        {
            if (_transaction != null) throw new InvalidOperationException("Transaction already started.");
            _transaction = _context.Database.BeginTransaction();
        }

        public void CommitTransaction()
        {
            if (_transaction == null) throw new InvalidOperationException("No transaction started.");
            _transaction.Commit();
            _transaction.Dispose();
            _transaction = null;
        }

        public void RollbackTransaction()
        {
            if (_transaction == null) throw new InvalidOperationException("No transaction started.");
            _transaction.Rollback();
            _transaction.Dispose();
            _transaction = null;
        }
    }
}
