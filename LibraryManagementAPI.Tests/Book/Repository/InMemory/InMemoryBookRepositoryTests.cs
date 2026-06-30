using FluentAssertions;
using LibraryManagementAPI.Core.Book.Repository.InMemory;
using LibraryManagementAPI.Shared;
using BookModel = LibraryManagementAPI.Core.Book.Entity.Book;

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

        var originalBook = CreateTestBook(
            id: bookId,
            title: "Clean Code",
            author: "Robert C. Martin",
            isAvailable: true);

        await _repository.SaveAsync(originalBook);

        var updatedBook = CreateTestBook(
            id: bookId,
            title: "Clean Architecture",
            author: "Robert C. Martin",
            isAvailable: false);

        // Act
        var result = await _repository.SaveAsync(updatedBook);

        // Assert
        result.Id.Should().Be(bookId);
        result.Title.Should().Be(updatedBook.Title);
        result.Author.Should().Be(updatedBook.Author);
        result.IsAvailable.Should().BeFalse();
    }

    [Fact]
    public async Task GivenRepositoryWithMultipleBooks_WhenSavingAnotherBook_ThenAllBooksArePersisted()
    {
        // Arrange
        var book1 = CreateTestBook(Guid.NewGuid(), "Book 1", "Author 1", true);
        var book2 = CreateTestBook(Guid.NewGuid(), "Book 2", "Author 2", false);

        await _repository.SaveAsync(book1);
        await _repository.SaveAsync(book2);

        var book3 = CreateTestBook(Guid.NewGuid(), "Book 3", "Author 3", true);

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

        var book = CreateTestBook(
            bookId,
            "Domain-Driven Design",
            "Eric Evans",
            true);

        await _repository.SaveAsync(book);

        // Act
        var foundBook = await _repository.FindAsync(bookId);

        // Assert
        foundBook.Should().NotBeNull();
        foundBook!.Id.Should().Be(bookId);
        foundBook.Title.Should().Be(book.Title);
        foundBook.Author.Should().Be(book.Author);
        foundBook.IsAvailable.Should().BeTrue();
    }

    [Fact]
    public async Task GivenRepositoryWithBooks_WhenFindingByNonExistentId_ThenNullIsReturned()
    {
        // Arrange
        var book = CreateTestBook(Guid.NewGuid(), "Book", "Author", true);
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
        var book1 = CreateTestBook(Guid.NewGuid(), "Book 1", "Author 1", true);
        var book2 = CreateTestBook(Guid.NewGuid(), "Book 2", "Author 2", false);
        var book3 = CreateTestBook(Guid.NewGuid(), "Book 3", "Author 3", true);

        await _repository.SaveAsync(book1);
        await _repository.SaveAsync(book2);
        await _repository.SaveAsync(book3);

        // Act
        var allBooks = await _repository.FindAllAsync();

        // Assert
        allBooks.Should().HaveCount(3);
        allBooks.Should().Contain(b => b.Title == book1.Title);
        allBooks.Should().Contain(b => b.Title == book2.Title);
        allBooks.Should().Contain(b => b.Title == book3.Title);
    }

    [Fact]
    public async Task GivenRepositoryWithBooks_WhenUpdatingExistingBook_ThenFindAllReturnsUpdatedBook()
    {
        // Arrange
        var bookId = Guid.NewGuid();

        var originalBook = CreateTestBook(
            bookId,
            "Old Title",
            "Old Author",
            true);

        await _repository.SaveAsync(originalBook);

        var updatedBook = CreateTestBook(
            bookId,
            "New Title",
            "New Author",
            false);

        // Act
        await _repository.SaveAsync(updatedBook);
        var allBooks = await _repository.FindAllAsync();

        // Assert
        allBooks.Should().HaveCount(1);

        allBooks[0].Id.Should().Be(bookId);
        allBooks[0].Title.Should().Be(updatedBook.Title);
        allBooks[0].Author.Should().Be(updatedBook.Author);
        allBooks[0].IsAvailable.Should().BeFalse();
    }

    /// <summary>
    /// Helper method to create a test Book with the given parameters.
    /// Uses reflection to invoke the protected constructor and set private properties.
    /// </summary>
    private static BookModel CreateTestBook(
        Guid id,
        string title,
        string author,
        bool isAvailable)
    {
        var book = (BookModel)Activator.CreateInstance(typeof(BookModel), nonPublic: true)!;

        typeof(BaseEntity).GetProperty("Id")?.SetValue(book, id);
        typeof(BaseEntity).GetProperty("CreatedAt")?.SetValue(book, DateTime.UtcNow);

        typeof(BookModel).GetProperty("Title")?.SetValue(book, title);
        typeof(BookModel).GetProperty("Author")?.SetValue(book, author);
        typeof(BookModel).GetProperty("IsAvailable")?.SetValue(book, isAvailable);

        return book;
    }
}