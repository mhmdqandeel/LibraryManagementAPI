using LibraryManagementAPI.Controller.Member.Request;
using LibraryManagementAPI.Controller.Member.Response;
using LibraryManagementAPI.Core.Member.UseCase.Dtos;
using LibraryManagementAPI.Shared.UseCase;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementAPI.Controller.Member;

[ApiController]
[Route("api/v1/members")]
public class MemberController : Microsoft.AspNetCore.Mvc.Controller
{
    private readonly IFunctionalUseCase<CreateMemberRequest, MemberDto> _createMemberUseCase;

    public MemberController(IFunctionalUseCase<CreateMemberRequest, MemberDto> createMemberUseCase)
    {
        _createMemberUseCase = createMemberUseCase;
    }

    [HttpPost]
    [ProducesResponseType(typeof(MemberDto), StatusCodes.Status201Created)]
    public async Task<ActionResult<MemberDto>> CreateMember([FromBody] CreateMemberRequest request)
    {
        var memberDto = await _createMemberUseCase.Execute(request);

        var createMemberResponse = new CreateMemberResponse(memberDto);

        return StatusCode(StatusCodes.Status201Created, createMemberResponse);
    }
}