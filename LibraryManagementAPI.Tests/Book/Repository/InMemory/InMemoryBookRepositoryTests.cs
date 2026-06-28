using LibraryManagementAPI.Book.Repository.InMemory;
using LibraryManagementAPI.Shared;
using BookModel = LibraryManagementAPI.Models.Book;

namespace LibraryManagementAPI.Tests.Book.Repository.InMemory;

public class InMemoryBookRepositoryTests
{
    private readonly InMemoryBookRepository _repository;

    public InMemoryBookRepositoryTests()
    {
        _repository = new InMemoryBookRepository();
    }

    [Fact]
    public async Task GivenRepositoryWithExistingBook_WhenSavingBookWithSameId_ThenBookIsUpdatedSuccessfully()
    {
        // Arrange
        var bookId = Guid.NewGuid();
        var originalBook = CreateTestBook(id: bookId, title: "Original Title", author: "Original Author");
        await _repository.SaveAsync(originalBook);

        var updatedBook = CreateTestBook(id: bookId, title: "Updated Title", author: "Updated Author");

        // Act
        var result = await _repository.SaveAsync(updatedBook);

        // Assert
        result.Title.Should().Be("Updated Title");
        result.Author.Should().Be("Updated Author");
        result.Id.Should().Be(bookId.ToString());
    }

    [Fact]
    public async Task GivenRepositoryWithMultipleBooks_WhenSavingAnotherBook_ThenAllBooksArePersisted()
    {
        // Arrange
        var book1 = CreateTestBook(id: Guid.NewGuid(), title: "Book 1", author: "Author 1");
        var book2 = CreateTestBook(id: Guid.NewGuid(), title: "Book 2", author: "Author 2");
        var book3 = CreateTestBook(id: Guid.NewGuid(), title: "Book 3", author: "Author 3");

        await _repository.SaveAsync(book1);
        await _repository.SaveAsync(book2);

        // Act
        await _repository.SaveAsync(book3);

        // Assert
        var allBooks = await _repository.FindAllAsync();
        allBooks.Should().HaveCount(3);
        allBooks.Should().Contain(b => b.Id == book1.Id);
        allBooks.Should().Contain(b => b.Id == book2.Id);
        allBooks.Should().Contain(b => b.Id == book3.Id);
    }

    [Fact]
    public async Task GivenRepositoryWithBook_WhenFindingByExistingId_ThenBookIsReturnedSuccessfully()
    {
        // Arrange
        var bookId = Guid.NewGuid();
        var book = CreateTestBook(id: bookId, title: "Test Book", author: "Test Author");
        await _repository.SaveAsync(book);

        // Act
        var foundBook = await _repository.FindAsync(bookId);

        // Assert
        foundBook.Should().NotBeNull();
        foundBook.Id.Should().Be(bookId.ToString());
        foundBook.Title.Should().Be("Test Book");
        foundBook.Author.Should().Be("Test Author");
    }

    [Fact]
    public async Task GivenRepositoryWithBooks_WhenFindingByNonExistentId_ThenNullIsReturned()
    {
        // Arrange
        var book = CreateTestBook(id: Guid.NewGuid(), title: "Existing Book", author: "Test Author");
        await _repository.SaveAsync(book);
        var nonExistentId = Guid.NewGuid();

        // Act
        var foundBook = await _repository.FindAsync(nonExistentId);

        // Assert
        foundBook.Should().BeNull();
    }

    [Fact]
    public async Task GivenRepositoryWithMultipleBooks_WhenFindingAll_ThenAllBooksAreReturned()
    {
        // Arrange
        var book1 = CreateTestBook(id: Guid.NewGuid(), title: "Book 1", author: "Author 1");
        var book2 = CreateTestBook(id: Guid.NewGuid(), title: "Book 2", author: "Author 2");
        var book3 = CreateTestBook(id: Guid.NewGuid(), title: "Book 3", author: "Author 3");

        await _repository.SaveAsync(book1);
        await _repository.SaveAsync(book2);
        await _repository.SaveAsync(book3);

        // Act
        var allBooks = await _repository.FindAllAsync();

        // Assert
        allBooks.Should().HaveCount(3);
        allBooks.Should().Contain(b => b.Title == "Book 1");
        allBooks.Should().Contain(b => b.Title == "Book 2");
        allBooks.Should().Contain(b => b.Title == "Book 3");
    }

    [Fact]
    public async Task GivenRepositoryWithBooks_WhenUpdatingExistingBook_ThenFindAllReturnsUpdatedBook()
    {
        // Arrange
        var bookId = Guid.NewGuid();
        var originalBook = CreateTestBook(id: bookId, title: "Original", author: "Original Author");
        await _repository.SaveAsync(originalBook);

        var updatedBook = CreateTestBook(id: bookId, title: "Updated", author: "Updated Author");

        // Act
        await _repository.SaveAsync(updatedBook);
        var allBooks = await _repository.FindAllAsync();

        // Assert
        allBooks.Should().HaveCount(1);
        allBooks[0].Title.Should().Be("Updated");
        allBooks[0].Author.Should().Be("Updated Author");
    }

    /// <summary>
    /// Helper method to create a test Book with the given parameters.
    /// Uses reflection to set the private properties since Book has private setters.
    /// </summary>
    private static BookModel CreateTestBook(Guid id, string title, string author, bool isAvailable = true)
    {
        var book = new BookModel();

        // Set Id using reflection
        typeof(BaseEntity).GetProperty("Id")?.SetValue(book, id);
        typeof(BaseEntity).GetProperty("CreatedAt")?.SetValue(book, DateTime.UtcNow);

        // Set Book properties using reflection
        typeof(BookModel).GetProperty("Title")?.SetValue(book, title);
        typeof(BookModel).GetProperty("Author")?.SetValue(book, author);
        typeof(BookModel).GetProperty("IsAvailable")?.SetValue(book, isAvailable);

        return book;
    }
}