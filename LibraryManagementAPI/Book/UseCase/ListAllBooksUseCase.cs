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

        return books
            .Select(book => new BookDto(
                book.Title,
                book.Author,
                book.IsAvailable
            ))
            .ToList();
    }
}