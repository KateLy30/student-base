using StudentBase.Domain;
using StudentBase.Domain.Extensions;
using System.Globalization;

namespace StudentBase.MAUI.Converters;

public class StatusStudentsToStringConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is StatusStudents status)
            return status.ToDisplayString();

        return value?.ToString() ?? string.Empty;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        throw new NotImplementedException();
}
