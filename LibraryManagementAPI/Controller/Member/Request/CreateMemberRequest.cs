namespace LibraryManagementAPI.Controller.Member.Request;

public record CreateMemberRequest(
    string Name,
    string Email,
    int BorrowLimit);