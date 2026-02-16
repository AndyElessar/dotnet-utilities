using Utilities.ROC;

namespace Blazor;

public static class RocDateInputConverter
{
    public static string ToInputDateValue(RocDateTime? value) =>
        value.HasValue ? value.Value.ToDateTime().ToString("yyyy-MM-dd") : string.Empty;

    public static bool TryParseInputDateValue(string? value, out RocDateTime? result)
    {
        if(string.IsNullOrWhiteSpace(value))
        {
            result = null;
            return true;
        }

        if(DateTime.TryParse(value, out var date))
        {
            result = new RocDateTime(date.Date);
            return true;
        }

        result = null;
        return false;
    }
}
