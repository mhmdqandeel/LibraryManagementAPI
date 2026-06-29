namespace LibraryManagementAPI.Shared;

public interface IBaseRepository <T>
{
    Task<T> SaveAsync(T entity);
    Task<T?> FindAsync(Guid id);
    Task<IReadOnlyList<T>> FindAllAsync();
}