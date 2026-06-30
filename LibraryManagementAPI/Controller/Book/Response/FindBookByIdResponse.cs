using LibraryManagementAPI.Book.UseCase.Response;

namespace LibraryManagementAPI.Controller.Book.Response;

public record FindBookByIdResponse(BookDto Book);