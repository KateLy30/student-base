
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
    Text,           // текст

    [Description("Число")]
    Number,         // Число

    [Description("Дата")]
    Date,           // Дата

    [Description("Да/Нет")]
    Boolean,        // Да/Нет

    [Description("Email")]
    Email,          // Email

    [Description("Телефон")]
    Phone,          // Телефон
}

public enum EntitiesPicker
{
    [Description("Программа обучения")]
    Program,

    [Description("Группа")]
    Group,

    [Description("Студент")]
    Student,

    [Description("Квитанция")]
    Payment,

    [Description("Перевод")]
    Transfer
}

public enum StudentFieldsName
{
    [Description("ФИО")]
    Name,

    [Description("Номер телефона")]
    Phone,

    [Description("Дата рождения")]
    DateOfBirth,

    [Description("Дата поступления")]
    DateOfReceipt,

    [Description("Уровень образования")]
    EducationLevel,

    [Description("Форма обучения")]
    FormOfEducation,

    [Description("Продолжительность обучения")]
    DurationTraining,

    [Description("Статус")]
    Status,

    [Description("Группа")]
    Group
}
