

using StudentBase.Application.Interfaces;
using StudentBase.Domain.Entities;

namespace StudentBase.Application.Implementations
{
    public class StudentService : IStudentService
    {
        private readonly IDataService _dataService;
        public StudentService(IDataService dataService)
        {
            _dataService = dataService;
        }

        public async Task<StudentResult<object>> CreateStudentAsync(StudentEntity entity)
        {
            try
            {
                if (entity == null)
                {
                    return new StudentResult<object>
                    {
                        Success = false,
                        ErrorMessage = "Пустые данные."
                    };
                }
                var result = await _dataService.Students.CreateAsync(entity);
                return new StudentResult<object>
                {
                    Success = true,
                    Message = "Данные успешно сохранены в базу."
                };
            }
            catch (Exception ex)
            {
                return new StudentResult<object>
                {
                    ErrorMessage = ex.Message,
                    Success = false
                };
            }
        }

        public async Task<StudentResult<object>> DeleteStudentAsync(int id)
        {
            try
            {
                var student = await _dataService.Students.GetByIdAsync(id);
                if (student == null)
                {
                    return new StudentResult<object>
                    {
                        Success = false,
                        ErrorMessage = $"Студент с ID {id} не найден."
                    };
                }
                var result = await _dataService.Students.DeleteAsync(id);
                if (result)
                {
                    return new StudentResult<object>
                    {
                        Success = true,
                        Message = $"Студент с ID {id} успешно удален."
                    };
                }
                else
                {
                    return new StudentResult<object>
                    {
                        Success = false,
                        ErrorMessage = $"Студента с ID {id} не получилось удалить или его не существует."
                    };
                }
            }
            catch (Exception ex)
            {
                return new StudentResult<object>
                {
                    Success = false,
                    ErrorMessage = ex.Message
                };
            }
        }

        public async Task<IEnumerable<StudentEntity>?> GetAllStudentsAsync()
        {
            try
            {
                return await _dataService.Students.GetAllAsync();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<IEnumerable<StudentEntity>?> GetAllStudentsByGroupIdAsync(int groupId)
        {
            try
            {
                return await _dataService.Students.GetAllByGroupIdAsync(groupId);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<IEnumerable<StudentEntity>?> GetAllStudentsByProgramIdAsync(int programId)
        {
            try
            {
                return await _dataService.Students.GetAllByProgramIdAsync(programId);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<StudentResult<object>> GetStudentByIdAsync(int id)
        {
            try
            {
                var result = await _dataService.Students.GetByIdAsync(id);
                if (result == null)
                {
                    return new StudentResult<object>
                    {
                        Success = false,
                        ErrorMessage = $"Студента с ID {id} не существует."
                    };
                }
                return new StudentResult<object>
                {
                    Success = true,
                    Student = result
                };
            }
            catch (Exception ex)
            {
                return new StudentResult<object>
                {
                    ErrorMessage = ex.Message,
                    Success = false
                };
            }
        }

        public async Task<StudentResult<object>> GetStudentByNameAsync(string name)
        {
            try
            {
                var result = await _dataService.Students.GetByNameAsync(name);
                if(result == null)
                {
                    return new StudentResult<object>
                    {
                        Success = false,
                        ErrorMessage = "Такого студента нет в базе."
                    };
                }
                return new StudentResult<object>
                {
                    Success = true,
                    Student = result
                };
            }
            catch (Exception ex)
            {
                return new StudentResult<object>
                {
                    ErrorMessage = ex.Message,
                    Success = false
                };
            }
        }

        public async Task<StudentResult<object>> GetStudentsCountAsync()
        {
            try
            {
                var count = await _dataService.Students.GetStudentsCountAsync();
                return new StudentResult<object>
                {
                    Success = true,
                    Count = count
                };
            }
            catch (Exception ex)
            {
                return new StudentResult<object>
                {
                    ErrorMessage = ex.Message,
                    Success = false
                };
            }
        }

        public async Task<StudentResult<object>> UpdateStudentAsync(StudentEntity entity)
        {
            try
            {
                if (entity == null)
                {
                    return new StudentResult<object>
                    {
                        ErrorMessage = "Данные пустые.",
                        Success = false
                    };
                }
                var result = await _dataService.Students.UpdateAsync(entity);
                if (result)
                {
                    return new StudentResult<object>
                    {
                        Message = "Данные успешно изменены.",
                        Success = true
                    };
                }
                else
                {
                    return new StudentResult<object>
                    {
                        ErrorMessage = $"В базе нету студента с ID {entity.Id}.",
                        Success = false
                    };
                }
            }
            catch(Exception ex)
            {
                return new StudentResult<object>
                {
                    ErrorMessage= ex.Message,
                    Success = false
                };
            }
        }
    }
}
