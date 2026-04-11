using System.ComponentModel;
using System.Reflection;

namespace StudentBase.Domain.Extensions;

public static class EnumExtensions
{
    /// <summary>
    /// Достает из значения Enum-списка его описание
    /// </summary>
    /// <param name="value">Enum-значение</param>
    /// <returns>Форматированная строка: "<атрибут_описания>"</returns>
    public static string ToDisplayString(this Enum value)
    {
        if (value == null)
            return string.Empty;

        var fi = value.GetType().GetField(value.ToString());
        if (fi != null)
        {
            var attr = fi.GetCustomAttribute<DescriptionAttribute>();
            if (attr != null)
                return attr.Description;
        }
        return value.ToString(); // fallback, если нет атрибута
    }
}
// метод для отображения семестров
public static class SemesterExtensions
{
    /// <summary>
    /// Преобразует номер семестра в читаемый формат с годом и сезоном
    /// </summary>
    /// <param name="semesterNumber">Номер семестра (1, 2, 3...)</param>
    /// <param name="enrollmentDate">Дата поступления студента</param>
    /// <returns>Форматированная строка: "1 семестр 2024 (осень)"</returns>
    public static string ToSemesterDisplay(this int semesterNumber, DateTime enrollmentDate)
    {
        bool isAutumn = semesterNumber % 2 == 1;
        int year = enrollmentDate.Year + (semesterNumber - 1) / 2;
        if (!isAutumn) year++;

        return $"{semesterNumber} семестр {year} ({(isAutumn ? "осень" : "весна")})";
    }
}
