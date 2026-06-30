using LibraryManagementAPI.Book.UseCase.Response;

namespace LibraryManagementAPI.Controller.Book.Response;

public record ListBooksResponse(List<BookDto>  Books);