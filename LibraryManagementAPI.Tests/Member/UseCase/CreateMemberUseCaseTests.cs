using LibraryManagementAPI.Controller.Member.Request;
using LibraryManagementAPI.Core.Member.Repository;
using LibraryManagementAPI.Core.Member.UseCase;
using Moq;

namespace LibraryManagementAPI.Tests.Member.UseCase;

public class CreateMemberUseCaseTests
{
    private readonly Mock<IMemberRepository> _memberRepositoryMock;
    private readonly CreateMemberUseCase _useCase;

    public CreateMemberUseCaseTests()
    {
        _memberRepositoryMock = new Mock<IMemberRepository>();
        _useCase = new CreateMemberUseCase(_memberRepositoryMock.Object);
    }

    [Fact]
    public async Task GivenValidCreateMemberRequest_WhenExecutingUseCase_ThenCreateAndReturnMemberDto()
    {
        // Arrange
        var request = new CreateMemberRequest(
            "Mohammed",
            "mohammed@example.com",
            5);
        
        Core.Member.Entity.Member? savedMember = null;

        _memberRepositoryMock
            .Setup(repository => repository.SaveAsync(It.IsAny<Core.Member.Entity.Member>()))
            .Callback<Core.Member.Entity.Member>(member => savedMember = member)
            .ReturnsAsync((Core.Member.Entity.Member member) => member);

        // Act
        var result = await _useCase.Execute(request);

        // Assert
        Assert.NotNull(savedMember);

        Assert.Equal(request.Name, savedMember!.Name);
        Assert.Equal(request.Email, savedMember.Email);
        Assert.Equal(request.BorrowLimit, savedMember.BorrowLimit);

        Assert.Equal(savedMember.Id, result.Id);
        Assert.Equal(savedMember.Name, result.Name);
        Assert.Equal(savedMember.Email, result.Email);
        Assert.Equal(savedMember.BorrowLimit, result.BorrowLimit);

        _memberRepositoryMock.Verify(
            repository => repository.SaveAsync(It.IsAny<Core.Member.Entity.Member>()),
            Times.Once);

        _memberRepositoryMock.VerifyNoOtherCalls();
    }
}