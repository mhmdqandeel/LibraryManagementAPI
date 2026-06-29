using LibraryManagementAPI.Member.Repository.InMemory;
using LibraryManagementAPI.Shared;
using MemberModel = LibraryManagementAPI.Models.Member;

namespace LibraryManagementAPI.Tests.Member.Repository.InMemory;

public class InMemoryMemberRepositoryTests
{
    private readonly InMemoryMemberRepository _repository;

    public InMemoryMemberRepositoryTests()
    {
        _repository = new InMemoryMemberRepository();
    }

    [Fact]
    public async Task GivenRepositoryWithExistingMember_WhenSavingMemberWithSameId_ThenMemberIsUpdatedSuccessfully()
    {
        // Arrange
        var memberId = Guid.NewGuid();
        var originalMember = CreateTestMember(id: memberId, name: "Original Name", email: "original@test.com", borrowLimit: 5);
        await _repository.SaveAsync(originalMember);

        var updatedMember = CreateTestMember(id: memberId, name: "Updated Name", email: "updated@test.com", borrowLimit: 10);

        // Act
        var result = await _repository.SaveAsync(updatedMember);

        // Assert
        result.Name.Should().Be("Updated Name");
        result.Email.Should().Be("updated@test.com");
        result.BorrowLimit.Should().Be(10);
        result.Id.Should().Be(memberId);
    }

    [Fact]
    public async Task GivenRepositoryWithMultipleMembers_WhenSavingAnotherMember_ThenAllMembersArePersisted()
    {
        // Arrange
        var member1 = CreateTestMember(id: Guid.NewGuid(), name: "Member 1", email: "member1@test.com", borrowLimit: 5);
        var member2 = CreateTestMember(id: Guid.NewGuid(), name: "Member 2", email: "member2@test.com", borrowLimit: 3);
        var member3 = CreateTestMember(id: Guid.NewGuid(), name: "Member 3", email: "member3@test.com", borrowLimit: 7);

        await _repository.SaveAsync(member1);
        await _repository.SaveAsync(member2);

        // Act
        await _repository.SaveAsync(member3);

        // Assert
        var allMembers = await _repository.FindAllAsync();
        allMembers.Should().HaveCount(3);
        allMembers.Should().Contain(m => m.Id == member1.Id);
        allMembers.Should().Contain(m => m.Id == member2.Id);
        allMembers.Should().Contain(m => m.Id == member3.Id);
    }

    [Fact]
    public async Task GivenRepositoryWithMember_WhenFindingByExistingId_ThenMemberIsReturnedSuccessfully()
    {
        // Arrange
        var memberId = Guid.NewGuid();
        var member = CreateTestMember(id: memberId, name: "Test Member", email: "test@test.com", borrowLimit: 5);
        await _repository.SaveAsync(member);

        // Act
        var foundMember = await _repository.FindAsync(memberId);

        // Assert
        foundMember.Should().NotBeNull();
        foundMember.Id.Should().Be(memberId);
        foundMember.Name.Should().Be("Test Member");
        foundMember.Email.Should().Be("test@test.com");
        foundMember.BorrowLimit.Should().Be(5);
    }

    [Fact]
    public async Task GivenRepositoryWithMembers_WhenFindingByNonExistentId_ThenNullIsReturned()
    {
        // Arrange
        var member = CreateTestMember(id: Guid.NewGuid(), name: "Existing Member", email: "existing@test.com", borrowLimit: 5);
        await _repository.SaveAsync(member);
        var nonExistentId = Guid.NewGuid();

        // Act
        var foundMember = await _repository.FindAsync(nonExistentId);

        // Assert
        foundMember.Should().BeNull();
    }

    [Fact]
    public async Task GivenRepositoryWithMultipleMembers_WhenFindingAll_ThenAllMembersAreReturned()
    {
        // Arrange
        var member1 = CreateTestMember(id: Guid.NewGuid(), name: "Member 1", email: "member1@test.com", borrowLimit: 5);
        var member2 = CreateTestMember(id: Guid.NewGuid(), name: "Member 2", email: "member2@test.com", borrowLimit: 3);
        var member3 = CreateTestMember(id: Guid.NewGuid(), name: "Member 3", email: "member3@test.com", borrowLimit: 7);

        await _repository.SaveAsync(member1);
        await _repository.SaveAsync(member2);
        await _repository.SaveAsync(member3);

        // Act
        var allMembers = await _repository.FindAllAsync();

        // Assert
        allMembers.Should().HaveCount(3);
        allMembers.Should().Contain(m => m.Name == "Member 1");
        allMembers.Should().Contain(m => m.Name == "Member 2");
        allMembers.Should().Contain(m => m.Name == "Member 3");
    }

    [Fact]
    public async Task GivenRepositoryWithMembers_WhenUpdatingExistingMember_ThenFindAllReturnsUpdatedMember()
    {
        // Arrange
        var memberId = Guid.NewGuid();
        var originalMember = CreateTestMember(id: memberId, name: "Original Name", email: "original@test.com", borrowLimit: 5);
        await _repository.SaveAsync(originalMember);

        var updatedMember = CreateTestMember(id: memberId, name: "Updated Name", email: "updated@test.com", borrowLimit: 10);

        // Act
        await _repository.SaveAsync(updatedMember);
        var allMembers = await _repository.FindAllAsync();

        // Assert
        allMembers.Should().HaveCount(1);
        allMembers[0].Name.Should().Be("Updated Name");
        allMembers[0].Email.Should().Be("updated@test.com");
        allMembers[0].BorrowLimit.Should().Be(10);
    }

    /// <summary>
    /// Helper method to create a test Member with the given parameters.
    /// Uses reflection to invoke the protected constructor and set private properties.
    /// </summary>
    private static MemberModel CreateTestMember(Guid id, string name, string email, int borrowLimit)
    {
        var member = (MemberModel)Activator.CreateInstance(typeof(MemberModel), nonPublic: true)!;

        typeof(BaseEntity).GetProperty("Id")?.SetValue(member, id);
        typeof(BaseEntity).GetProperty("CreatedAt")?.SetValue(member, DateTime.UtcNow);

        typeof(MemberModel).GetProperty("Name")?.SetValue(member, name);
        typeof(MemberModel).GetProperty("Email")?.SetValue(member, email);
        typeof(MemberModel).GetProperty("BorrowLimit")?.SetValue(member, borrowLimit);

        return member;
    }
}