
namespace StudentBase.Domain.Extensions;

public static class TermsOfStudyExtensions
{
    public static string ToDisplayString(this TermsOfStudy term)
    {
        switch (term)
        {
            case TermsOfStudy.OneYearTenMonths:
                return "1 г. 10 мес.";
            case TermsOfStudy.TwoYearsTenMonths:
                return "2 г. 10 мес.";
            case TermsOfStudy.ThreeYearsTenMonths:
                return "3 г. 10 мес.";
            default:
                return term.ToString();
        }
    }
}
