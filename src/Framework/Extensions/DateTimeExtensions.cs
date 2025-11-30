using System.Globalization;

namespace Framework.Extensions;

public static class DateTimeExtensions
{
    public static string ToShamsi(this DateTime date)
    {
        var pc = new PersianCalendar();

        return $"{pc.GetYear(date):0000}/{pc.GetMonth(date):00}/{pc.GetDayOfMonth(date):00}";
    }
}
