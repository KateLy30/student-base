
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

/// <summary>
/// Тип поля (хранится как строка в БД или enum)
/// </summary>
public enum FieldType
{
    [Description("Текст")]
    Text = 0,           // текст

    [Description("Число")]
    Number = 1,         // Число

    [Description("Дата")]
    Date = 2,           // Дата

    [Description("Да/Нет")]
    Boolean = 3,        // Да/Нет

    [Description("Email")]
    Email = 4,          // Email

    [Description("Телефон")]
    Phone = 5,          // Телефон

    [Description("Список")]
    Enumeration = 6,    // Выпадающий список
}
