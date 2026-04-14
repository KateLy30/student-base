using StudentBase.Domain.Entities;

namespace StudentBase.Application.Interfaces;

public record PaymentResult<T>
{
    public bool Success { get; init; }
    public string? Message { get; init; }
    public string? ErrorMessage { get; init; }
    public int Count { get; init; }
    public PaymentEntity? Payment { get; init; }
}

public interface IPaymentService
{
    Task<PaymentResult<object>> CreatePaymentAsync(PaymentEntity entity);
    Task<PaymentResult<object>> UpdatePaymentAsync(PaymentEntity entity);
    Task<PaymentResult<object>> DeletePaymentAsync(int id);
    Task<IEnumerable<PaymentEntity>?> GetAllPaymentsAsync();
    Task<PaymentResult<object>> GetPaymentById(int id);
    Task<IEnumerable<PaymentEntity>?> GetAllPaymentsByStudentAsync(int studentId);
}
