using Microsoft.VisualBasic;
using StudentBase.Application.Interfaces;
using StudentBase.Domain.Entities;
using StudentBase.Domain.Repositories;

namespace StudentBase.Application.Implementations
{
    public class StudentTransferService : IStudentTransferService
    {
        private readonly IStudentTransferRepository _studentTransferRepository;
        public StudentTransferService(IStudentTransferRepository studentTransferRepository)
        {
            _studentTransferRepository = studentTransferRepository;
        }

        public async Task<StudentTransferResult<object>> CreateStudentTransferAsync(StudentTransferEntity entity)
        {
            try
            {
                if (entity == null)
                {
                    return new StudentTransferResult<object>
                    {
                        Success = false,
                        ErrorMessage = "Данные пустые."
                    };
                }
                var result = await _studentTransferRepository.CreateAsync(entity);
                return new StudentTransferResult<object>
                {
                    Success = true,
                    Message = "Данные успешно созранены."
                };
            }
            catch (Exception ex)
            {
                return new StudentTransferResult<object>
                {
                    Success = false,
                    ErrorMessage = ex.Message,
                };
            }
        }

        public async Task<StudentTransferResult<object>> DeleteStudentTransferAsync(int id)
        {
            try
            {
                var result = await _studentTransferRepository.DeleteAsync(id);
                if (result)
                {
                    return new StudentTransferResult<object>
                    {
                        Success = true,
                        Message = $"Перевод с ID {id} успешно удален."
                    };
                }

                return new StudentTransferResult<object>
                {
                    Success = false,
                    ErrorMessage = $"Перевод с ID {id} не удалось удалить или его не существует."
                };
            }
            catch(Exception ex)
            {
                return new StudentTransferResult<object>
                {
                    Success = false,
                    ErrorMessage = ex.Message,
                };
            }
        }

        public async Task<IEnumerable<StudentTransferEntity>?> GetAllStudentTransfersAsync()
        {
            try
            {
                return await _studentTransferRepository.GetAllAsync();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<IEnumerable<StudentTransferEntity>?> GetAllStudentTransfersByStudentAsync(int studentId)
        {
            try
            {
                return await _studentTransferRepository.GetAllByStudentAsync(studentId);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<StudentTransferResult<object>> GetStudentTransferById(int id)
        {
            try
            {
                var result = await _studentTransferRepository.GetByIdAsync(id);
                if (result == null)
                {
                    return new StudentTransferResult<object>
                    {
                        Success = false,
                        ErrorMessage = $"Перевода с ID {id} не существует."
                    };
                }

                return new StudentTransferResult<object>
                {
                    Success = true,
                    StudentTransfer = result
                };
            }
            catch (Exception ex)
            {
                return new StudentTransferResult<object>
                {
                    Success = false,
                    ErrorMessage = ex.Message
                };
            }
        }

        public async Task<StudentTransferResult<object>> UpdateStudentTransferAsync(StudentTransferEntity entity)
        {
            try
            {
                if (entity == null)
                {
                    return new StudentTransferResult<object>
                    {
                        Success = false,
                        ErrorMessage = "Данные пустые."
                    };
                }
                var result = await _studentTransferRepository.UpdateAsync(entity);
                if (result)
                {
                    return new StudentTransferResult<object>
                    {
                        Success = true,
                        Message = "Данные успешно изменены."
                    };
                }

                return new StudentTransferResult<object>
                {
                    Success = false,
                    ErrorMessage = $"Перевод с ID {entity.Id} не найден."
                };
            }
            catch(Exception ex)
            {
                return new StudentTransferResult<object>
                {
                    Success = false,
                    ErrorMessage = ex.Message
                };
            }
        }
    }
}
