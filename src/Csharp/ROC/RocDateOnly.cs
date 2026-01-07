using System.ComponentModel;
using System.Globalization;
using System.Text;

namespace Utilities.ROC;

/// <summary>
/// 民國日期結構，提供類似 <see cref="DateOnly"/> 的 API
/// </summary>
/// <remarks>
/// 從西元 DateOnly 建立民國日期
/// </remarks>
/// <param name="ceVal">西元日期</param>
[TypeConverter(typeof(RocDateOnlyTypeConverter))]
public readonly struct RocDateOnly(DateOnly ceVal) :
    IComparable,
    IComparable<RocDateOnly>,
    IEquatable<RocDateOnly>,
    IFormattable,
    IParsable<RocDateOnly>,
    ISpanFormattable,
    ISpanParsable<RocDateOnly>,
    IUtf8SpanFormattable
{
    private static readonly TaiwanCalendar s_calendar = new();
    private readonly DateOnly _ceVal = ceVal;

    #region Constructors

    /// <summary>
    /// 使用民國年、月、日建立民國日期
    /// </summary>
    /// <param name="rocYear">民國年 (1 = 民國元年 = 西元 1912 年)</param>
    /// <param name="month">月份 (1-12)</param>
    /// <param name="day">日期 (1-31，依月份而定)</param>
    public RocDateOnly(int rocYear, int month, int day)
        : this(DateOnly.FromDateTime(s_calendar.ToDateTime(rocYear, month, day, 0, 0, 0, 0))) { }

    #endregion

    #region Properties

    /// <summary>
    /// 取得民國年
    /// </summary>
    public int Year => s_calendar.GetYear(_ceVal.ToDateTime(default));

    /// <summary>
    /// 取得西元年
    /// </summary>
    public int CeYear => _ceVal.Year;

    /// <summary>
    /// 取得月份 (1-12)
    /// </summary>
    public int Month => _ceVal.Month;

    /// <summary>
    /// 取得日期 (1-31)
    /// </summary>
    public int Day => _ceVal.Day;

    /// <summary>
    /// 取得星期幾
    /// </summary>
    public DayOfWeek DayOfWeek => _ceVal.DayOfWeek;

    /// <summary>
    /// 取得一年中的第幾天 (1-366)
    /// </summary>
    public int DayOfYear => _ceVal.DayOfYear;

    /// <summary>
    /// 取得自西元 0001 年 1 月 1 日起的天數
    /// </summary>
    public int DayNumber => _ceVal.DayNumber;

    /// <summary>
    /// 取得對應的西元 <see cref="DateOnly"/>
    /// </summary>
    public DateOnly CeDateOnly => _ceVal;

    /// <summary>
    /// 取得最小值 (民國元年 1 月 1 日，即西元 1912 年 1 月 1 日)
    /// </summary>
    public static RocDateOnly MinValue => new(new DateOnly(1912, 1, 1));

    /// <summary>
    /// 取得最大值 (西元 9999 年 12 月 31 日)
    /// </summary>
    public static RocDateOnly MaxValue => new(DateOnly.MaxValue);

    #endregion

    #region Factory Methods

    /// <summary>
    /// 從 <see cref="DateTime"/> 建立民國日期
    /// </summary>
    /// <param name="dateTime">西元日期時間</param>
    /// <returns>民國日期</returns>
    public static RocDateOnly FromDateTime(DateTime dateTime) =>
        new(DateOnly.FromDateTime(dateTime));

    /// <summary>
    /// 從 <see cref="DateOnly"/> 建立民國日期
    /// </summary>
    /// <param name="dateOnly">西元日期</param>
    /// <returns>民國日期</returns>
    public static RocDateOnly FromDateOnly(DateOnly dateOnly) =>
        new(dateOnly);

    /// <summary>
    /// 從天數建立民國日期
    /// </summary>
    /// <param name="dayNumber">自西元 0001 年 1 月 1 日起的天數</param>
    /// <returns>民國日期</returns>
    public static RocDateOnly FromDayNumber(int dayNumber) =>
        new(DateOnly.FromDayNumber(dayNumber));

    /// <summary>
    /// 取得今天的民國日期
    /// </summary>
    public static RocDateOnly Today => FromDateTime(DateTime.Today);

    #endregion

    #region Add Methods

    /// <summary>
    /// 增加指定天數
    /// </summary>
    /// <param name="value">要增加的天數 (可為負數)</param>
    /// <returns>新的民國日期</returns>
    public RocDateOnly AddDays(int value) => new(_ceVal.AddDays(value));

    /// <summary>
    /// 增加指定月數
    /// </summary>
    /// <param name="value">要增加的月數 (可為負數)</param>
    /// <returns>新的民國日期</returns>
    public RocDateOnly AddMonths(int value) => new(_ceVal.AddMonths(value));

    /// <summary>
    /// 增加指定年數
    /// </summary>
    /// <param name="value">要增加的年數 (可為負數)</param>
    /// <returns>新的民國日期</returns>
    public RocDateOnly AddYears(int value) => new(_ceVal.AddYears(value));

    #endregion

    #region Conversion Methods

    /// <summary>
    /// 轉換為 <see cref="DateTime"/>
    /// </summary>
    /// <param name="time">時間部分</param>
    /// <returns>西元日期時間</returns>
    public DateTime ToDateTime(TimeOnly time) => _ceVal.ToDateTime(time);

    /// <summary>
    /// 轉換為 <see cref="DateTime"/>，並指定 <see cref="DateTimeKind"/>
    /// </summary>
    /// <param name="time">時間部分</param>
    /// <param name="kind">日期時間種類</param>
    /// <returns>西元日期時間</returns>
    public DateTime ToDateTime(TimeOnly time, DateTimeKind kind) =>
        _ceVal.ToDateTime(time, kind);

    /// <summary>
    /// 轉換為長日期字串
    /// </summary>
    /// <returns>長日期字串</returns>
    public string ToLongDateString() =>
        $"民國{Year}年{Month}月{Day}日";

    /// <summary>
    /// 轉換為短日期字串
    /// </summary>
    /// <returns>短日期字串 (格式: yyy/MM/dd)</returns>
    public string ToShortDateString() =>
        $"{Year:000}/{Month:00}/{Day:00}";

    #endregion

    #region Parse Methods

    /// <summary>
    /// 將字串轉換為民國日期
    /// </summary>
    /// <param name="s">要轉換的字串 (格式: yyy/MM/dd 或 yyyMMdd)</param>
    /// <returns>民國日期</returns>
    /// <exception cref="FormatException">字串格式不正確</exception>
    public static RocDateOnly Parse(string s)
    {
        ArgumentNullException.ThrowIfNull(s);
        return Parse(s.AsSpan());
    }

    /// <summary>
    /// 將字元範圍轉換為民國日期
    /// </summary>
    /// <param name="s">要轉換的字元範圍</param>
    /// <returns>民國日期</returns>
    /// <exception cref="FormatException">字串格式不正確</exception>
    public static RocDateOnly Parse(ReadOnlySpan<char> s)
    {
        if(TryParse(s, out var result))
            return result;
        throw new FormatException($"無法將 '{s}' 轉換為民國日期");
    }

    /// <summary>
    /// 將字串轉換為民國日期
    /// </summary>
    /// <param name="s">要轉換的字串</param>
    /// <param name="provider">格式提供者</param>
    /// <returns>民國日期</returns>
    public static RocDateOnly Parse(string s, IFormatProvider? provider)
    {
        ArgumentNullException.ThrowIfNull(s);
        return Parse(s.AsSpan(), provider);
    }

    /// <summary>
    /// 將字元範圍轉換為民國日期
    /// </summary>
    /// <param name="s">要轉換的字元範圍</param>
    /// <param name="provider">格式提供者</param>
    /// <returns>民國日期</returns>
    public static RocDateOnly Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        if(TryParse(s, provider, out var result))
            return result;
        throw new FormatException($"無法將 '{s}' 轉換為民國日期");
    }

    /// <summary>
    /// 使用指定格式將字串轉換為民國日期
    /// </summary>
    /// <param name="s">要轉換的字串</param>
    /// <param name="format">格式字串</param>
    /// <returns>民國日期</returns>
    public static RocDateOnly ParseExact(string s, string format)
    {
        ArgumentNullException.ThrowIfNull(s);
        ArgumentNullException.ThrowIfNull(format);
        return ParseExact(s.AsSpan(), format.AsSpan());
    }

    /// <summary>
    /// 使用指定格式將字元範圍轉換為民國日期
    /// </summary>
    /// <param name="s">要轉換的字元範圍</param>
    /// <param name="format">格式字串</param>
    /// <returns>民國日期</returns>
    public static RocDateOnly ParseExact(ReadOnlySpan<char> s, ReadOnlySpan<char> format)
    {
        if(TryParseExact(s, format, out var result))
            return result;
        throw new FormatException($"無法使用格式 '{format}' 將 '{s}' 轉換為民國日期");
    }

    /// <summary>
    /// 嘗試將字串轉換為民國日期
    /// </summary>
    /// <param name="s">要轉換的字串</param>
    /// <param name="result">轉換結果</param>
    /// <returns>是否轉換成功</returns>
    public static bool TryParse(string? s, out RocDateOnly result)
    {
        if(s is null)
        {
            result = default;
            return false;
        }
        return TryParse(s.AsSpan(), out result);
    }

    /// <summary>
    /// 嘗試將字元範圍轉換為民國日期
    /// </summary>
    /// <param name="s">要轉換的字元範圍</param>
    /// <param name="result">轉換結果</param>
    /// <returns>是否轉換成功</returns>
    public static bool TryParse(ReadOnlySpan<char> s, out RocDateOnly result)
    {
        return TryParse(s, null, out result);
    }

    /// <summary>
    /// 嘗試將字串轉換為民國日期
    /// </summary>
    /// <param name="s">要轉換的字串</param>
    /// <param name="provider">格式提供者</param>
    /// <param name="result">轉換結果</param>
    /// <returns>是否轉換成功</returns>
    public static bool TryParse(string? s, IFormatProvider? provider, out RocDateOnly result)
    {
        if(s is null)
        {
            result = default;
            return false;
        }
        return TryParse(s.AsSpan(), provider, out result);
    }

    /// <summary>
    /// 嘗試將字元範圍轉換為民國日期
    /// </summary>
    /// <param name="s">要轉換的字元範圍</param>
    /// <param name="provider">格式提供者</param>
    /// <param name="result">轉換結果</param>
    /// <returns>是否轉換成功</returns>
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out RocDateOnly result)
    {
        result = default;
        s = s.Trim();

        // 只接受民國格式: yyy/MM/dd, yyy-MM-dd, yyyMMdd
        return TryParseRocFormat(s, out result);
    }

    /// <summary>
    /// 嘗試使用指定格式將字串轉換為民國日期
    /// </summary>
    /// <param name="s">要轉換的字串</param>
    /// <param name="format">格式字串</param>
    /// <param name="result">轉換結果</param>
    /// <returns>是否轉換成功</returns>
    public static bool TryParseExact(string? s, string? format, out RocDateOnly result)
    {
        if(s is null || format is null)
        {
            result = default;
            return false;
        }
        return TryParseExact(s.AsSpan(), format.AsSpan(), out result);
    }

    /// <summary>
    /// 嘗試使用指定格式將字元範圍轉換為民國日期
    /// </summary>
    /// <param name="s">要轉換的字元範圍</param>
    /// <param name="format">格式字串</param>
    /// <param name="result">轉換結果</param>
    /// <returns>是否轉換成功</returns>
    public static bool TryParseExact(ReadOnlySpan<char> s, ReadOnlySpan<char> format, out RocDateOnly result)
    {
        return TryParseExact(s, format, null, DateTimeStyles.None, out result);
    }

    /// <summary>
    /// 嘗試使用指定格式將字元範圍轉換為民國日期
    /// </summary>
    /// <param name="s">要轉換的字元範圍</param>
    /// <param name="format">格式字串</param>
    /// <param name="provider">格式提供者</param>
    /// <param name="style">日期時間樣式</param>
    /// <param name="result">轉換結果</param>
    /// <returns>是否轉換成功</returns>
    public static bool TryParseExact(ReadOnlySpan<char> s, ReadOnlySpan<char> format,
        IFormatProvider? provider, DateTimeStyles style, out RocDateOnly result)
    {
        result = default;
        s = s.Trim();

        // 如果格式包含民國年份格式 (yyy)，嘗試民國格式解析
        if(format.Contains("yyy".AsSpan(), StringComparison.Ordinal))
        {
            return TryParseRocFormat(s, out result);
        }

        // 使用西元格式解析
        if(DateOnly.TryParseExact(s, format, provider, style, out var ceDate))
        {
            result = new RocDateOnly(ceDate);
            return true;
        }

        return false;
    }

    private static bool TryParseRocFormat(ReadOnlySpan<char> s, out RocDateOnly result)
    {
        result = default;

        // 移除可能的分隔符號並檢查長度
        Span<char> digits = stackalloc char[8];
        int digitCount = 0;

        foreach(var c in s)
        {
            if(char.IsDigit(c))
            {
                if(digitCount >= 8) return false;
                digits[digitCount++] = c;
            }
        }

        // 預期格式: yyyMMdd (7 位數民國格式)
        if(digitCount == 7)
        {
            // 民國年格式: yyyMMdd
            if(!int.TryParse(digits[..3], out int rocYear)) return false;
            if(!int.TryParse(digits.Slice(3, 2), out int month)) return false;
            if(!int.TryParse(digits.Slice(5, 2), out int day)) return false;

            if(month < 1 || month > 12 || day < 1 || day > 31)
                return false;

            try
            {
                result = new RocDateOnly(rocYear, month, day);
                return true;
            }
            catch
            {
                return false;
            }
        }

        return false;
    }

    #endregion

    #region Formatting

    /// <summary>
    /// 轉換為字串 (預設格式: yyy/MM/dd)
    /// </summary>
    /// <returns>民國日期字串</returns>
    public override string ToString() => ToShortDateString();

    /// <summary>
    /// 使用指定格式轉換為字串
    /// </summary>
    /// <param name="format">格式字串。支援的格式: yyy (民國年)、yyyy (西元年)、MM (月)、dd (日)</param>
    /// <returns>格式化的日期字串</returns>
    public string ToString(string? format)
    {
        return ToString(format, null);
    }

    /// <summary>
    /// 使用指定格式和提供者轉換為字串
    /// </summary>
    /// <param name="format">格式字串</param>
    /// <param name="formatProvider">格式提供者</param>
    /// <returns>格式化的日期字串</returns>
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        if(string.IsNullOrEmpty(format))
            return ToShortDateString();

        // 處理年份格式 (從最長到最短)
        // yyyy => 4位數民國年，補零 (例: 0113)
        // yyy => 3位數民國年，補零 (例: 113)
        // yy => 2位數，取後兩位 (例: 13)
        // y => 1位數，取個位數 (例: 3)
        var result = format
            .Replace("yyyy", Year.ToString("0000"))
            .Replace("yyy", Year.ToString("000"))
            .Replace("yy", (Year % 100).ToString("00"))
            .Replace("y", (Year % 10).ToString())
            .Replace("MM", Month.ToString("00"))
            .Replace("dd", Day.ToString("00"))
            .Replace("M", Month.ToString())
            .Replace("d", Day.ToString());

        return result;
    }

    /// <summary>
    /// 嘗試格式化為字元範圍
    /// </summary>
    /// <param name="destination">目標字元範圍</param>
    /// <param name="charsWritten">寫入的字元數</param>
    /// <param name="format">格式字串</param>
    /// <param name="provider">格式提供者</param>
    /// <returns>是否格式化成功</returns>
    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        var str = format.IsEmpty
            ? ToString()
            : ToString(format.ToString(), provider);

        if(str.Length <= destination.Length)
        {
            str.AsSpan().CopyTo(destination);
            charsWritten = str.Length;
            return true;
        }
        charsWritten = 0;
        return false;
    }

    /// <summary>
    /// 嘗試格式化為 UTF-8 位元組範圍
    /// </summary>
    /// <param name="utf8Destination">目標 UTF-8 位元組範圍</param>
    /// <param name="bytesWritten">寫入的位元組數</param>
    /// <param name="format">格式字串</param>
    /// <param name="provider">格式提供者</param>
    /// <returns>是否格式化成功</returns>
    public bool TryFormat(Span<byte> utf8Destination, out int bytesWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        var str = format.IsEmpty
            ? ToString()
            : ToString(format.ToString(), provider);

        var byteCount = Encoding.UTF8.GetByteCount(str);
        if(byteCount <= utf8Destination.Length)
        {
            bytesWritten = Encoding.UTF8.GetBytes(str, utf8Destination);
            return true;
        }
        bytesWritten = 0;
        return false;
    }

    #endregion

    #region Deconstruct

    /// <summary>
    /// 將民國日期解構為年、月、日
    /// </summary>
    /// <param name="year">民國年</param>
    /// <param name="month">月份</param>
    /// <param name="day">日期</param>
    public void Deconstruct(out int year, out int month, out int day)
    {
        year = Year;
        month = Month;
        day = Day;
    }

    #endregion

    #region Comparison

    /// <summary>
    /// 比較兩個民國日期
    /// </summary>
    /// <param name="other">要比較的民國日期</param>
    /// <returns>比較結果</returns>
    public int CompareTo(RocDateOnly other) => _ceVal.CompareTo(other._ceVal);

    /// <summary>
    /// 比較民國日期與物件
    /// </summary>
    /// <param name="obj">要比較的物件</param>
    /// <returns>比較結果</returns>
    public int CompareTo(object? obj)
    {
        if(obj is null) return 1;
        if(obj is RocDateOnly other) return CompareTo(other);
        throw new ArgumentException($"物件必須是 {nameof(RocDateOnly)} 類型", nameof(obj));
    }

    #endregion

    #region Equality

    /// <summary>
    /// 判斷兩個民國日期是否相等
    /// </summary>
    /// <param name="other">要比較的民國日期</param>
    /// <returns>是否相等</returns>
    public bool Equals(RocDateOnly other) => _ceVal.Equals(other._ceVal);

    /// <summary>
    /// 判斷民國日期與物件是否相等
    /// </summary>
    /// <param name="obj">要比較的物件</param>
    /// <returns>是否相等</returns>
    public override bool Equals(object? obj) =>
        obj is RocDateOnly other && Equals(other);

    /// <summary>
    /// 取得雜湊碼
    /// </summary>
    /// <returns>雜湊碼</returns>
    public override int GetHashCode() => _ceVal.GetHashCode();

    #endregion

    #region Operators

    /// <summary>
    /// 判斷兩個民國日期是否相等
    /// </summary>
    public static bool operator ==(RocDateOnly left, RocDateOnly right) => left.Equals(right);

    /// <summary>
    /// 判斷兩個民國日期是否不相等
    /// </summary>
    public static bool operator !=(RocDateOnly left, RocDateOnly right) => !left.Equals(right);

    /// <summary>
    /// 判斷左邊的民國日期是否小於右邊
    /// </summary>
    public static bool operator <(RocDateOnly left, RocDateOnly right) => left.CompareTo(right) < 0;

    /// <summary>
    /// 判斷左邊的民國日期是否大於右邊
    /// </summary>
    public static bool operator >(RocDateOnly left, RocDateOnly right) => left.CompareTo(right) > 0;

    /// <summary>
    /// 判斷左邊的民國日期是否小於或等於右邊
    /// </summary>
    public static bool operator <=(RocDateOnly left, RocDateOnly right) => left.CompareTo(right) <= 0;

    /// <summary>
    /// 判斷左邊的民國日期是否大於或等於右邊
    /// </summary>
    public static bool operator >=(RocDateOnly left, RocDateOnly right) => left.CompareTo(right) >= 0;

    /// <summary>
    /// 從西元日期顯式轉換為民國日期
    /// </summary>
    public static explicit operator RocDateOnly(DateOnly ceDate) => new(ceDate);

    /// <summary>
    /// 從民國日期顯式轉換為西元日期
    /// </summary>
    public static explicit operator DateOnly(RocDateOnly rocDate) => rocDate._ceVal;

    #endregion
}
