using LibraryManagementAPI.Shared;

namespace LibraryManagementAPI.Core.Member.Entity;

public class Member : BaseEntity
{
    public string Name { get; private set; }
    public string Email { get; private set; }
    public int BorrowLimit  { get; private set; }
    
    public Member(string name, string email, int borrowLimit)
    {
        Name = name;
        Email = email;
        BorrowLimit = borrowLimit;
    }
    
    public Member()
    {
    }
}