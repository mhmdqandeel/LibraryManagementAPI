using LibraryManagementAPI.Controller.Member.Request;
using LibraryManagementAPI.Core.Member.Repository;
using LibraryManagementAPI.Core.Member.UseCase.Dtos;
using LibraryManagementAPI.Shared.UseCase;

namespace LibraryManagementAPI.Core.Member.UseCase;

public class CreateMemberUseCase : IFunctionalUseCase<CreateMemberRequest, MemberDto>
{
    private readonly IMemberRepository _memberRepository;

    public CreateMemberUseCase(IMemberRepository memberRepository)
    {
        _memberRepository = memberRepository;
    }

    public async Task<MemberDto> Execute(CreateMemberRequest request)
    {
        var member = new Entity.Member(
            request.Name,
            request.Email,
            request.BorrowLimit);

        await _memberRepository.SaveAsync(member);

        return new MemberDto(
            member.Id,
            member.Name,
            member.Email,
            member.BorrowLimit);
    }
}