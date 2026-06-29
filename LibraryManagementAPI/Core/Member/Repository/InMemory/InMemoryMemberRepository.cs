namespace LibraryManagementAPI.Member.Repository.InMemory;

public class InMemoryMemberRepository : IMemberRepository
{
    private readonly List<Models.Member> _members = [];

    public Task<Models.Member> SaveAsync(Models.Member entity)
    {
        var existingMember = _members.FirstOrDefault(member => member.Id == entity.Id);

        if (existingMember is null)
        {
            _members.Add(entity);
        }
        else
        {
            var index = _members.IndexOf(existingMember);
            _members[index] = entity;
        }

        return Task.FromResult(entity);
    }

    public Task<Models.Member?> FindAsync(Guid id)
    {
        return Task.FromResult(_members.FirstOrDefault(member => member.Id == id));
    }

    public Task<IReadOnlyList<Models.Member>> FindAllAsync()
    {
        return Task.FromResult<IReadOnlyList<Models.Member>>(_members);
    }
}