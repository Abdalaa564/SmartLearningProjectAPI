namespace SmartLearning.Application.GenericInterfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<T> Repository<T>() where T : class;
        int Complete();
        Task<int> CompleteAsync();

        void BeginTransaction();
        void CommitTransaction();
        void RollbackTransaction();
    }
}
