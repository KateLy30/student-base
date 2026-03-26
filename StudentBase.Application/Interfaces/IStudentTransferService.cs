using StudentBase.Domain.Entities;

namespace StudentBase.Application.Interfaces;
public record StudentTransferResult<T>
{
    public bool Success { get; init; }
    public string? Message { get; init; }
    public string? ErrorMessage { get; init; }
    public int Count { get; init; }
    public StudentTransferEntity? StudentTransfer { get; init; }
}
public interface IStudentTransferService
{
    Task<StudentTransferResult<object>> CreateStudentTransferAsync(StudentTransferEntity entity);
    Task<StudentTransferResult<object>> UpdateStudentTransferAsync(StudentTransferEntity entity);
    Task<StudentTransferResult<object>> DeleteStudentTransferAsync(int id);
    Task<IEnumerable<StudentTransferEntity>?> GetAllStudentTransfersAsync();
    Task<StudentTransferResult<object>> GetStudentTransferById(int id);
    Task<IEnumerable<StudentTransferEntity>?> GetAllStudentTransfersByStudentAsync(int studentId);

}
