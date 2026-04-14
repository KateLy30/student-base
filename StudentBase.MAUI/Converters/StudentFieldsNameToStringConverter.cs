using StudentBase.MAUI.ViewModels;
using System.Globalization;

namespace StudentBase.MAUI.Converters;

public class StudentFieldsNameToStringConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is FieldOption field)
            return field.DisplayName;
        return string.Empty;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        throw new NotImplementedException();
}
