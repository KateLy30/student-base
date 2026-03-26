using StudentBase.Application.Interfaces;
using StudentBase.Domain.Entities;
using StudentBase.Domain.Repositories;

namespace StudentBase.Application.Implementations
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        public PaymentService(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public async Task<PaymentResult<object>> CreatePaymentTransferAsync(PaymentEntity entity)
        {
            try
            {
                if (entity == null)
                {
                    return new PaymentResult<object>
                    {
                        Success = false,
                        ErrorMessage = "Данные пустые."
                    };
                }
                var result = await _paymentRepository.CreateAsync(entity);
                return new PaymentResult<object>
                {
                    Success = true,
                    Message = "Данные успешно созранены."
                };
            }
            catch (Exception ex)
            {
                return new PaymentResult<object>
                {
                    Success = false,
                    ErrorMessage = ex.Message
                };
            }
        }

        public async Task<PaymentResult<object>> DeletePaymentTransferAsync(int id)
        {
            try
            {
                var result = await _paymentRepository.DeleteAsync(id);
                if (result)
                {
                    return new PaymentResult<object>
                    {
                        Success = true,
                        Message = $"Квитанция с ID {id} успешно удалена."
                    };
                }

                return new PaymentResult<object>
                {
                    Success = false,
                    ErrorMessage = $"Квитанцию с ID {id} не удалось удалить или ее не существует."
                };
            }
            catch (Exception ex)
            {
                return new PaymentResult<object>
                {
                    Success = false,
                    ErrorMessage = ex.Message
                };
            }
        }

        public async Task<IEnumerable<PaymentEntity>?> GetAllPaymentsAsync()
        {
            try
            {
                return await _paymentRepository.GetAllAsync();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<IEnumerable<PaymentEntity>?> GetAllPaymentsByStudentAsync(int studentId)
        {
            try
            {
                return await _paymentRepository.GetAllByStudentAsync(studentId);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<PaymentResult<object>> GetPaymentById(int id)
        {
            try
            {
                var result = await _paymentRepository.GetByIdAsync(id);
                if(result == null)
                {
                    return new PaymentResult<object>
                    {
                        Success = false,
                        ErrorMessage = $"Квитанции с ID {id} не существует."
                    };
                }

                return new PaymentResult<object>
                {
                    Success = true,
                    Payment = result
                };
            }
            catch (Exception ex)
            {
                return new PaymentResult<object>
                {
                    Success = false,
                    ErrorMessage = ex.Message
                };
            }
        }

        public async Task<PaymentResult<object>> UpdatePaymentTransferAsync(PaymentEntity entity)
        {
            try
            {
                if(entity == null)
                {
                    return new PaymentResult<object>
                    {
                        Success = false,
                        ErrorMessage = "Данные пустые."
                    };
                }
                var result = await _paymentRepository.UpdateAsync(entity);
                if (result)
                {
                    return new PaymentResult<object>
                    {
                        Success = true,
                        Message = "Данные успешно изменены."
                    };
                }

                return new PaymentResult<object>
                {
                    Success = false,
                    ErrorMessage = $"Квитанция с ID {entity.Id} не найденa."
                };
            }
            catch(Exception ex)
            {
                return new PaymentResult<object>
                {
                    Success = false,
                    ErrorMessage = ex.Message
                };
            }
        }
    }
}
