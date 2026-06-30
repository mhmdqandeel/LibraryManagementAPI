using LibraryManagementAPI.Book.UseCase.Response;
using LibraryManagementAPI.Core.Book.Repository;
using LibraryManagementAPI.Shared.UseCase;

namespace LibraryManagementAPI.Core.Book.UseCase;

public class FindBookByIdUseCase : IFunctionalUseCase<Guid, BookDto>
{
    private readonly IBookRepository _bookRepository;

    public FindBookByIdUseCase(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task<BookDto> Execute(Guid request)
    {
        var book = await _bookRepository.FindAsync(request);
        
        return book is null ? throw new Exception("Book not found") : new BookDto(book.Id,book.Title, book.Author, book.IsAvailable);
    }
}