using LibraryManagementAPI.Book.UseCase.Response;
using LibraryManagementAPI.Controller.Response;
using LibraryManagementAPI.Shared.UseCase;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementAPI.Controller;

[ApiController]
[Route("api/v1/books")]
public class BookController : Microsoft.AspNetCore.Mvc.Controller
{
    private readonly ISupplierUseCase<List<BookDto>> _listAllBooksUseCase;
    private readonly IFunctionalUseCase<Guid, BookDto> _findBookByIdUseCase;

    public BookController(ISupplierUseCase<List<BookDto>> listAllBooksUseCase,
        IFunctionalUseCase<Guid, BookDto> findBookByIdUseCase)
    {
        _listAllBooksUseCase = listAllBooksUseCase;
        _findBookByIdUseCase = findBookByIdUseCase;
    }

    [HttpGet]
    [ProducesResponseType(typeof(ListBooksResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<ListBooksResponse>> GetAll()
    {
        var books = await _listAllBooksUseCase.Execute();

        var listBooksResponse = new ListBooksResponse(books);

        return Ok(listBooksResponse);
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(BookDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<BookDto>> GetById(Guid id)
    {
        var book = await _findBookByIdUseCase.Execute(id);

        var response = new FindBookByIdResponse(book);

        return Ok(response);
    }
}