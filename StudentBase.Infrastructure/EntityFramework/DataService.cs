using StudentBase.Application.implementations;
using StudentBase.Application.Implementations;
using StudentBase.Application.Interfaces;
using StudentBase.Domain.Repositories;

namespace StudentBase.Infrastructure.EntityFramework;

public class DataService : IDataService
{
    private readonly IProgramRepository _programRepository;
    private readonly IStudentRepository _studentRepository;
    private readonly IGroupRepository _groupRepository;
    private readonly IStudentTransferRepository _studentTransferRepository;
    private readonly IPaymentRepository _paymentRepository;
    public DataService(IProgramRepository programRepository, IStudentRepository studentRepository, IGroupRepository groupRepository, IStudentTransferRepository studentTransferRepository, IPaymentRepository paymentRepository)
    {
        _programRepository = programRepository;
        _studentRepository = studentRepository;
        _groupRepository = groupRepository;
        _studentTransferRepository = studentTransferRepository;
        _paymentRepository = paymentRepository;

        StudentService = new StudentService(_studentRepository);
        ProgramService = new ProgramService(_programRepository);
        GroupService = new GroupService(_groupRepository);
        StudentTransferService = new StudentTransferService(_studentTransferRepository);
        PaymentsService = new PaymentService(_paymentRepository);
    }
    public IStudentService StudentService { get; }

    public IProgramService ProgramService { get; }

    public IGroupService GroupService { get; }

    public IStudentTransferService StudentTransferService { get; }

    public IPaymentService PaymentsService { get; }
}
