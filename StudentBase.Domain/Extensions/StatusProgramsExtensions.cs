
namespace StudentBase.Domain.Extensions;

public static class StatusProgramsExtensions
{
    public static string ToDisplayString(this StatusPrograms status)
    {
        return status switch
        {
            StatusPrograms.CurrentProgram => "Актуальная программа",
            StatusPrograms.ProgramIsArchived => "Программа в архиве",
            _ => status.ToString(),
        };
    }
}
