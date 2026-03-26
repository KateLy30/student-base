
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
public enum PaymentType
{
    [Description("Наличные")]
    Cash,

    [Description("Безналичные")]
    NonCash
}
public enum FieldType
{
    Text = 0,           // Однострочный текст
    MultilineText = 1,  // Многострочный текст
    Number = 2,         // Число
    Date = 3,           // Дата
    Boolean = 4,        // Да/Нет
    Email = 5,          // Email
    Phone = 6,          // Телефон
    Enumeration = 7,    // Выпадающий список
}
