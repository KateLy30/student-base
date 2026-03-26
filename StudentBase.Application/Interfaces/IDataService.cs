using StudentBase.Domain.Repositories;

namespace StudentBase.Application.Interfaces;

public interface IDataService
{
    IStudentService StudentService { get; }
    IProgramService ProgramService { get; }
    IGroupService GroupService { get; }
    IStudentTransferService StudentTransferService { get; }
    IPaymentService PaymentsService { get; }
}
