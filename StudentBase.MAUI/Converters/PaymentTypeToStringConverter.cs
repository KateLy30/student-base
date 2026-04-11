using StudentBase.Domain;
using StudentBase.Domain.Extensions;
using System.Globalization;

namespace StudentBase.MAUI.Converters;

public class PaymentTypeToStringConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is PaymentType paymentType && parameter is string paramString)
        {
            if (paramString == "Cash" && paymentType == PaymentType.Cash)
                return true;
            if (paramString == "NonCash" && paymentType == PaymentType.NonCash)
                return true;
        }
        return false;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool isChecked && isChecked && parameter is string paramString)
        {
            if (paramString == "Cash")
                return PaymentType.Cash;
            if (paramString == "NonCash")
                return PaymentType.NonCash;
        }
        return Binding.DoNothing;
    }
}
