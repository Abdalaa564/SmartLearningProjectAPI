namespace SmartLearning.Application.GenericInterfaces
{
    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> GetAll(params Expression<Func<T, object>>[] includes);
        T GetById(int id);
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
        void Add(T entity);
        void Update(T entity);
        void Remove(T entity);
    }
}
