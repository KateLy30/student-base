using StudentBase.Domain.Entities;

namespace StudentBase.Domain.Repositories;

public interface IStudentTransferRepository : IRepository<StudentTransferEntity, int>
{
    Task<IEnumerable<StudentTransferEntity>?> GetAllByStudentAsync(int studentId); 
}
