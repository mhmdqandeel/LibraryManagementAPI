namespace LibraryManagementAPI.Book.Repository.InMemory;

public class InMemoryBookRepository : IBookRepository
{
    private readonly List<Models.Book> _books;

    public InMemoryBookRepository()
    {
        _books =
        [
            new Models.Book("Clean Code", "Robert C. Martin"),
            new Models.Book("The Pragmatic Programmer", "Andrew Hunt"),
            new Models.Book("Design Patterns", "Erich Gamma"),
            new Models.Book("Refactoring", "Martin Fowler"),
            new Models.Book("Domain-Driven Design", "Eric Evans")
        ];
    }
    
    public Task<Models.Book> SaveAsync(Models.Book entity)
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

    public Task<Models.Book?> FindAsync(Guid id)
    {
        return Task.FromResult(_books.FirstOrDefault(book => book.Id == id));
    }

    public Task<IReadOnlyList<Models.Book>> FindAllAsync()
    {
        return Task.FromResult<IReadOnlyList<Models.Book>>(_books);
    }
}