namespace LibraryManagementAPI.Core.Member.UseCase.Dtos;

public record MemberDto(
    Guid Id,
    string Name,
    string Email,
    int BorrowLimit);