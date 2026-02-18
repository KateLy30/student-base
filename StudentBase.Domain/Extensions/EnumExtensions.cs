using System.ComponentModel;
using System.Reflection;

namespace StudentBase.Domain.Extensions;

public static class EnumExtensions
{
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
