namespace LibraryManagementAPI.Shared;

public class BaseEntity
{ 
    public Guid Id { get; private set; }
    public DateTime CreatedAt { get; private set; }
}