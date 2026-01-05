using StudentBase.Domain.Entities;

namespace StudentBase.Domain.Repositories
{
    public interface IStudentRepository : IRepository<StudentEntity, int>
    {
        Task<IEnumerable<StudentEntity>?> GetAllByGroupIdAsync(int groupId);
        Task<IEnumerable<StudentEntity>?> GetAllByProgramIdAsync(int programId);
        Task<StudentEntity?> GetByEmailAsync(string email);
        Task<StudentEntity?> GetByPhoneAsync(string phone);
        Task<StudentEntity?> GetByNameAsync(string name);
        Task<int> GetStudentsCountAsync();
    }
}
