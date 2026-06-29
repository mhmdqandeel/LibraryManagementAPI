using LibraryManagementAPI.Shared;

namespace LibraryManagementAPI.Models;

public class Member : BaseEntity
{
    public string Name { get; private set; }
    public string Email { get; private set; }
    public int BorrowLimit  { get; private set; }
    
    protected Member(string name, string email, int borrowLimit)
    {
        Name = name;
        Email = email;
        BorrowLimit = borrowLimit;
    }
    
    protected Member()
    {
    }
}