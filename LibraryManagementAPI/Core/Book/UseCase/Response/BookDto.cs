namespace LibraryManagementAPI.Book.UseCase.Response;

public record BookDto (
    Guid Id,
    string Title,
    string Author,
    bool IsAvailable);