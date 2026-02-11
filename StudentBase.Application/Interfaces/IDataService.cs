using StudentBase.Domain.Repositories;

namespace StudentBase.Application.Interfaces;

public interface IDataService
{
    IStudentRepository Students { get; }
    IGroupRepository Groups { get; }
    IProgramRepository Programs { get; }
}
