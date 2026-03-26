using StudentBase.Domain;
using StudentBase.Domain.Entities;

namespace StudentBase.Application.Interfaces;
public record GroupResult<T>
{
    public bool Success { get; init; }
    public string? Message { get; init; }
    public string? ErrorMessage { get; init; }
    public int Count { get; init; }
    public GroupEntity? Group { get; init; }
    public StatusGroups? Status { get; init; }
}

public interface IGroupService
{
    Task<GroupResult<object>> CreateGroupAsync(GroupEntity entity);
    Task<GroupResult<object>> UpdateGroupAsync(GroupEntity entity);
    Task<GroupResult<object>> DeleteGroupAsync(int id);
    Task<GroupResult<object>> GetGroupByIdAync(int id);
    Task<GroupResult<object>> GetGroupByNameAsync(string name);
    Task<IEnumerable<GroupEntity>?> GetAllGroupsAsync();
    Task<IEnumerable<GroupEntity>?> GetAllGroupsByProgramIdAsync(int programId);
    Task<GroupResult<object>> GetGroupsCountAsync();
    Task<GroupResult<object>> GetStatusGroupAsync(int id);
}
