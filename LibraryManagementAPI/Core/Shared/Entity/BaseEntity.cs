namespace LibraryManagementAPI.Shared;

public class BaseEntity
{ 
    public Guid Id { get; private set; }
    public DateTime CreatedAt { get; private set; }
    
    protected BaseEntity()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
    }
}