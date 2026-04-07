using StudentBase.Application.Interfaces;
using StudentBase.Domain.Entities;
using StudentBase.Domain.Repositories;

namespace StudentBase.Application.Implementations
{
    public class StudentTransferService : IStudentTransferService
    {
        private readonly IStudentTransferRepository _studentTransferRepository;
        private readonly IStudentRepository _studentRepository;
        public StudentTransferService(IStudentTransferRepository studentTransferRepository, IStudentRepository studentRepository)
        {
            _studentTransferRepository = studentTransferRepository;
            _studentRepository = studentRepository;
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

                var student = await _studentRepository.GetByIdAsync(entity.StudentId);
                if (student == null)
                {
                    return new StudentTransferResult<object>
                    {
                        Success = false,
                        ErrorMessage = $"Студента с ID {entity.StudentId} не существует."
                    };
                }

                student.EducationalGroup = entity.ToGroup;
                student.CurrentGroupId = entity.ToGroupId;
                student.DurationTraining = entity.ToGroup.EducationalProgram.DurationTraining;
                student.StudentTransfers?.Add(entity);

                await _studentRepository.UpdateAsync(student);

                await _studentTransferRepository.CreateAsync(entity);
                return new StudentTransferResult<object>
                {
                    Success = true,
                    Message = "Данные успешно сохранены."
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
