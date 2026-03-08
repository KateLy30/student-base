
using System.ComponentModel;

namespace StudentBase.Domain;
public enum StatusGroups
{
    [Description("Открыта")]
    Open,

    [Description("Закрыта")]
    Closed
}
public enum StatusStudents
{
    [Description("Обучается")]
    Studying,

    [Description("Выпустился")]
    Graduated
}
public enum StatusPrograms
{
    [Description("Актуальная программа")]
    CurrentProgram,

    [Description("Программа в архиве")]
    ProgramIsArchived
}
public enum FormsOfEducation
{
    [Description("Очная форма")]
    FullTime,

    [Description("Заочная форма")]
    Correspondence
}
public enum TermsOfStudy
{
    [Description("1 г. 10 мес. ")]
    OneYearTenMonths,   

    [Description("2 г. 10 мес.")]
    TwoYearsTenMonths,   

    [Description("3 г. 10 мес.")]
    ThreeYearsTenMonths
}
public enum LevelsOfEducation
{
    [Description("Основное общее образование (9 классов)")]
    BasicGeneralEducation,

    [Description("Среднее общее образование (11 классов)")]
    SecondaryGeneralEducation  
}
