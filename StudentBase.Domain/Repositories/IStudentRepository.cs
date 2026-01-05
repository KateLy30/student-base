using StudentBase.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentBase.Domain.Repositories
{
    public interface IStudentRepository : IRepository<StudentEntity, int>
    {
        Task<IEnumerable<StudentEntity>?> GetAllByGroupIdAsync(int groupId);
        Task<IEnumerable<StudentEntity>?> GetAllByProgramIdAsync(int programId);
        Task<StudentEntity?> GetByEmailAsync(string email);
        Task<StudentEntity?> GetByPhoneAsync(string phone);
        Task<StudentEntity?> GetByNameAsync(string name);
    }
}
