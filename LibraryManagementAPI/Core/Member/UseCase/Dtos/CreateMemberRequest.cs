namespace LibraryManagementAPI.Core.Member.UseCase.Dtos;

public record CreateMemberRequest(
    string Name,
    string Email,
    int BorrowLimit);