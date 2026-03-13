using StudentBase.Application.Interfaces;
using StudentBase.Domain.Entities;

namespace StudentBase.Application.implementations
{
    public class GroupService : IGroupService
    {
        private readonly IDataService _dataService;
        public GroupService(IDataService dataService)
        {
            _dataService = dataService;
        }

        public async Task<GroupResult<object>> CreateGroupAsync(GroupEntity entity)
        {
            try
            {
                if (entity == null)
                    return new GroupResult<object> { Success = false, ErrorMessage = "Пустые данные." };

                var result = await _dataService.Groups.CreateAsync(entity);
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
                var result = await _dataService.Groups.DeleteAsync(id);
                if (result)
                {
                    return new GroupResult<object>
                    {
                        Success = result,
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

        public Task<IEnumerable<GroupEntity>?> GetAllGroupsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<GroupResult<object>> GetByIdAync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<GroupResult<object>> GetGroupsCountAsync()
        {
            throw new NotImplementedException();
        }

        public Task<GroupResult<object>> GetStatusGroupAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<GroupResult<object>> UpdateGroupAsync(GroupEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
