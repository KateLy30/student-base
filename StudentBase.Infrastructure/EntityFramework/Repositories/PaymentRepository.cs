using Microsoft.EntityFrameworkCore;
using StudentBase.Domain.Entities;
using StudentBase.Domain.Repositories;

namespace StudentBase.Infrastructure.EntityFramework.Repositories;

public class PaymentRepository : IPaymentRepository
{
    private readonly AppDbContext _context;
    public PaymentRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task<bool> CreateAsync(PaymentEntity entity)
    {
        entity.CreateAt = DateTime.Now;
        await _context.Payments.AddAsync(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _context.Payments.FindAsync(id);
        if (entity == null) return false;

        _context.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<PaymentEntity>?> GetAllAsync()
    {
        return await _context.Payments.Include(p => p.Student).ToListAsync();
    }

    public async Task<IEnumerable<PaymentEntity>?> GetAllByStudentAsync(int studentId)
    {
        return await _context.Payments.Where(r => r.StudentId == studentId).ToListAsync();
    }

    public async Task<PaymentEntity?> GetByIdAsync(int id)
    {
        return await _context.Payments.FindAsync(id);
    }

    public async Task<bool> UpdateAsync(PaymentEntity entity)
    {
        var entityInDB = await _context.Payments.FindAsync(entity.Id);
        if (entityInDB == null) return false;

        UpdateEntity(entityInDB, entity);
        await _context.SaveChangesAsync();
        return true;
    }

    public static void UpdateEntity(PaymentEntity entityInDatabase, PaymentEntity updatedEntity)
    {
        entityInDatabase.StudentId = updatedEntity.StudentId;
        entityInDatabase.PaidSemester = updatedEntity.PaidSemester;
        entityInDatabase.Amount = updatedEntity.Amount;
        entityInDatabase.PaymentDate = updatedEntity.PaymentDate;
        entityInDatabase.UpdateAt = updatedEntity.UpdateAt;
        entityInDatabase.PaymentType = updatedEntity.PaymentType;
        entityInDatabase.Comment = updatedEntity.Comment;
        entityInDatabase.Student = updatedEntity.Student;
    }
}
