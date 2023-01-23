namespace NewsBook.Repository
{
    public interface INewsBase<T>
    {
        IQueryable<T> FindAll();
    }
}
