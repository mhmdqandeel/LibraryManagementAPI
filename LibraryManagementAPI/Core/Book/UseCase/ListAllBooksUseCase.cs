using LibraryManagementAPI.Book.Repository;
using LibraryManagementAPI.Book.UseCase.Response;
using LibraryManagementAPI.Shared.UseCase;

namespace LibraryManagementAPI.Book.UseCase;

public class ListAllBooksUseCase : ISupplierUseCase<List<BookDto>>
{
    private readonly IBookRepository _bookRepository;
    
    public ListAllBooksUseCase(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }
    
    public async Task<List<BookDto>> Execute()
    {
        var books = await _bookRepository.FindAllAsync();

        if (books is null)
        {
            throw new Exception("No books found");
        }

        return books
            .Select(book => new BookDto(
                book.Id,
                book.Title,
                book.Author,
                book.IsAvailable
            ))
            .ToList();
    }
}