using LibraryManagementAPI.Book.UseCase.Response;
using LibraryManagementAPI.Shared.UseCase;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementAPI.Controller;

[ApiController]
[Route("api/v1/books")]
public class BookController : Microsoft.AspNetCore.Mvc.Controller
{
    private readonly ISupplierUseCase<List<BookDto>> _listAllBooksUseCase;

    public BookController(ISupplierUseCase<List<BookDto>> listAllBooksUseCase)
    {
        _listAllBooksUseCase = listAllBooksUseCase;
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<BookDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<BookDto>>> GetAll()
    {
        var books = await _listAllBooksUseCase.Execute();

        return Ok(books);
    }
}