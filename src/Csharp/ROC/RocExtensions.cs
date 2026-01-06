namespace Utilities.ROC;

/// <summary>
/// 民國日期相關的共用常數與擴充方法
/// </summary>
public static partial class RocExtensions
{
    /// <summary>
    /// 民國元年的西元年份偏移量 (1911)
    /// </summary>
    public const int RocYearOffset = 1911;

    /// <param name="rocYear">民國年</param>
    extension(int rocYear)
    {
        /// <summary>
        /// 判斷指定的民國年是否為閏年
        /// </summary>
        /// <returns>是否為閏年</returns>
        public bool IsRocLeapYear() => DateTime.IsLeapYear(rocYear + RocYearOffset);

        /// <summary>
        /// 取得指定民國年月的天數
        /// </summary>
        /// <param name="month">月份</param>
        /// <returns>該月天數</returns>
        public int RocDaysInMonth(int month) => DateTime.DaysInMonth(rocYear + RocYearOffset, month);

        /// <summary>
        /// 將民國年轉換為西元年
        /// </summary>
        /// <returns>西元年</returns>
        public int ToCeYear() => rocYear + RocYearOffset;
    }

    /// <param name="ceYear">西元年</param>
    extension(int ceYear)
    {
        /// <summary>
        /// 將西元年轉換為民國年
        /// </summary>
        /// <returns>民國年</returns>
        public int ToRocYear() => ceYear - RocYearOffset;
    }

    /// <param name="dateTime">西元日期時間</param>
    extension(DateTime dateTime)
    {
        /// <summary>
        /// 將 <see cref="DateTime"/> 轉換為 <see cref="RocDateTime"/>
        /// </summary>
        /// <returns>民國日期時間</returns>
        public RocDateTime ToRocDateTime() => new(dateTime);
    }

    /// <param name="dateOnly">西元日期</param>
    extension(DateOnly dateOnly)
    {
        /// <summary>
        /// 將 <see cref="DateOnly"/> 轉換為 <see cref="RocDateOnly"/>
        /// </summary>
        /// <returns>民國日期</returns>
        public RocDateOnly ToRocDateOnly() => new(dateOnly);
    }

    /// <param name="rocDateTime">民國日期時間</param>
    extension(RocDateTime rocDateTime)
    {
        /// <summary>
        /// 將 <see cref="RocDateTime"/> 轉換為 <see cref="RocDateOnly"/>
        /// </summary>
        /// <returns>民國日期</returns>
        public RocDateOnly ToRocDateOnly() => new(rocDateTime.ToDateOnly());
    }

    /// <param name="rocDateTime">民國日期時間</param>
    extension(RocDateTime? rocDateTime)
    {
        public string ToString(string format = "yyy/MM/dd HH:mm:ss") =>
            rocDateTime.HasValue ? rocDateTime.Value.ToString(format) : string.Empty;
    }

    /// <param name="rocDateOnly">民國日期</param>
    extension(RocDateOnly rocDateOnly)
    {
        /// <summary>
        /// 將 <see cref="RocDateOnly"/> 轉換為 <see cref="RocDateTime"/>
        /// </summary>
        /// <param name="time">時間部分</param>
        /// <returns>民國日期時間</returns>
        public RocDateTime ToRocDateTime(TimeOnly time = default) =>
            new(rocDateOnly.ToDateTime(time));
    }

    /// <param name="rocDateOnly">民國日期</param>
    extension(RocDateOnly? rocDateOnly)
    {
        public string ToString(string format = "yyy/MM/dd") =>
            rocDateOnly.HasValue ? rocDateOnly.Value.ToString(format) : string.Empty;
    }

    /// <param name="timeProvider">時間提供者</param>
    extension(TimeProvider timeProvider)
    {
        /// <summary>
        /// 使用指定的 <see cref="TimeProvider"/> 取得目前的民國日期時間
        /// </summary>
        /// <returns>目前的民國日期時間</returns>
        public RocDateTime GetRocNow() =>
            new(timeProvider.GetLocalNow().DateTime);

        /// <summary>
        /// 使用指定的 <see cref="TimeProvider"/> 取得目前的民國日期時間 (UTC)
        /// </summary>
        /// <returns>目前的民國日期時間 (UTC)</returns>
        public RocDateTime GetRocUtcNow() =>
            new(timeProvider.GetUtcNow().UtcDateTime);

        /// <summary>
        /// 使用指定的 <see cref="TimeProvider"/> 取得今天的民國日期時間
        /// </summary>
        /// <returns>今天的民國日期時間</returns>
        public RocDateTime GetRocToday() =>
            new(timeProvider.GetLocalNow().Date);

        /// <summary>
        /// 使用指定的 <see cref="TimeProvider"/> 取得今天的民國日期
        /// </summary>
        /// <returns>今天的民國日期</returns>
        public RocDateOnly GetRocDateOnlyToday() =>
            new(DateOnly.FromDateTime(timeProvider.GetLocalNow().DateTime));
    }
}
