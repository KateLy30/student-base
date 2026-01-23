
namespace StudentBase.Domain;
public enum StatusGroups
{
    Open,
    Closed
}
public enum StatusStudents
{
    Studying,
    Graduated
}
public enum StatusPrograms
{
    Relevant,
    InArchive
}
public enum FormsOfEducation
{
    FullTime,
    Correspondence
}
public enum TermsOfStudy
{
    OneYearTenMonths = 22,      // 1 year and 10 months
    TwoYearsTenMonths = 34,     // 2 years and 10 months
    ThreeYearsTenMonths = 46    //3 years and 10 months
}
public enum LevelsOfEducation
{
    BasicGeneral,      // 9 classes
    SecondaryGeneral  // 11 classes
}
