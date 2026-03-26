using StudentBase.Application.Interfaces;
using StudentBase.Domain.Entities;
using StudentBase.Domain.Repositories;

namespace StudentBase.Application.implementations
{
    public class GroupService : IGroupService
    {
        private readonly IGroupRepository _groupRepository;
        public GroupService(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }

        public async Task<GroupResult<object>> CreateGroupAsync(GroupEntity entity)
        {
            try
            {
                if (entity == null)
                    return new GroupResult<object> { Success = false, ErrorMessage = "Пустые данные." };

                var result = await _groupRepository.CreateAsync(entity);
                return new GroupResult<object>
                {
                    Message = "Данные успешно сохранены в базу",
                    Success = result
                };
            }
            catch (Exception ex)
            {
                return new GroupResult<object> { ErrorMessage = ex.Message, Success = false };
            }
        }

        public async Task<GroupResult<object>> DeleteGroupAsync(int id)
        {
            try
            {
                var result = await _groupRepository.DeleteAsync(id);
                if (result)
                {
                    return new GroupResult<object>
                    {
                        Success = true,
                        Message = $"Группа с ID {id} успешна удалена."
                    };
                }
                else
                {
                    return new GroupResult<object>
                    {
                        Success = false,
                        ErrorMessage = $"Группу с ID {id} не получилось удалить или ее не существует."
                    };
                }
            }
            catch (Exception ex)
            {
                return new GroupResult<object>
                {
                    Success = false,
                    ErrorMessage = ex.Message,
                };
            }
        }

        public async Task<IEnumerable<GroupEntity>?> GetAllGroupsAsync()
        {
            try
            {
                return await _groupRepository.GetAllAsync();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<IEnumerable<GroupEntity>?> GetAllGroupsByProgramIdAsync(int programId)
        {
            try
            {
                return await _groupRepository.GetAllByProgramIdAsync(programId);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<GroupResult<object>> GetGroupByIdAync(int id)
        {
            try
            {
                var result = await _groupRepository.GetByIdAsync(id);
                if (result == null)
                {
                    return new GroupResult<object>
                    {
                        Success = false,
                        ErrorMessage = $"Группа с {id} не найдена."
                    };
                }

                return new GroupResult<object>
                {
                    Success = true,
                    Group = result
                };
            }
            catch (Exception ex)
            {
                return new GroupResult<object>
                {
                    Success = false,
                    ErrorMessage = ex.Message
                };
            }
        }

        public async Task<GroupResult<object>> GetGroupByNameAsync(string name)
        {
            try
            {
                if (name == null)
                {
                    return new GroupResult<object>
                    {
                        Success = false,
                        ErrorMessage = "Пустое имя"
                    };
                }
                var result = await _groupRepository.GetByNameAsync(name);
                if (result == null)
                {
                    return new GroupResult<object>
                    {
                        Success = false,
                        ErrorMessage = $"Группа с названием: {name} не найдена."
                    };
                }

                return new GroupResult<object>
                {
                    Success = true,
                    Group = result
                };
            }
            catch (Exception ex)
            {
                return new GroupResult<object>
                {
                    Success = false,
                    ErrorMessage = ex.Message
                };
            }
        }

        public async Task<GroupResult<object>> GetGroupsCountAsync()
        {
            try
            {
                var result = await _groupRepository.GetGroupsCountAsync();
                return new GroupResult<object>
                {
                    Success = true,
                    Count = result
                };
            }
            catch (Exception ex)
            {
                return new GroupResult<object>
                {
                    Success = false,
                    ErrorMessage = ex.Message
                };
            }
        }

        public async Task<GroupResult<object>> GetStatusGroupAsync(int id)
        {
            try
            {
                var result = await _groupRepository.GetStatusGroupsAsync(id);
                if (result == null)
                {
                    return new GroupResult<object>
                    {
                        Success = false,
                        ErrorMessage = $"Группа с ID {id} не найдена."
                    };
                }

                return new GroupResult<object>
                {
                    Success = true,
                    Status = result
                };
            }
            catch (Exception ex)
            {
                return new GroupResult<object>
                {
                    Success = false,
                    ErrorMessage = ex.Message
                };
            }
        }

        public async Task<GroupResult<object>> UpdateGroupAsync(GroupEntity entity)
        {
            try
            {
                if (entity == null)
                {
                    return new GroupResult<object>
                    {
                        Success = false,
                        ErrorMessage = "Данные пустые"
                    };
                }

                var result = await _groupRepository.UpdateAsync(entity);
                if (result)
                {
                    return new GroupResult<object>
                    {
                        Success = true,
                        Message = "Данные успешно изменены."
                    };
                }

                return new GroupResult<object>
                {
                    Success = false,
                    ErrorMessage = $"В базе нет группы с ID {entity.Id}."
                };
            }
            catch (Exception ex)
            {
                return new GroupResult<object>
                {
                    Success = false,
                    ErrorMessage = ex.Message
                };
            }
        }
    }
}
