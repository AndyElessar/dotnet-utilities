namespace Utilities;

public static partial class UtilitiesExtensions
{
    extension<T>(T) where T : class
    {
        public static bool operator true(T? val) => val is not null;
        public static bool operator false(T? val) => val is null;
    }

    extension<T>(T?) where T : struct
    {
        public static bool operator true(T? val) => val.HasValue;
        public static bool operator false(T? val) => !val.HasValue;
    }

    extension(bool?)
    {
        public static bool operator true(bool? val) => val.HasValue && val.Value;
        public static bool operator false(bool? val) => !val.HasValue || !val.Value;
    }
}
