using StudentBase.Domain.Entities;

namespace StudentBase.Domain.Repositories;

public interface IPaymentRepository : IRepository<PaymentEntity, int>
{
    Task<IEnumerable<PaymentEntity>?> GetAllByStudentAsync(int studentId);
}
