namespace NewsBook.Core
{
    public interface IBaseResponse<T>
    {
        IQueryable<T> FindAll();
    }
}
