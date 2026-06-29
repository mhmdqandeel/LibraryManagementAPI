using LibraryManagementAPI.Shared;

namespace LibraryManagementAPI.Models;

public class Book : BaseEntity
{
    public string Title { get; private set; }
    public string Author { get; private set; }
    public bool IsAvailable { get; private set; }

    public Book(string title, string author)
        : base()
    {
        Title = title;
        Author = author;
        IsAvailable = true;
    }
    protected Book()
    {
    }
}