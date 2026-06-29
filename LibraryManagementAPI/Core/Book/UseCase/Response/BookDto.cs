namespace LibraryManagementAPI.Book.UseCase.Response;

public record BookDto (
    string Title,
    string Author,
    bool IsAvailable);