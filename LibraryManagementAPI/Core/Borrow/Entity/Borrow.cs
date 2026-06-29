using LibraryManagementAPI.Shared;

namespace LibraryManagementAPI.Models;

public class Borrow : BaseEntity
{
    public Guid BookId { get; private set; }
    public Guid MemberId { get; private set; }
    public DateTime ReturnDate { get; private set; }
    public bool IsReturned { get; private set; }
    
    public Borrow(Guid bookId, Guid memberId, DateTime returnDate)
    {
        BookId = bookId;
        MemberId = memberId;
        ReturnDate = returnDate;
        IsReturned = false;
    }
    
    public Borrow()
    {
    }
}