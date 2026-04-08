using StudentBase.Domain;
using StudentBase.Domain.Entities;

namespace StudentBase.Application.Interfaces;

public record ProgramResult<T>
{
    public bool Success { get; init; }
    public string? Message { get; init; }
    public string? ErrorMessage { get; init; }
    public int Count { get; init; }
    public StatusPrograms Status {  get; init; }
    public ProgramEntity? Program { get; init; }
    public List<T>? DataList { get; init; }

}
public interface IProgramService
{
    Task<ProgramResult<object>> CreateProgramAsync(ProgramEntity entity);
    Task<ProgramResult<object>> UpdateProgramAsync(ProgramEntity entity);
    Task<ProgramResult<object>> DeleteProgramAsync(int id);
    Task<ProgramResult<object>> GetProgramByIdAync(int id);
    Task<IEnumerable<ProgramEntity>?> GetAllProgramsAsync();
    Task<ProgramResult<object>> GetProgramsCountAsync();
    Task<ProgramResult<object>> GetProgramByQualificationAsync(string qualification);
    Task<ProgramResult<IEnumerable<ProgramEntity>>> GetAllProgramsBySpecialtyAsync(string specialty);
    Task<ProgramResult<object>> GetStatusProgramAsync(int id);
}
