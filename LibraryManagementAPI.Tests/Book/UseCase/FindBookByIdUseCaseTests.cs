using LibraryManagementAPI.Book.UseCase;
using LibraryManagementAPI.Core.Book.Repository;
using LibraryManagementAPI.Core.Book.UseCase;
using Moq;
using Xunit;

namespace LibraryManagementAPI.Tests.Book.UseCase;

public class FindBookByIdUseCaseTests
{
    private readonly Mock<IBookRepository> _bookRepositoryMock;
    private readonly FindBookByIdUseCase _useCase;

    public FindBookByIdUseCaseTests()
    {
        _bookRepositoryMock = new Mock<IBookRepository>();
        _useCase = new FindBookByIdUseCase(_bookRepositoryMock.Object);
    }

    [Fact]
    public async Task GivenExistingBookId_WhenExecutingUseCase_ThenReturnBookDto()
    {
        // Arrange
        var book = new Core.Book.Entity.Book("Clean Code", "Robert C. Martin");

        _bookRepositoryMock
            .Setup(repository => repository.FindAsync(book.Id))
            .ReturnsAsync(book);

        // Act
        var result = await _useCase.Execute(book.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(book.Id, result.Id);
        Assert.Equal(book.Title, result.Title);
        Assert.Equal(book.Author, result.Author);
        Assert.Equal(book.IsAvailable, result.IsAvailable);

        _bookRepositoryMock.Verify(
            repository => repository.FindAsync(book.Id),
            Times.Once);

        _bookRepositoryMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task GivenNonExistingBookId_WhenExecutingUseCase_ThenThrowException()
    {
        // Arrange
        var bookId = Guid.NewGuid();

        _bookRepositoryMock
            .Setup(repository => repository.FindAsync(bookId))
            .ReturnsAsync((Core.Book.Entity.Book?)null);

        // Act
        var exception = await Assert.ThrowsAsync<Exception>(
            () => _useCase.Execute(bookId));

        // Assert
        Assert.Equal("Book not found", exception.Message);

        _bookRepositoryMock.Verify(
            repository => repository.FindAsync(bookId),
            Times.Once);

        _bookRepositoryMock.VerifyNoOtherCalls();
    }
}