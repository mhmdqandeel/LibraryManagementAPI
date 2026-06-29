using LibraryManagementAPI.Book.UseCase.Response;

namespace LibraryManagementAPI.Controller.Response;

public record ListBooksResponse(List<BookDto>  Books);