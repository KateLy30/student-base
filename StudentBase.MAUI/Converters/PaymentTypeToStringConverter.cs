using StudentBase.Domain;
using StudentBase.Domain.Extensions;
using System.Globalization;

namespace StudentBase.MAUI.Converters;

public class PaymentTypeToStringConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is PaymentType type)
            return type.ToDisplayString();

        return value?.ToString() ?? string.Empty;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => 
        throw new NotImplementedException();
}
