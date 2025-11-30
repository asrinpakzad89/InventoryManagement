namespace Framework.Extensions;

public static class EnumExtensions
{
    public static int ToInt<TEnum>(this TEnum enumValue) where TEnum : Enum
    {
        return Convert.ToInt32(enumValue);
    }

    public static TEnum ToEnum<TEnum>(this int value) where TEnum : Enum
    {
        if (!Enum.IsDefined(typeof(TEnum), value))
            throw new ArgumentException($"مقدار {value} در Enum {typeof(TEnum).Name} تعریف نشده است.");

        return (TEnum)Enum.ToObject(typeof(TEnum), value);
    }

    public static List<TEnum> ToListByDisplayName<TEnum>() where TEnum : Enum
    {
        return Enum.GetValues(typeof(TEnum))
            .Cast<TEnum>()
            .OrderBy(e => e.GetDisplayName())
            .ToList();
    }
    public static string GetDisplayName<TEnum>(this TEnum value)
    {
        var memberInfo = typeof(TEnum).GetMember(value.ToString()).FirstOrDefault();
        if(memberInfo != null)
        {
            var displayAttribute = memberInfo.GetCustomAttribute<DisplayAttribute>();
            if (displayAttribute != null)
                return displayAttribute.Name ?? value.ToString();

        }
        return value.ToString();
    }
}
