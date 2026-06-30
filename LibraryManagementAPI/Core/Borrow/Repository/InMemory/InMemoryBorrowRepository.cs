namespace LibraryManagementAPI.Core.Borrow.Repository.InMemory;

public class InMemoryBorrowRepository : IBorrowRepository
{
    private readonly List<Core.Borrow.Entity.Borrow> _borrows = [];

    public Task<Core.Borrow.Entity.Borrow> SaveAsync(Core.Borrow.Entity.Borrow entity)
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

    public Task<Core.Borrow.Entity.Borrow?> FindAsync(Guid id)
    {
        return Task.FromResult(_borrows.FirstOrDefault(borrow => borrow.Id == id));
    }

    public Task<IReadOnlyList<Core.Borrow.Entity.Borrow>> FindAllAsync()
    {
        return Task.FromResult<IReadOnlyList<Core.Borrow.Entity.Borrow>>(_borrows);
    }
}