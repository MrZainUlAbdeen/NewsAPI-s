namespace NewsBook.Repository
{
    public interface IBaseRepository<T>
    {
        IQueryable<T> FindAll();
    }
}
