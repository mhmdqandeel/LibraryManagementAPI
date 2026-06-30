using LibraryManagementAPI.Book.UseCase;
using LibraryManagementAPI.Core.Book.Repository;
using LibraryManagementAPI.Core.Book.UseCase;
using Moq;
using Xunit;

namespace LibraryManagementAPI.Tests.Book.UseCase;

public class ListAllBooksUseCaseTests
{
    private readonly Mock<IBookRepository> _bookRepositoryMock;
    private readonly ListAllBooksUseCase _useCase;

    public ListAllBooksUseCaseTests()
    {
        _bookRepositoryMock = new Mock<IBookRepository>();
        _useCase = new ListAllBooksUseCase(_bookRepositoryMock.Object);
    }

    [Fact]
    public async Task GivenExistingBooks_WhenExecutingUseCase_ThenReturnBookDtos()
    {
        // Arrange
        var books = new List<Core.Book.Entity.Book>
        {
            new("Clean Code", "Robert C. Martin"),
            new("The Pragmatic Programmer", "Andrew Hunt")
        };

        _bookRepositoryMock
            .Setup(repository => repository.FindAllAsync())
            .ReturnsAsync(books);

        // Act
        var result = await _useCase.Execute();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);

        Assert.Equal(books[0].Id, result[0].Id);
        Assert.Equal(books[0].Title, result[0].Title);
        Assert.Equal(books[0].Author, result[0].Author);
        Assert.Equal(books[0].IsAvailable, result[0].IsAvailable);

        Assert.Equal(books[1].Id, result[1].Id);
        Assert.Equal(books[1].Title, result[1].Title);
        Assert.Equal(books[1].Author, result[1].Author);
        Assert.Equal(books[1].IsAvailable, result[1].IsAvailable);

        _bookRepositoryMock.Verify(
            repository => repository.FindAllAsync(),
            Times.Once);

        _bookRepositoryMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task GivenNoBooks_WhenExecutingUseCase_ThenThrowException()
    {
        // Arrange
        _bookRepositoryMock
            .Setup(repository => repository.FindAllAsync())
            .ReturnsAsync((List<Core.Book.Entity.Book>?)null);

        // Act
        var exception = await Assert.ThrowsAsync<Exception>(() => _useCase.Execute());

        // Assert
        Assert.Equal("No books found", exception.Message);

        _bookRepositoryMock.Verify(
            repository => repository.FindAllAsync(),
            Times.Once);

        _bookRepositoryMock.VerifyNoOtherCalls();
    }
}