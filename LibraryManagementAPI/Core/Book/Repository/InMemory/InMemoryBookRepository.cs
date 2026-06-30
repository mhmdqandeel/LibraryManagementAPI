namespace LibraryManagementAPI.Core.Book.Repository.InMemory;

public class InMemoryBookRepository : IBookRepository
{
    private readonly List<Core.Book.Entity.Book> _books;

    public InMemoryBookRepository()
    {
        _books =
        [
            new Core.Book.Entity.Book("Clean Code", "Robert C. Martin"),
            new Core.Book.Entity.Book("The Pragmatic Programmer", "Andrew Hunt"),
            new Core.Book.Entity.Book("Design Patterns", "Erich Gamma"),
            new Core.Book.Entity.Book("Refactoring", "Martin Fowler"),
            new Core.Book.Entity.Book("Domain-Driven Design", "Eric Evans")
        ];
    }
    
    public Task<Core.Book.Entity.Book> SaveAsync(Core.Book.Entity.Book entity)
    {
        var existingBook = _books.FirstOrDefault(book => book.Id == entity.Id); 

        if (existingBook is null)
        {
            _books.Add(entity);
        }
        else
        {
            var index = _books.IndexOf(existingBook);
            _books[index] = entity;
        }

        return Task.FromResult(entity);
    }

    public Task<Core.Book.Entity.Book?> FindAsync(Guid id)
    {
        return Task.FromResult(_books.FirstOrDefault(book => book.Id == id));
    }

    public Task<IReadOnlyList<Core.Book.Entity.Book>> FindAllAsync()
    {
        return Task.FromResult<IReadOnlyList<Core.Book.Entity.Book>>(_books);
    }
}