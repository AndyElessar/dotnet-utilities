using System.ComponentModel;
using System.Globalization;
using System.Text;

namespace Utilities.ROC;

/// <summary>
/// 民國日期時間
/// </summary>
/// <remarks>
/// 從西元 DateTime 建立民國日期時間
/// </remarks>
/// <param name="ceVal">西元日期時間</param>
[TypeConverter(typeof(RocDateTimeTypeConverter))]
public readonly struct RocDateTime(DateTime ceVal) :
    IComparable,
    IComparable<RocDateTime>,
    IEquatable<RocDateTime>,
    IFormattable,
    IParsable<RocDateTime>,
    ISpanFormattable,
    ISpanParsable<RocDateTime>,
    IUtf8SpanFormattable
{
    private readonly DateTime _ceVal = ceVal;

    #region Constructors

    /// <summary>
    /// 從民國年月日建立民國日期時間
    /// </summary>
    public RocDateTime(int year, int month, int day)
        : this(new DateTime(year + RocExtensions.RocYearOffset, month, day)) { }

    /// <summary>
    /// 從民國年月日時分秒建立民國日期時間
    /// </summary>
    public RocDateTime(int year, int month, int day, int hour, int minute, int second)
        : this(new DateTime(year + RocExtensions.RocYearOffset, month, day, hour, minute, second)) { }

    /// <summary>
    /// 從民國年月日時分秒毫秒建立民國日期時間
    /// </summary>
    public RocDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond)
        : this(new DateTime(year + RocExtensions.RocYearOffset, month, day, hour, minute, second, millisecond)) { }

    /// <summary>
    /// 從民國年月日時分秒毫秒微秒建立民國日期時間
    /// </summary>
    public RocDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond, int microsecond)
        : this(new DateTime(year + RocExtensions.RocYearOffset, month, day, hour, minute, second, millisecond, microsecond)) { }

    /// <summary>
    /// 從 Ticks 建立民國日期時間
    /// </summary>
    public RocDateTime(long ticks)
        : this(new DateTime(ticks)) { }

    /// <summary>
    /// 從 Ticks 和 DateTimeKind 建立民國日期時間
    /// </summary>
    public RocDateTime(long ticks, DateTimeKind kind)
        : this(new DateTime(ticks, kind)) { }

    #endregion

    #region Static Properties

    /// <summary>
    /// 取得目前的民國日期時間
    /// </summary>
    public static RocDateTime Now => new(DateTime.Now);

    /// <summary>
    /// 取得目前的民國日期時間 (UTC)
    /// </summary>
    public static RocDateTime UtcNow => new(DateTime.UtcNow);

    /// <summary>
    /// 取得今天的民國日期
    /// </summary>
    public static RocDateTime Today => new(DateTime.Today);

    /// <summary>
    /// 取得民國日期時間的最小值
    /// </summary>
    public static RocDateTime MinValue => new(DateTime.MinValue);

    /// <summary>
    /// 取得民國日期時間的最大值
    /// </summary>
    public static RocDateTime MaxValue => new(DateTime.MaxValue);

    /// <summary>
    /// 取得 Unix Epoch 的民國日期時間
    /// </summary>
    public static RocDateTime UnixEpoch => new(DateTime.UnixEpoch);

    #endregion

    #region Instance Properties

    /// <summary>
    /// 取得民國年
    /// </summary>
    public int Year => _ceVal.Year - RocExtensions.RocYearOffset;

    /// <summary>
    /// 取得月
    /// </summary>
    public int Month => _ceVal.Month;

    /// <summary>
    /// 取得日
    /// </summary>
    public int Day => _ceVal.Day;

    /// <summary>
    /// 取得時
    /// </summary>
    public int Hour => _ceVal.Hour;

    /// <summary>
    /// 取得分
    /// </summary>
    public int Minute => _ceVal.Minute;

    /// <summary>
    /// 取得秒
    /// </summary>
    public int Second => _ceVal.Second;

    /// <summary>
    /// 取得毫秒
    /// </summary>
    public int Millisecond => _ceVal.Millisecond;

    /// <summary>
    /// 取得微秒
    /// </summary>
    public int Microsecond => _ceVal.Microsecond;

    /// <summary>
    /// 取得奈秒
    /// </summary>
    public int Nanosecond => _ceVal.Nanosecond;

    /// <summary>
    /// 取得 Ticks
    /// </summary>
    public long Ticks => _ceVal.Ticks;

    /// <summary>
    /// 取得星期幾
    /// </summary>
    public DayOfWeek DayOfWeek => _ceVal.DayOfWeek;

    /// <summary>
    /// 取得一年中的第幾天
    /// </summary>
    public int DayOfYear => _ceVal.DayOfYear;

    /// <summary>
    /// 取得時間的種類 (Local, Utc, Unspecified)
    /// </summary>
    public DateTimeKind Kind => _ceVal.Kind;

    /// <summary>
    /// 取得日期部分 (時間為 00:00:00)
    /// </summary>
    public RocDateTime Date => new(_ceVal.Date);

    /// <summary>
    /// 取得時間部分
    /// </summary>
    public TimeSpan TimeOfDay => _ceVal.TimeOfDay;

    #endregion

    #region Add Methods

    /// <summary>
    /// 加入指定的民國年數
    /// </summary>
    public RocDateTime AddYears(int value) => new(_ceVal.AddYears(value));

    /// <summary>
    /// 加入指定的月數
    /// </summary>
    public RocDateTime AddMonths(int months) => new(_ceVal.AddMonths(months));

    /// <summary>
    /// 加入指定的天數
    /// </summary>
    public RocDateTime AddDays(double value) => new(_ceVal.AddDays(value));

    /// <summary>
    /// 加入指定的小時數
    /// </summary>
    public RocDateTime AddHours(double value) => new(_ceVal.AddHours(value));

    /// <summary>
    /// 加入指定的分鐘數
    /// </summary>
    public RocDateTime AddMinutes(double value) => new(_ceVal.AddMinutes(value));

    /// <summary>
    /// 加入指定的秒數
    /// </summary>
    public RocDateTime AddSeconds(double value) => new(_ceVal.AddSeconds(value));

    /// <summary>
    /// 加入指定的毫秒數
    /// </summary>
    public RocDateTime AddMilliseconds(double value) => new(_ceVal.AddMilliseconds(value));

    /// <summary>
    /// 加入指定的微秒數
    /// </summary>
    public RocDateTime AddMicroseconds(double value) => new(_ceVal.AddMicroseconds(value));

    /// <summary>
    /// 加入指定的 Ticks
    /// </summary>
    public RocDateTime AddTicks(long value) => new(_ceVal.AddTicks(value));

    /// <summary>
    /// 加入指定的 TimeSpan
    /// </summary>
    public RocDateTime Add(TimeSpan value) => new(_ceVal.Add(value));

    /// <summary>
    /// 減去指定的 TimeSpan
    /// </summary>
    public RocDateTime Subtract(TimeSpan value) => new(_ceVal.Subtract(value));

    /// <summary>
    /// 計算與另一個民國日期時間的差距
    /// </summary>
    public TimeSpan Subtract(RocDateTime value) => _ceVal.Subtract(value._ceVal);

    #endregion

    #region Conversion Methods

    /// <summary>
    /// 轉換為西元 DateTime
    /// </summary>
    public DateTime ToDateTime() => _ceVal;

    /// <summary>
    /// 從西元 DateTime 轉換為民國日期時間
    /// </summary>
    public static RocDateTime FromDateTime(DateTime dateTime) => new(dateTime);

    /// <summary>
    /// 轉換為 DateOnly
    /// </summary>
    public DateOnly ToDateOnly() => DateOnly.FromDateTime(_ceVal);

    /// <summary>
    /// 轉換為 TimeOnly
    /// </summary>
    public TimeOnly ToTimeOnly() => TimeOnly.FromDateTime(_ceVal);

    /// <summary>
    /// 轉換為本地時間
    /// </summary>
    public RocDateTime ToLocalTime() => new(_ceVal.ToLocalTime());

    /// <summary>
    /// 轉換為 UTC 時間
    /// </summary>
    public RocDateTime ToUniversalTime() => new(_ceVal.ToUniversalTime());

    /// <summary>
    /// 轉換為 OLE Automation 日期
    /// </summary>
    public double ToOADate() => _ceVal.ToOADate();

    /// <summary>
    /// 從 OLE Automation 日期轉換
    /// </summary>
    public static RocDateTime FromOADate(double d) => new(DateTime.FromOADate(d));

    /// <summary>
    /// 轉換為二進位格式
    /// </summary>
    public long ToBinary() => _ceVal.ToBinary();

    /// <summary>
    /// 從二進位格式轉換
    /// </summary>
    public static RocDateTime FromBinary(long dateData) => new(DateTime.FromBinary(dateData));

    /// <summary>
    /// 轉換為檔案時間
    /// </summary>
    public long ToFileTime() => _ceVal.ToFileTime();

    /// <summary>
    /// 從檔案時間轉換
    /// </summary>
    public static RocDateTime FromFileTime(long fileTime) => new(DateTime.FromFileTime(fileTime));

    /// <summary>
    /// 轉換為 UTC 檔案時間
    /// </summary>
    public long ToFileTimeUtc() => _ceVal.ToFileTimeUtc();

    /// <summary>
    /// 從 UTC 檔案時間轉換
    /// </summary>
    public static RocDateTime FromFileTimeUtc(long fileTime) => new(DateTime.FromFileTimeUtc(fileTime));

    #endregion

    #region Parse Methods

    /// <summary>
    /// 從民國日期字串解析 (格式: yyy/MM/dd 或 yyy/MM/dd HH:mm:ss)
    /// </summary>
    public static RocDateTime Parse(string s)
    {
        ArgumentNullException.ThrowIfNull(s);
        return ParseInternal(s) ?? throw new FormatException($"無法解析民國日期時間: {s}");
    }

    /// <summary>
    /// 嘗試從民國日期字串解析
    /// </summary>
    public static bool TryParse(string? s, out RocDateTime result)
    {
        if(string.IsNullOrWhiteSpace(s))
        {
            result = default;
            return false;
        }

        var parsed = ParseInternal(s);
        if(parsed.HasValue)
        {
            result = parsed.Value;
            return true;
        }

        result = default;
        return false;
    }

    /// <summary>
    /// 使用指定格式從民國日期字串解析
    /// </summary>
    public static RocDateTime ParseExact(string s, string format, IFormatProvider? provider = null)
    {
        ArgumentNullException.ThrowIfNull(s);
        ArgumentNullException.ThrowIfNull(format);

        var dt = DateTime.ParseExact(s, format, provider ?? CultureInfo.InvariantCulture);
        // 假設解析出的年份是民國年，需要轉換為西元年
        return new RocDateTime(dt.AddYears(RocExtensions.RocYearOffset));
    }

    /// <summary>
    /// 嘗試使用指定格式從民國日期字串解析
    /// </summary>
    public static bool TryParseExact(string? s, string? format, IFormatProvider? provider, DateTimeStyles style, out RocDateTime result)
    {
        if(DateTime.TryParseExact(s, format, provider ?? CultureInfo.InvariantCulture, style, out var dt))
        {
            result = new RocDateTime(dt.AddYears(RocExtensions.RocYearOffset));
            return true;
        }

        result = default;
        return false;
    }

    #region IParsable<RocDateTime> Implementation

    /// <summary>
    /// 從字串解析民國日期時間 (IParsable 介面實作)
    /// </summary>
    static RocDateTime IParsable<RocDateTime>.Parse(string s, IFormatProvider? provider) =>
        Parse(s);

    /// <summary>
    /// 嘗試從字串解析民國日期時間 (IParsable 介面實作)
    /// </summary>
    static bool IParsable<RocDateTime>.TryParse(string? s, IFormatProvider? provider, out RocDateTime result) =>
        TryParse(s, out result);

    #endregion

    #region ISpanParsable<RocDateTime> Implementation

    /// <summary>
    /// 從 ReadOnlySpan&lt;char&gt; 解析民國日期時間
    /// </summary>
    public static RocDateTime Parse(ReadOnlySpan<char> s, IFormatProvider? provider = null)
    {
        var str = s.ToString();
        return ParseInternal(str) ?? throw new FormatException($"無法解析民國日期時間: {str}");
    }

    /// <summary>
    /// 嘗試從 ReadOnlySpan&lt;char&gt; 解析民國日期時間
    /// </summary>
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out RocDateTime result)
    {
        if(s.IsEmpty)
        {
            result = default;
            return false;
        }

        var parsed = ParseInternal(s.ToString());
        if(parsed.HasValue)
        {
            result = parsed.Value;
            return true;
        }

        result = default;
        return false;
    }

    /// <summary>
    /// 嘗試從 ReadOnlySpan&lt;char&gt; 解析民國日期時間
    /// </summary>
    public static bool TryParse(ReadOnlySpan<char> s, out RocDateTime result) =>
        TryParse(s, null, out result);

    #endregion

    // 常見的民國日期格式
    private static readonly string[] s_formats =
    [
        "yyy年MM月dd日 HH時mm分ss秒",
        "yyy年MM月dd日",
        "yyy/MM/dd HH:mm:ss.fffffff",
        "yyy/MM/dd HH:mm:ss.ffffff",
        "yyy/MM/dd HH:mm:ss.fffff",
        "yyy/MM/dd HH:mm:ss.ffff",
        "yyy/MM/dd HH:mm:ss.fff",
        "yyy/MM/dd HH:mm:ss.ff",
        "yyy/MM/dd HH:mm:ss.f",
        "yyy/MM/dd HH:mm:ss",
        "yyy/MM/dd HH:mm",
        "yyy/MM/dd",
        "yyy-MM-dd HH:mm:ss",
        "yyy-MM-dd",
        "yyyMMdd",
        "yyyMMddHHmmss"
    ];
    private static RocDateTime? ParseInternal(string s)
    {
        foreach(var format in s_formats)
        {
            if(DateTime.TryParseExact(s, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dt))
            {
                return new RocDateTime(dt.AddYears(RocExtensions.RocYearOffset));
            }
        }

        return null;
    }

    #endregion

    #region Static Utility Methods

    /// <summary>
    /// 判斷指定的民國年是否為閏年
    /// </summary>
    public static bool IsLeapYear(int year) => DateTime.IsLeapYear(year + RocExtensions.RocYearOffset);

    /// <summary>
    /// 取得指定民國年月的天數
    /// </summary>
    public static int DaysInMonth(int year, int month) => DateTime.DaysInMonth(year + RocExtensions.RocYearOffset, month);

    /// <summary>
    /// 指定種類建立新的民國日期時間
    /// </summary>
    public static RocDateTime SpecifyKind(RocDateTime value, DateTimeKind kind) =>
        new(DateTime.SpecifyKind(value._ceVal, kind));

    /// <summary>
    /// 比較兩個民國日期時間
    /// </summary>
    public static int Compare(RocDateTime t1, RocDateTime t2) => DateTime.Compare(t1._ceVal, t2._ceVal);

    /// <summary>
    /// 判斷兩個民國日期時間是否相等
    /// </summary>
    public static bool Equals(RocDateTime t1, RocDateTime t2) => t1._ceVal == t2._ceVal;

    #endregion

    #region ToString Methods

    /// <summary>
    /// 轉換為民國日期時間字串
    /// </summary>
    public override string ToString() => $"{Year:D3}/{Month:D2}/{Day:D2} {Hour:D2}:{Minute:D2}:{Second:D2}";

    /// <summary>
    /// 使用指定格式轉換為字串 (年份會顯示為民國年)
    /// </summary>
    public string ToString(string? format) => ToString(format, null);

    /// <summary>
    /// 使用指定格式和格式提供者轉換為字串
    /// </summary>
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        if(string.IsNullOrEmpty(format))
            return ToString();

        // 建立民國年的 DateTime (用於格式化，但年份是民國年)
        // 由於 DateTime 無法表示小於 1 的年份，我們需要特殊處理
        var rocYear = Year;
        if(rocYear is >= 1 and <= 9999)
        {
            try
            {
                var rocDateTime = new DateTime(rocYear, Month, Day, Hour, Minute, Second, Millisecond, Microsecond);
                return rocDateTime.ToString(format, formatProvider);
            }
            catch
            {
                // 如果年份無效，使用手動格式化
            }
        }

        // 手動處理格式字串中的年份
        return FormatManually(format);
    }

    private string FormatManually(string format)
    {
        // 簡單的格式化處理
        var result = format
            .Replace("yyyy", Year.ToString("D4"))
            .Replace("yyy", Year.ToString("D3"))
            .Replace("yy", (Year % 100).ToString("D2"))
            .Replace("y", (Year % 100).ToString())
            .Replace("MMMM", _ceVal.ToString("MMMM"))
            .Replace("MMM", _ceVal.ToString("MMM"))
            .Replace("MM", Month.ToString("D2"))
            .Replace("M", Month.ToString())
            .Replace("dddd", _ceVal.ToString("dddd"))
            .Replace("ddd", _ceVal.ToString("ddd"))
            .Replace("dd", Day.ToString("D2"))
            .Replace("d", Day.ToString())
            .Replace("HH", Hour.ToString("D2"))
            .Replace("H", Hour.ToString())
            .Replace("hh", (Hour % 12 == 0 ? 12 : Hour % 12).ToString("D2"))
            .Replace("h", (Hour % 12 == 0 ? 12 : Hour % 12).ToString())
            .Replace("mm", Minute.ToString("D2"))
            .Replace("m", Minute.ToString())
            .Replace("ss", Second.ToString("D2"))
            .Replace("s", Second.ToString())
            .Replace("fffffff", _ceVal.ToString("fffffff"))
            .Replace("ffffff", _ceVal.ToString("ffffff"))
            .Replace("fffff", _ceVal.ToString("fffff"))
            .Replace("ffff", _ceVal.ToString("ffff"))
            .Replace("fff", _ceVal.ToString("fff"))
            .Replace("ff", _ceVal.ToString("ff"))
            .Replace("f", _ceVal.ToString("f"))
            .Replace("tt", Hour >= 12 ? "PM" : "AM")
            .Replace("t", Hour >= 12 ? "P" : "A");

        return result;
    }

    /// <summary>
    /// 轉換為長日期字串
    /// </summary>
    public string ToLongDateString() => $"民國{Year}年{Month}月{Day}日";

    /// <summary>
    /// 轉換為短日期字串
    /// </summary>
    public string ToShortDateString() => $"{Year:D3}/{Month:D2}/{Day:D2}";

    /// <summary>
    /// 轉換為長時間字串
    /// </summary>
    public string ToLongTimeString() => _ceVal.ToLongTimeString();

    /// <summary>
    /// 轉換為短時間字串
    /// </summary>
    public string ToShortTimeString() => _ceVal.ToShortTimeString();

    #endregion

    #region ISpanFormattable Implementation

    /// <summary>
    /// 嘗試將民國日期時間格式化到 Span&lt;char&gt;
    /// </summary>
    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        var formatted = format.IsEmpty
            ? ToString()
            : ToString(format.ToString(), provider);

        if(formatted.Length <= destination.Length)
        {
            formatted.AsSpan().CopyTo(destination);
            charsWritten = formatted.Length;
            return true;
        }

        charsWritten = 0;
        return false;
    }

    #endregion

    #region IUtf8SpanFormattable Implementation

    /// <summary>
    /// 嘗試將民國日期時間格式化到 UTF-8 Span&lt;byte&gt;
    /// </summary>
    public bool TryFormat(Span<byte> utf8Destination, out int bytesWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        var formatted = format.IsEmpty
            ? ToString()
            : ToString(format.ToString(), provider);

        var byteCount = Encoding.UTF8.GetByteCount(formatted);
        if(byteCount <= utf8Destination.Length)
        {
            bytesWritten = Encoding.UTF8.GetBytes(formatted, utf8Destination);
            return true;
        }

        bytesWritten = 0;
        return false;
    }

    #endregion

    #region Comparison & Equality

    /// <inheritdoc/>
    public int CompareTo(RocDateTime other) => _ceVal.CompareTo(other._ceVal);

    /// <inheritdoc/>
    public int CompareTo(object? obj)
    {
        if(obj is null) return 1;
        if(obj is RocDateTime other) return CompareTo(other);
        throw new ArgumentException($"物件必須是 {nameof(RocDateTime)} 類型", nameof(obj));
    }

    /// <inheritdoc/>
    public bool Equals(RocDateTime other) => _ceVal == other._ceVal;

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is RocDateTime other && Equals(other);

    /// <inheritdoc/>
    public override int GetHashCode() => _ceVal.GetHashCode();

    #endregion

    #region Operators

    /// <summary>
    /// 判斷兩個民國日期時間是否相等
    /// </summary>
    public static bool operator ==(RocDateTime left, RocDateTime right) => left.Equals(right);

    /// <summary>
    /// 判斷兩個民國日期時間是否不相等
    /// </summary>
    public static bool operator !=(RocDateTime left, RocDateTime right) => !left.Equals(right);

    /// <summary>
    /// 判斷左邊的民國日期時間是否小於右邊
    /// </summary>
    public static bool operator <(RocDateTime left, RocDateTime right) => left._ceVal < right._ceVal;

    /// <summary>
    /// 判斷左邊的民國日期時間是否大於右邊
    /// </summary>
    public static bool operator >(RocDateTime left, RocDateTime right) => left._ceVal > right._ceVal;

    /// <summary>
    /// 判斷左邊的民國日期時間是否小於或等於右邊
    /// </summary>
    public static bool operator <=(RocDateTime left, RocDateTime right) => left._ceVal <= right._ceVal;

    /// <summary>
    /// 判斷左邊的民國日期時間是否大於或等於右邊
    /// </summary>
    public static bool operator >=(RocDateTime left, RocDateTime right) => left._ceVal >= right._ceVal;

    /// <summary>
    /// 加入 TimeSpan
    /// </summary>
    public static RocDateTime operator +(RocDateTime d, TimeSpan t) => new(d._ceVal + t);

    /// <summary>
    /// 減去 TimeSpan
    /// </summary>
    public static RocDateTime operator -(RocDateTime d, TimeSpan t) => new(d._ceVal - t);

    /// <summary>
    /// 計算兩個民國日期時間的差距
    /// </summary>
    public static TimeSpan operator -(RocDateTime d1, RocDateTime d2) => d1._ceVal - d2._ceVal;

    #endregion

    #region Implicit/Explicit Conversions

    /// <summary>
    /// 從 DateTime 顯式轉換為 RocDateTime
    /// </summary>
    public static explicit operator RocDateTime(DateTime dateTime) => new(dateTime);

    /// <summary>
    /// 從 RocDateTime 顯式轉換為 DateTime
    /// </summary>
    public static explicit operator DateTime(RocDateTime rocDateTime) => rocDateTime._ceVal;

    #endregion

    #region Deconstruct

    /// <summary>
    /// 解構為民國年、月、日
    /// </summary>
    public void Deconstruct(out int year, out int month, out int day)
    {
        year = Year;
        month = Month;
        day = Day;
    }

    /// <summary>
    /// 解構為民國年、月、日、時、分、秒
    /// </summary>
    public void Deconstruct(out int year, out int month, out int day, out int hour, out int minute, out int second)
    {
        year = Year;
        month = Month;
        day = Day;
        hour = Hour;
        minute = Minute;
        second = Second;
    }

    #endregion
}
