
namespace StudentBase.Domain.Extensions;

public static class FormsOfEducationExtensions
{
    public static string ToDisplayString(this FormsOfEducation forms)
    {
        return forms switch
        {
            FormsOfEducation.FullTime => "Очная форма",
            FormsOfEducation.Correspondence => "Заочная форма",
            _ => forms.ToString(),
        };
    }
}
