using StudentBase.Application.Interfaces;
using StudentBase.Domain;
using StudentBase.Domain.Entities;
using StudentBase.Domain.Repositories;

namespace StudentBase.Application.implementations
{
    public class ProgramService : IProgramService
    {
        private readonly IProgramRepository _programRepository;
        public ProgramService(IProgramRepository programRepository)
        {
            _programRepository = programRepository;
        }
        public async Task<ProgramResult<object>> CreateProgramAsync(ProgramEntity entity)
        {
            try
            {
                if (entity == null)
                    return new ProgramResult<object> { ErrorMessage = "Пустые данные.", Success = false };

                var result = await _programRepository.CreateAsync(entity);
                return new ProgramResult<object> { Message = "Данные успешно сохранены в базу.", Success = result };
            }
            catch (Exception ex)
            {
                return new ProgramResult<object> { ErrorMessage = ex.Message, Success = false };
            }
        }

        public async Task<ProgramResult<object>> DeleteProgramAsync(int id)
        {
            try
            {
                var program = await _programRepository.GetByIdAsync(id);
                if (program == null)
                {
                    return new ProgramResult<object> { Success = false, ErrorMessage = $"Программа с ID {id} не найдена." };
                }
                if (program.EducationalGroups != null)
                {

                }
                var result = await _programRepository.DeleteAsync(id);
                if (result)
                {
                    return new ProgramResult<object>
                    {
                        Success = result,
                        Message = $"Программа с ID {id} успешно удалена."
                    };
                }
                else
                {
                    return new ProgramResult<object>
                    {
                        Success = result,
                        ErrorMessage = $"Программу с ID {id} не получилось удалить или ее не существует."
                    };
                }
            }
            catch (Exception ex)
            {
                return new ProgramResult<object> { Success = false, ErrorMessage = ex.Message };
            }
        }

        public async Task<ProgramResult<IEnumerable<ProgramEntity>>> GetAllProgramsBySpecialtyAsync(string specialty)
        {
            try
            {
                if (specialty == null)
                {
                    return new ProgramResult<IEnumerable<ProgramEntity>>
                    {
                        ErrorMessage = "Специальность пустая.",
                        Success = false,
                        DataList = null
                    };
                }
                var result = await _programRepository.GetAllBySpecialtyAsync(specialty);
                return new ProgramResult<IEnumerable<ProgramEntity>>
                {
                    Success = true,
                    DataList = [result]
                };
            }
            catch (Exception ex)
            {
                return new ProgramResult<IEnumerable<ProgramEntity>>
                {
                    ErrorMessage = ex.Message,
                    DataList = null,
                    Success = false
                };
            }
        }

        public async Task<IEnumerable<ProgramEntity>?> GetAllProgramsAsync()
        {
            try
            {
                return await _programRepository.GetAllAsync();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<ProgramResult<object>> GetProgramByIdAync(int id)
        {
            try
            {
                var result = await _programRepository.GetByIdAsync(id);
                if (result == null)
                {
                    return new ProgramResult<object>
                    {
                        Success = false,
                        ErrorMessage = $"Программы с ID {id} не существует."
                    };
                }
                return new ProgramResult<object>
                {
                    Success = true,
                    Program = result
                };
            }
            catch (Exception ex)
            {
                return new ProgramResult<object>
                {
                    ErrorMessage = ex.Message,
                    Success = false
                };
            }
        }

        public async Task<ProgramResult<object>> GetProgramByQualificationAsync(string qualification)
        {
            try
            {
                var result = await _programRepository.GetByQualificationAsync(qualification);
                if (result == null)
                {
                    return new ProgramResult<object>
                    {
                        Success = false,
                        ErrorMessage = "Программы с такой квалификацией не существует"
                    };
                }
                return new ProgramResult<object>
                {
                    Success = true,
                    Program = result
                };
            }
            catch (Exception ex)
            {
                return new ProgramResult<object> { Success = false, ErrorMessage = ex.Message };
            }
        }

        public async Task<ProgramResult<object>> GetProgramsCountAsync()
        {
            try
            {
                var count = await _programRepository.GetProgramsCountAsync();
                return new ProgramResult<object> { Count = count, Success = true };
            }
            catch (Exception ex)
            {
                return new ProgramResult<object> { ErrorMessage = ex.Message, Success = false };
            }
        }

        public async Task<ProgramResult<object>> GetStatusProgramAsync(int id)
        {
            try
            {
                var status = await _programRepository.GetStatusProgramAsync(id);
                if (status == null) return new ProgramResult<object> { ErrorMessage = "У программы нет статуса.", Success = false };
                return new ProgramResult<object> { Success = true, Status = (StatusPrograms)status };
            }
            catch (Exception ex)
            {
                return new ProgramResult<object> { ErrorMessage = ex.Message, Success = false };
            }
        }

        public async Task<ProgramResult<object>> UpdateProgramAsync(ProgramEntity entity)
        {
            try
            {
                if (entity == null) return new ProgramResult<object> { ErrorMessage = "Данные пустые.", Success = false };
                var result = await _programRepository.UpdateAsync(entity);
                if (result)
                {
                    return new ProgramResult<object>
                    {
                        Message = "Данные успешно изменены.",
                        Success = true
                    };
                }
                else
                {
                    return new ProgramResult<object>
                    {
                        ErrorMessage = $"В базе нету программы с ID {entity.Id}.",
                        Success = false
                    };
                }
            }
            catch (Exception ex)
            {
                return new ProgramResult<object> { ErrorMessage = ex.Message, Success = false };
            }
        }
    }
}
