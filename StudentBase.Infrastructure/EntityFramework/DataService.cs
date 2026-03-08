using StudentBase.Application.Interfaces;
using StudentBase.Domain.Repositories;
using StudentBase.Infrastructure.EntityFramework.Repositories;

namespace StudentBase.Infrastructure.EntityFramework;

public class DataService : IDataService
{
    private readonly AppDbContext _context;
    public DataService(AppDbContext context)
    {
        _context = context;

        Students = new StudentRepository(_context);
        Groups = new GroupRepository(_context);
        Programs = new ProgramRepository(_context);
        Transfers = new StudentTransferRepository(_context);
        Receipts = new PaymentRepository(_context);
    }
    public IStudentRepository Students { get; }

    public IGroupRepository Groups { get; }

    public IProgramRepository Programs { get; }

    public IStudentTransferRepository Transfers { get; }

    public IPaymentRepository Receipts { get; }
}
