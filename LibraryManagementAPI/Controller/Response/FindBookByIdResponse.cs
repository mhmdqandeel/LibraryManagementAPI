using LibraryManagementAPI.Book.UseCase.Response;

namespace LibraryManagementAPI.Controller.Response;

public record FindBookByIdResponse(BookDto Book);