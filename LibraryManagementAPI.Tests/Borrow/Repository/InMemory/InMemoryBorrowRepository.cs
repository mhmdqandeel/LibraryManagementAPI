using LibraryManagementAPI.Borrow.Repository.InMemory;
using LibraryManagementAPI.Shared;
using BorrowModel = LibraryManagementAPI.Models.Borrow;

namespace LibraryManagementAPI.Tests.Borrow.Repository.InMemory;

public class InMemoryBorrowRepositoryTests
{
    private readonly InMemoryBorrowRepository _repository;

    public InMemoryBorrowRepositoryTests()
    {
        _repository = new InMemoryBorrowRepository();
    }

    [Fact]
    public async Task GivenRepositoryWithExistingBorrow_WhenSavingBorrowWithSameId_ThenBorrowIsUpdatedSuccessfully()
    {
        // Arrange
        var borrowId = Guid.NewGuid();
        var originalBorrow = CreateTestBorrow(
            id: borrowId,
            bookId: Guid.NewGuid(),
            memberId: Guid.NewGuid(),
            returnDate: DateTime.UtcNow.AddDays(7),
            isReturned: false);
        await _repository.SaveAsync(originalBorrow);

        var updatedBorrow = CreateTestBorrow(
            id: borrowId,
            bookId: Guid.NewGuid(),
            memberId: Guid.NewGuid(),
            returnDate: DateTime.UtcNow.AddDays(14),
            isReturned: true);

        // Act
        var result = await _repository.SaveAsync(updatedBorrow);

        // Assert
        result.BookId.Should().Be(updatedBorrow.BookId);
        result.MemberId.Should().Be(updatedBorrow.MemberId);
        result.ReturnDate.Should().Be(updatedBorrow.ReturnDate);
        result.IsReturned.Should().BeTrue();
        result.Id.Should().Be(borrowId);
    }

    [Fact]
    public async Task GivenRepositoryWithMultipleBorrows_WhenSavingAnotherBorrow_ThenAllBorrowsArePersisted()
    {
        // Arrange
        var borrow1 = CreateTestBorrow(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow.AddDays(3), false);
        var borrow2 = CreateTestBorrow(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow.AddDays(4), true);

        await _repository.SaveAsync(borrow1);
        await _repository.SaveAsync(borrow2);

        var borrow3 = CreateTestBorrow(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow.AddDays(5), false);

        // Act
        await _repository.SaveAsync(borrow3);

        // Assert
        var allBorrows = await _repository.FindAllAsync();
        allBorrows.Should().HaveCount(3);
        allBorrows.Should().Contain(b => b.Id == borrow1.Id);
        allBorrows.Should().Contain(b => b.Id == borrow2.Id);
        allBorrows.Should().Contain(b => b.Id == borrow3.Id);
    }

    [Fact]
    public async Task GivenRepositoryWithBorrow_WhenFindingByExistingId_ThenBorrowIsReturnedSuccessfully()
    {
        // Arrange
        var borrowId = Guid.NewGuid();
        var borrow = CreateTestBorrow(borrowId, Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow.AddDays(10), false);
        await _repository.SaveAsync(borrow);

        // Act
        var foundBorrow = await _repository.FindAsync(borrowId);

        // Assert
        foundBorrow.Should().NotBeNull();
        foundBorrow.Id.Should().Be(borrowId);
        foundBorrow.BookId.Should().Be(borrow.BookId);
        foundBorrow.MemberId.Should().Be(borrow.MemberId);
        foundBorrow.ReturnDate.Should().Be(borrow.ReturnDate);
        foundBorrow.IsReturned.Should().BeFalse();
    }

    [Fact]
    public async Task GivenRepositoryWithBorrows_WhenFindingByNonExistentId_ThenNullIsReturned()
    {
        // Arrange
        var borrow = CreateTestBorrow(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow.AddDays(8), false);
        await _repository.SaveAsync(borrow);
        var nonExistentId = Guid.NewGuid();

        // Act
        var foundBorrow = await _repository.FindAsync(nonExistentId);

        // Assert
        foundBorrow.Should().BeNull();
    }

    [Fact]
    public async Task GivenRepositoryWithMultipleBorrows_WhenFindingAll_ThenAllBorrowsAreReturned()
    {
        // Arrange
        var borrow1 = CreateTestBorrow(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow.AddDays(1), false);
        var borrow2 = CreateTestBorrow(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow.AddDays(2), true);
        var borrow3 = CreateTestBorrow(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow.AddDays(3), false);

        await _repository.SaveAsync(borrow1);
        await _repository.SaveAsync(borrow2);
        await _repository.SaveAsync(borrow3);

        // Act
        var allBorrows = await _repository.FindAllAsync();

        // Assert
        allBorrows.Should().HaveCount(3);
        allBorrows.Should().Contain(b => b.BookId == borrow1.BookId);
        allBorrows.Should().Contain(b => b.BookId == borrow2.BookId);
        allBorrows.Should().Contain(b => b.BookId == borrow3.BookId);
    }

    [Fact]
    public async Task GivenRepositoryWithBorrows_WhenUpdatingExistingBorrow_ThenFindAllReturnsUpdatedBorrow()
    {
        // Arrange
        var borrowId = Guid.NewGuid();
        var originalBorrow = CreateTestBorrow(borrowId, Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow.AddDays(5), false);
        await _repository.SaveAsync(originalBorrow);

        var updatedBorrow = CreateTestBorrow(borrowId, Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow.AddDays(15), true);

        // Act
        await _repository.SaveAsync(updatedBorrow);
        var allBorrows = await _repository.FindAllAsync();

        // Assert
        allBorrows.Should().HaveCount(1);
        allBorrows[0].BookId.Should().Be(updatedBorrow.BookId);
        allBorrows[0].MemberId.Should().Be(updatedBorrow.MemberId);
        allBorrows[0].ReturnDate.Should().Be(updatedBorrow.ReturnDate);
        allBorrows[0].IsReturned.Should().BeTrue();
    }

    /// <summary>
    /// Helper method to create a test Borrow with the given parameters.
    /// Uses reflection to invoke the protected constructor and set private properties.
    /// </summary>
    private static BorrowModel CreateTestBorrow(Guid id, Guid bookId, Guid memberId, DateTime returnDate, bool isReturned)
    {
        var borrow = (BorrowModel)Activator.CreateInstance(typeof(BorrowModel), nonPublic: true)!;

        typeof(BaseEntity).GetProperty("Id")?.SetValue(borrow, id);
        typeof(BaseEntity).GetProperty("CreatedAt")?.SetValue(borrow, DateTime.UtcNow);

        typeof(BorrowModel).GetProperty("BookId")?.SetValue(borrow, bookId);
        typeof(BorrowModel).GetProperty("MemberId")?.SetValue(borrow, memberId);
        typeof(BorrowModel).GetProperty("ReturnDate")?.SetValue(borrow, returnDate);
        typeof(BorrowModel).GetProperty("IsReturned")?.SetValue(borrow, isReturned);

        return borrow;
    }
}