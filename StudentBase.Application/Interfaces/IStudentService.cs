using StudentBase.Domain.Entities;

namespace StudentBase.Application.Interfaces;

public record StudentResult<T>
{
    public bool Success { get; init; }
    public string? Message { get; init; }
    public string? ErrorMessage { get; init; }
    public int Count { get; init; }
    public StudentEntity? Student { get; init; }

}
public interface IStudentService
{
    Task<IEnumerable<StudentEntity>?> GetAllStudentsByGroupIdAsync(int groupId);
    Task<IEnumerable<StudentEntity>?> GetAllStudentsByProgramIdAsync(int programId);
    Task<StudentResult<object>> GetStudentByNameAsync(string name);
    Task<StudentResult<object>> GetStudentsCountAsync();
    Task<StudentResult<object>> CreateStudentAsync(StudentEntity entity);
    Task<StudentResult<object>> UpdateStudentAsync(StudentEntity entity);
    Task<StudentResult<object>> DeleteStudentAsync(int id);
    Task<IEnumerable<StudentEntity>?> GetAllStudentsAsync();
    Task<StudentResult<object>> GetStudentByIdAsync(int id);
}
