namespace LibraryManagementAPI.Core.Member.Repository.InMemory;

public class InMemoryMemberRepository : IMemberRepository
{
    private readonly List<Core.Member.Entity.Member> _members = [];

    public Task<Core.Member.Entity.Member> SaveAsync(Core.Member.Entity.Member entity)
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

    public Task<Core.Member.Entity.Member?> FindAsync(Guid id)
    {
        return Task.FromResult(_members.FirstOrDefault(member => member.Id == id));
    }

    public Task<IReadOnlyList<Core.Member.Entity.Member>> FindAllAsync()
    {
        return Task.FromResult<IReadOnlyList<Core.Member.Entity.Member>>(_members);
    }
}