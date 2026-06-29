namespace LibraryManagementAPI.Borrow.Repository.InMemory;

public class InMemoryBorrowRepository : IBorrowRepository
{
    private readonly List<Models.Borrow> _borrows = [];

    public Task<Models.Borrow> SaveAsync(Models.Borrow entity)
    {
        var existingBorrow = _borrows.FirstOrDefault(borrow => borrow.Id == entity.Id);

        if (existingBorrow is null)
        {
            _borrows.Add(entity);
        }
        else
        {
            var index = _borrows.IndexOf(existingBorrow);
            _borrows[index] = entity;
        }

        return Task.FromResult(entity);
    }

    public Task<Models.Borrow?> FindAsync(Guid id)
    {
        return Task.FromResult(_borrows.FirstOrDefault(borrow => borrow.Id == id));
    }

    public Task<IReadOnlyList<Models.Borrow>> FindAllAsync()
    {
        return Task.FromResult<IReadOnlyList<Models.Borrow>>(_borrows);
    }
}