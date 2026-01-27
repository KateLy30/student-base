
namespace StudentBase.Domain;
public enum StatusGroups
{
    Открыта,
    Закрыта
}
public enum StatusStudents
{
    Обучается,
    Выпустился
}
public enum StatusPrograms
{
    Актуальна,
    В_Архиве
}
public enum FormsOfEducation
{
    Очная,
    Заочная
}
public enum TermsOfStudy
{
    OneYearTenMonths = 22,      // 1 year and 10 months
    TwoYearsTenMonths = 34,     // 2 years and 10 months
    ThreeYearsTenMonths = 46    //3 years and 10 months
}
public enum LevelsOfEducation
{
    Основное_общее,      // 9 classes
    Среднее_общее  // 11 classes
}
