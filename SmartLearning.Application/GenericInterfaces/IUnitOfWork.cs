namespace SmartLearning.Application.GenericInterfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<T> Repository<T>() where T : class;
        int Complete();
        void BeginTransaction();
        void CommitTransaction();
        void RollbackTransaction();
    }
}
