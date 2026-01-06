using Utilities.ROC;

namespace Csharp.Test.ROC;

[Category(Constants.RocDateTimeCategory)]
internal class RocDateTimeTests
{
    #region Constructor Tests

    [Test]
    public async Task Constructor_FromDateTime_ShouldCreateCorrectRocDateTime()
    {
        // Arrange
        var ceDateTime = new DateTime(2024, 1, 15, 10, 30, 45);

        // Act
        var rocDateTime = new RocDateTime(ceDateTime);

        // Assert
        await Assert.That(rocDateTime.Year).IsEqualTo(113);
        await Assert.That(rocDateTime.Month).IsEqualTo(1);
        await Assert.That(rocDateTime.Day).IsEqualTo(15);
        await Assert.That(rocDateTime.Hour).IsEqualTo(10);
        await Assert.That(rocDateTime.Minute).IsEqualTo(30);
        await Assert.That(rocDateTime.Second).IsEqualTo(45);
    }

    [Test]
    public async Task Constructor_FromRocYearMonthDay_ShouldCreateCorrectDateTime()
    {
        // Arrange & Act
        var rocDateTime = new RocDateTime(113, 6, 20);

        // Assert
        await Assert.That(rocDateTime.Year).IsEqualTo(113);
        await Assert.That(rocDateTime.Month).IsEqualTo(6);
        await Assert.That(rocDateTime.Day).IsEqualTo(20);
        await Assert.That(rocDateTime.ToDateTime().Year).IsEqualTo(2024);
    }

    [Test]
    public async Task Constructor_FromRocYearMonthDayHourMinuteSecond_ShouldCreateCorrectDateTime()
    {
        // Arrange & Act
        var rocDateTime = new RocDateTime(113, 6, 20, 14, 30, 45);

        // Assert
        await Assert.That(rocDateTime.Hour).IsEqualTo(14);
        await Assert.That(rocDateTime.Minute).IsEqualTo(30);
        await Assert.That(rocDateTime.Second).IsEqualTo(45);
    }

    [Test]
    public async Task Constructor_FromRocYearMonthDayHourMinuteSecondMillisecond_ShouldCreateCorrectDateTime()
    {
        // Arrange & Act
        var rocDateTime = new RocDateTime(113, 6, 20, 14, 30, 45, 123);

        // Assert
        await Assert.That(rocDateTime.Millisecond).IsEqualTo(123);
    }

    [Test]
    public async Task Constructor_FromRocYearMonthDayHourMinuteSecondMillisecondMicrosecond_ShouldCreateCorrectDateTime()
    {
        // Arrange & Act
        var rocDateTime = new RocDateTime(113, 6, 20, 14, 30, 45, 123, 456);

        // Assert
        await Assert.That(rocDateTime.Millisecond).IsEqualTo(123);
        await Assert.That(rocDateTime.Microsecond).IsEqualTo(456);
    }

    [Test]
    public async Task Constructor_FromTicks_ShouldCreateCorrectDateTime()
    {
        // Arrange
        var ticks = new DateTime(2024, 1, 15, 10, 30, 0).Ticks;

        // Act
        var rocDateTime = new RocDateTime(ticks);

        // Assert
        await Assert.That(rocDateTime.Year).IsEqualTo(113);
    }

    [Test]
    public async Task Constructor_FromTicksAndKind_ShouldCreateCorrectDateTime()
    {
        // Arrange
        var ticks = new DateTime(2024, 1, 15, 10, 30, 0).Ticks;

        // Act
        var rocDateTime = new RocDateTime(ticks, DateTimeKind.Utc);

        // Assert
        await Assert.That(rocDateTime.Kind).IsEqualTo(DateTimeKind.Utc);
    }

    [Test]
    public async Task Constructor_RocYear1_ShouldBe1912()
    {
        // Arrange & Act
        var rocDateTime = new RocDateTime(1, 1, 1);

        // Assert
        await Assert.That(rocDateTime.ToDateTime().Year).IsEqualTo(1912);
    }

    #endregion

    #region Static Properties Tests

    [Test]
    public async Task Now_ShouldReturnCurrentRocDateTime()
    {
        // Arrange
        var fakeTime = new DateTimeOffset(2024, 7, 15, 10, 30, 0, TimeSpan.FromHours(8));
        var timeProvider = new FakeTimeProvider(fakeTime);

        // Act
        var rocNow = timeProvider.GetRocNow();

        // Assert
        await Assert.That(rocNow.Year).IsEqualTo(113);
        await Assert.That(rocNow.Month).IsEqualTo(7);
        await Assert.That(rocNow.Day).IsEqualTo(15);
        await Assert.That(rocNow.Hour).IsEqualTo(10);
        await Assert.That(rocNow.Minute).IsEqualTo(30);
    }

    [Test]
    public async Task UtcNow_ShouldReturnCurrentUtcRocDateTime()
    {
        // Arrange
        var fakeTime = new DateTimeOffset(2024, 7, 15, 2, 30, 0, TimeSpan.Zero);
        var timeProvider = new FakeTimeProvider(fakeTime);

        // Act
        var rocUtcNow = timeProvider.GetRocUtcNow();

        // Assert
        await Assert.That(rocUtcNow.Kind).IsEqualTo(DateTimeKind.Utc);
        await Assert.That(rocUtcNow.Year).IsEqualTo(113);
        await Assert.That(rocUtcNow.Month).IsEqualTo(7);
        await Assert.That(rocUtcNow.Day).IsEqualTo(15);
        await Assert.That(rocUtcNow.Hour).IsEqualTo(2);
        await Assert.That(rocUtcNow.Minute).IsEqualTo(30);
    }

    [Test]
    public async Task Today_ShouldReturnTodaysRocDate()
    {
        // Arrange
        var fakeTime = new DateTimeOffset(2024, 7, 15, 10, 30, 0, TimeSpan.FromHours(8));
        var timeProvider = new FakeTimeProvider(fakeTime);
        var expectedCeDate = new DateTime(2024, 7, 15);

        // Act
        var today = timeProvider.GetRocToday();

        // Assert
        await Assert.That(today.ToDateTime().Date).IsEqualTo(expectedCeDate);
        await Assert.That(today.Year).IsEqualTo(113);
        await Assert.That(today.Month).IsEqualTo(7);
        await Assert.That(today.Day).IsEqualTo(15);
    }

    [Test]
    public async Task MinValue_ShouldEqualDateTimeMinValue()
    {
        // Act
        var minValue = RocDateTime.MinValue;

        // Assert
        await Assert.That(minValue.ToDateTime()).IsEqualTo(DateTime.MinValue);
    }

    [Test]
    public async Task MaxValue_ShouldEqualDateTimeMaxValue()
    {
        // Act
        var maxValue = RocDateTime.MaxValue;

        // Assert
        await Assert.That(maxValue.ToDateTime()).IsEqualTo(DateTime.MaxValue);
    }

    [Test]
    public async Task UnixEpoch_ShouldEqualDateTimeUnixEpoch()
    {
        // Act
        var unixEpoch = RocDateTime.UnixEpoch;

        // Assert
        await Assert.That(unixEpoch.ToDateTime()).IsEqualTo(DateTime.UnixEpoch);
    }

    #endregion

    #region Instance Properties Tests

    [Test]
    public async Task Properties_ShouldReturnCorrectValues()
    {
        // Arrange
        var rocDateTime = new RocDateTime(113, 7, 25, 14, 30, 45);

        // Assert
        await Assert.That(rocDateTime.Year).IsEqualTo(113);
        await Assert.That(rocDateTime.Month).IsEqualTo(7);
        await Assert.That(rocDateTime.Day).IsEqualTo(25);
        await Assert.That(rocDateTime.Hour).IsEqualTo(14);
        await Assert.That(rocDateTime.Minute).IsEqualTo(30);
        await Assert.That(rocDateTime.Second).IsEqualTo(45);
        await Assert.That(rocDateTime.DayOfWeek).IsEqualTo(DayOfWeek.Thursday);
    }

    [Test]
    public async Task Date_ShouldReturnDatePartOnly()
    {
        // Arrange
        var rocDateTime = new RocDateTime(113, 7, 25, 14, 30, 45);

        // Act
        var date = rocDateTime.Date;

        // Assert
        await Assert.That(date.Hour).IsEqualTo(0);
        await Assert.That(date.Minute).IsEqualTo(0);
        await Assert.That(date.Second).IsEqualTo(0);
        await Assert.That(date.Day).IsEqualTo(25);
    }

    [Test]
    public async Task TimeOfDay_ShouldReturnCorrectTimeSpan()
    {
        // Arrange
        var rocDateTime = new RocDateTime(113, 7, 25, 14, 30, 45);

        // Act
        var timeOfDay = rocDateTime.TimeOfDay;

        // Assert
        await Assert.That(timeOfDay).IsEqualTo(new TimeSpan(14, 30, 45));
    }

    [Test]
    public async Task DayOfYear_ShouldReturnCorrectValue()
    {
        // Arrange - 2024/03/01 是閏年的第 61 天
        var rocDateTime = new RocDateTime(113, 3, 1);

        // Assert
        await Assert.That(rocDateTime.DayOfYear).IsEqualTo(61);
    }

    [Test]
    public async Task Ticks_ShouldReturnCorrectValue()
    {
        // Arrange
        var ceDateTime = new DateTime(2024, 5, 10, 10, 30, 0);
        var rocDateTime = new RocDateTime(ceDateTime);

        // Assert
        await Assert.That(rocDateTime.Ticks).IsEqualTo(ceDateTime.Ticks);
    }

    #endregion

    #region Add Methods Tests

    [Test]
    public async Task AddYears_ShouldAddCorrectYears()
    {
        // Arrange
        var rocDateTime = new RocDateTime(113, 6, 20);

        // Act
        var result = rocDateTime.AddYears(5);

        // Assert
        await Assert.That(result.Year).IsEqualTo(118);
    }

    [Test]
    public async Task AddMonths_ShouldAddCorrectMonths()
    {
        // Arrange
        var rocDateTime = new RocDateTime(113, 3, 15);

        // Act
        var result = rocDateTime.AddMonths(5);

        // Assert
        await Assert.That(result.Month).IsEqualTo(8);
    }

    [Test]
    public async Task AddDays_ShouldAddCorrectDays()
    {
        // Arrange
        var rocDateTime = new RocDateTime(113, 1, 15);

        // Act
        var result = rocDateTime.AddDays(10);

        // Assert
        await Assert.That(result.Day).IsEqualTo(25);
    }

    [Test]
    public async Task AddHours_ShouldAddCorrectHours()
    {
        // Arrange
        var rocDateTime = new RocDateTime(113, 1, 15, 10, 0, 0);

        // Act
        var result = rocDateTime.AddHours(5);

        // Assert
        await Assert.That(result.Hour).IsEqualTo(15);
    }

    [Test]
    public async Task AddMinutes_ShouldAddCorrectMinutes()
    {
        // Arrange
        var rocDateTime = new RocDateTime(113, 1, 15, 10, 30, 0);

        // Act
        var result = rocDateTime.AddMinutes(45);

        // Assert
        await Assert.That(result.Hour).IsEqualTo(11);
        await Assert.That(result.Minute).IsEqualTo(15);
    }

    [Test]
    public async Task AddSeconds_ShouldAddCorrectSeconds()
    {
        // Arrange
        var rocDateTime = new RocDateTime(113, 1, 15, 10, 30, 30);

        // Act
        var result = rocDateTime.AddSeconds(45);

        // Assert
        await Assert.That(result.Minute).IsEqualTo(31);
        await Assert.That(result.Second).IsEqualTo(15);
    }

    [Test]
    public async Task AddMilliseconds_ShouldAddCorrectMilliseconds()
    {
        // Arrange
        var rocDateTime = new RocDateTime(113, 1, 15, 10, 30, 30, 500);

        // Act
        var result = rocDateTime.AddMilliseconds(600);

        // Assert
        await Assert.That(result.Second).IsEqualTo(31);
        await Assert.That(result.Millisecond).IsEqualTo(100);
    }

    [Test]
    public async Task AddTicks_ShouldAddCorrectTicks()
    {
        // Arrange
        var rocDateTime = new RocDateTime(113, 1, 15);
        var ticksToAdd = TimeSpan.FromHours(1).Ticks;

        // Act
        var result = rocDateTime.AddTicks(ticksToAdd);

        // Assert
        await Assert.That(result.Hour).IsEqualTo(1);
    }

    [Test]
    public async Task Add_TimeSpan_ShouldAddCorrectly()
    {
        // Arrange
        var rocDateTime = new RocDateTime(113, 1, 15, 10, 0, 0);
        var timeSpan = new TimeSpan(2, 30, 45);

        // Act
        var result = rocDateTime.Add(timeSpan);

        // Assert
        await Assert.That(result.Hour).IsEqualTo(12);
        await Assert.That(result.Minute).IsEqualTo(30);
        await Assert.That(result.Second).IsEqualTo(45);
    }

    [Test]
    public async Task Subtract_TimeSpan_ShouldSubtractCorrectly()
    {
        // Arrange
        var rocDateTime = new RocDateTime(113, 1, 15, 10, 30, 45);
        var timeSpan = new TimeSpan(2, 30, 45);

        // Act
        var result = rocDateTime.Subtract(timeSpan);

        // Assert
        await Assert.That(result.Hour).IsEqualTo(8);
        await Assert.That(result.Minute).IsEqualTo(0);
        await Assert.That(result.Second).IsEqualTo(0);
    }

    [Test]
    public async Task Subtract_RocDateTime_ShouldReturnCorrectTimeSpan()
    {
        // Arrange
        var rocDateTime1 = new RocDateTime(113, 1, 15, 12, 0, 0);
        var rocDateTime2 = new RocDateTime(113, 1, 15, 10, 0, 0);

        // Act
        var result = rocDateTime1.Subtract(rocDateTime2);

        // Assert
        await Assert.That(result).IsEqualTo(TimeSpan.FromHours(2));
    }

    #endregion

    #region Conversion Methods Tests

    [Test]
    public async Task ToDateTime_ShouldConvertCorrectly()
    {
        // Arrange
        var rocDateTime = new RocDateTime(113, 5, 10, 14, 30, 0);

        // Act
        var dateTime = rocDateTime.ToDateTime();

        // Assert
        await Assert.That(dateTime).IsEqualTo(new DateTime(2024, 5, 10, 14, 30, 0));
    }

    [Test]
    public async Task FromDateTime_ShouldCreateCorrectRocDateTime()
    {
        // Arrange
        var dateTime = new DateTime(2024, 8, 15, 10, 30, 0);

        // Act
        var rocDateTime = RocDateTime.FromDateTime(dateTime);

        // Assert
        await Assert.That(rocDateTime.Year).IsEqualTo(113);
    }

    [Test]
    public async Task ToDateOnly_ShouldConvertCorrectly()
    {
        // Arrange
        var rocDateTime = new RocDateTime(113, 5, 10, 14, 30, 0);

        // Act
        var dateOnly = rocDateTime.ToDateOnly();

        // Assert
        await Assert.That(dateOnly).IsEqualTo(new DateOnly(2024, 5, 10));
    }

    [Test]
    public async Task ToTimeOnly_ShouldConvertCorrectly()
    {
        // Arrange
        var rocDateTime = new RocDateTime(113, 5, 10, 14, 30, 45);

        // Act
        var timeOnly = rocDateTime.ToTimeOnly();

        // Assert
        await Assert.That(timeOnly).IsEqualTo(new TimeOnly(14, 30, 45));
    }

    [Test]
    public async Task ToLocalTime_ShouldConvertCorrectly()
    {
        // Arrange
        var rocDateTime = new RocDateTime(DateTime.UtcNow);

        // Act
        var localTime = rocDateTime.ToLocalTime();

        // Assert
        await Assert.That(localTime.Kind).IsEqualTo(DateTimeKind.Local);
    }

    [Test]
    public async Task ToUniversalTime_ShouldConvertCorrectly()
    {
        // Arrange
        var rocDateTime = new RocDateTime(DateTime.Now);

        // Act
        var utcTime = rocDateTime.ToUniversalTime();

        // Assert
        await Assert.That(utcTime.Kind).IsEqualTo(DateTimeKind.Utc);
    }

    [Test]
    public async Task ToOADate_ShouldConvertCorrectly()
    {
        // Arrange
        var ceDateTime = new DateTime(2024, 5, 10, 12, 0, 0);
        var rocDateTime = new RocDateTime(ceDateTime);

        // Act
        var oaDate = rocDateTime.ToOADate();

        // Assert
        await Assert.That(oaDate).IsEqualTo(ceDateTime.ToOADate());
    }

    [Test]
    public async Task FromOADate_ShouldCreateCorrectRocDateTime()
    {
        // Arrange
        var ceDateTime = new DateTime(2024, 5, 10, 12, 0, 0);
        var oaDate = ceDateTime.ToOADate();

        // Act
        var rocDateTime = RocDateTime.FromOADate(oaDate);

        // Assert
        await Assert.That(rocDateTime.Year).IsEqualTo(113);
        await Assert.That(rocDateTime.Month).IsEqualTo(5);
        await Assert.That(rocDateTime.Day).IsEqualTo(10);
    }

    [Test]
    public async Task ToBinary_FromBinary_ShouldRoundTrip()
    {
        // Arrange
        var rocDateTime = new RocDateTime(113, 5, 10, 14, 30, 45);

        // Act
        var binary = rocDateTime.ToBinary();
        var restored = RocDateTime.FromBinary(binary);

        // Assert
        await Assert.That(restored).IsEqualTo(rocDateTime);
    }

    #endregion

    #region Parse Methods Tests

    [Test]
    public async Task Parse_ValidRocFormat_ShouldParseCorrectly()
    {
        // Act
        var result = RocDateTime.Parse("113/05/10 14:30:45");

        // Assert
        await Assert.That(result.Year).IsEqualTo(113);
        await Assert.That(result.Month).IsEqualTo(5);
        await Assert.That(result.Day).IsEqualTo(10);
        await Assert.That(result.Hour).IsEqualTo(14);
        await Assert.That(result.Minute).IsEqualTo(30);
        await Assert.That(result.Second).IsEqualTo(45);
    }

    [Test]
    public async Task Parse_ValidRocDateOnly_ShouldParseCorrectly()
    {
        // Act
        var result = RocDateTime.Parse("113/05/10");

        // Assert
        await Assert.That(result.Year).IsEqualTo(113);
        await Assert.That(result.Month).IsEqualTo(5);
        await Assert.That(result.Day).IsEqualTo(10);
    }

    [Test]
    public async Task Parse_InvalidFormat_ShouldThrowFormatException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<FormatException>(async () =>
        {
            RocDateTime.Parse("invalid");
            await Task.CompletedTask;
        });
    }

    [Test]
    public async Task TryParse_ValidFormat_ShouldReturnTrue()
    {
        // Act
        var success = RocDateTime.TryParse("113/05/10 14:30:45", out var result);

        // Assert
        await Assert.That(success).IsTrue();
        await Assert.That(result.Year).IsEqualTo(113);
    }

    [Test]
    public async Task TryParse_InvalidFormat_ShouldReturnFalse()
    {
        // Act
        var success = RocDateTime.TryParse("invalid", out _);

        // Assert
        await Assert.That(success).IsFalse();
    }

    [Test]
    public async Task TryParse_Null_ShouldReturnFalse()
    {
        // Act
        var success = RocDateTime.TryParse(null, out _);

        // Assert
        await Assert.That(success).IsFalse();
    }

    [Test]
    public async Task Parse_ChineseFormat_ShouldParseCorrectly()
    {
        // Act
        var result = RocDateTime.Parse("113年05月10日");

        // Assert
        await Assert.That(result.Year).IsEqualTo(113);
        await Assert.That(result.Month).IsEqualTo(5);
        await Assert.That(result.Day).IsEqualTo(10);
    }

    [Test]
    public async Task Parse_WithDash_ShouldParseCorrectly()
    {
        // Act
        var result = RocDateTime.Parse("113-05-10");

        // Assert
        await Assert.That(result.Year).IsEqualTo(113);
        await Assert.That(result.Month).IsEqualTo(5);
        await Assert.That(result.Day).IsEqualTo(10);
    }

    [Test]
    public async Task Parse_NoSeparator_ShouldParseCorrectly()
    {
        // Act
        var result = RocDateTime.Parse("1130510");

        // Assert
        await Assert.That(result.Year).IsEqualTo(113);
        await Assert.That(result.Month).IsEqualTo(5);
        await Assert.That(result.Day).IsEqualTo(10);
    }

    [Test]
    public async Task TryParse_Span_ValidFormat_ShouldReturnTrue()
    {
        // Act
        var success = RocDateTime.TryParse("113/05/10".AsSpan(), out var result);

        // Assert
        await Assert.That(success).IsTrue();
        await Assert.That(result.Year).IsEqualTo(113);
    }

    [Test]
    public async Task Parse_CeDateFormat_ShouldThrowFormatException()
    {
        // Act & Assert - 西元日期格式應該解析失敗
        await Assert.ThrowsAsync<FormatException>(async () =>
        {
            RocDateTime.Parse("2026-01-01");
            await Task.CompletedTask;
        });
    }

    [Test]
    public async Task Parse_CeDateFormatWithTime_ShouldThrowFormatException()
    {
        // Act & Assert - 西元日期時間格式應該解析失敗
        await Assert.ThrowsAsync<FormatException>(async () =>
        {
            RocDateTime.Parse("2026-01-07 14:30:45");
            await Task.CompletedTask;
        });
    }

    [Test]
    public async Task Parse_CeDateFormatWithSlash_ShouldThrowFormatException()
    {
        // Act & Assert - 西元日期格式 (斜線分隔) 應該解析失敗
        await Assert.ThrowsAsync<FormatException>(async () =>
        {
            RocDateTime.Parse("2026/01/07");
            await Task.CompletedTask;
        });
    }

    [Test]
    public async Task TryParse_CeDateFormat_ShouldReturnFalse()
    {
        // Act
        var success = RocDateTime.TryParse("2026-01-01", out _);

        // Assert - 西元格式應該解析失敗
        await Assert.That(success).IsFalse();
    }

    [Test]
    public async Task Parse_CeDateFormatNoSeparator_ShouldThrowFormatException()
    {
        // Act & Assert - 西元8位數字格式應該解析失敗
        await Assert.ThrowsAsync<FormatException>(async () =>
        {
            RocDateTime.Parse("20260101");
            await Task.CompletedTask;
        });
    }

    #endregion

    #region Static Utility Methods Tests

    [Test]
    public async Task IsLeapYear_LeapYear_ShouldReturnTrue()
    {
        // 民國 113 年 = 西元 2024 年，是閏年
        await Assert.That(RocDateTime.IsLeapYear(113)).IsTrue();
    }

    [Test]
    public async Task IsLeapYear_NonLeapYear_ShouldReturnFalse()
    {
        // 民國 114 年 = 西元 2025 年，不是閏年
        await Assert.That(RocDateTime.IsLeapYear(114)).IsFalse();
    }

    [Test]
    public async Task DaysInMonth_February_LeapYear_ShouldReturn29()
    {
        // 民國 113 年 = 西元 2024 年，是閏年
        await Assert.That(RocDateTime.DaysInMonth(113, 2)).IsEqualTo(29);
    }

    [Test]
    public async Task DaysInMonth_February_NonLeapYear_ShouldReturn28()
    {
        // 民國 114 年 = 西元 2025 年，不是閏年
        await Assert.That(RocDateTime.DaysInMonth(114, 2)).IsEqualTo(28);
    }

    [Test]
    public async Task SpecifyKind_ShouldChangeKind()
    {
        // Arrange
        var rocDateTime = new RocDateTime(113, 5, 10);

        // Act
        var result = RocDateTime.SpecifyKind(rocDateTime, DateTimeKind.Utc);

        // Assert
        await Assert.That(result.Kind).IsEqualTo(DateTimeKind.Utc);
    }

    [Test]
    public async Task Compare_ShouldCompareCorrectly()
    {
        // Arrange
        var dt1 = new RocDateTime(113, 5, 10);
        var dt2 = new RocDateTime(113, 5, 15);

        // Act & Assert
        await Assert.That(RocDateTime.Compare(dt1, dt2)).IsLessThan(0);
        await Assert.That(RocDateTime.Compare(dt2, dt1)).IsGreaterThan(0);
        await Assert.That(RocDateTime.Compare(dt1, dt1)).IsEqualTo(0);
    }

    [Test]
    public async Task Equals_Static_ShouldWorkCorrectly()
    {
        // Arrange
        var dt1 = new RocDateTime(113, 5, 10);
        var dt2 = new RocDateTime(113, 5, 10);
        var dt3 = new RocDateTime(113, 5, 15);

        // Act & Assert
        await Assert.That(RocDateTime.Equals(dt1, dt2)).IsTrue();
        await Assert.That(RocDateTime.Equals(dt1, dt3)).IsFalse();
    }

    #endregion

    #region ToString Tests

    [Test]
    public async Task ToString_Default_ShouldReturnCorrectFormat()
    {
        // Arrange
        var rocDateTime = new RocDateTime(113, 5, 10, 14, 30, 45);

        // Act
        var result = rocDateTime.ToString();

        // Assert
        await Assert.That(result).IsEqualTo("113/05/10 14:30:45");
    }

    [Test]
    public async Task ToLongDateString_ShouldReturnCorrectFormat()
    {
        // Arrange
        var rocDateTime = new RocDateTime(113, 5, 10);

        // Act
        var result = rocDateTime.ToLongDateString();

        // Assert
        await Assert.That(result).IsEqualTo("民國113年5月10日");
    }

    [Test]
    public async Task ToShortDateString_ShouldReturnCorrectFormat()
    {
        // Arrange
        var rocDateTime = new RocDateTime(113, 5, 10);

        // Act
        var result = rocDateTime.ToShortDateString();

        // Assert
        await Assert.That(result).IsEqualTo("113/05/10");
    }

    [Test]
    public async Task ToString_WithYearFormats_ShouldFormatCorrectly()
    {
        // Arrange
        var rocDateTime = new RocDateTime(113, 5, 10, 14, 30, 45);

        // Assert - yyyy => 0113, yyy => 113, yy => 13, y => 3
        await Assert.That(rocDateTime.ToString("yyyy/MM/dd")).IsEqualTo("0113/05/10");
        await Assert.That(rocDateTime.ToString("yyy/MM/dd")).IsEqualTo("113/05/10");
        await Assert.That(rocDateTime.ToString("yy/MM/dd")).IsEqualTo("13/05/10");
        await Assert.That(rocDateTime.ToString("y/MM/dd")).IsEqualTo("3/05/10");
    }

    [Test]
    public async Task ToString_YearFormats_ShouldBeConsistent()
    {
        // Arrange - 民國 105 年
        var rocDateTime = new RocDateTime(105, 3, 15);

        // Assert - yyyy => 0105, yyy => 105, yy => 05, y => 5
        await Assert.That(rocDateTime.ToString("yyyy")).IsEqualTo("0105");
        await Assert.That(rocDateTime.ToString("yyy")).IsEqualTo("105");
        await Assert.That(rocDateTime.ToString("yy")).IsEqualTo("05");
        await Assert.That(rocDateTime.ToString("y")).IsEqualTo("5");
    }

    #endregion

    #region Comparison Tests

    [Test]
    public async Task CompareTo_EarlierDate_ShouldReturnPositive()
    {
        // Arrange
        var dt1 = new RocDateTime(113, 6, 15);
        var dt2 = new RocDateTime(113, 5, 15);

        // Act
        var result = dt1.CompareTo(dt2);

        // Assert
        await Assert.That(result).IsGreaterThan(0);
    }

    [Test]
    public async Task CompareTo_LaterDate_ShouldReturnNegative()
    {
        // Arrange
        var dt1 = new RocDateTime(113, 5, 15);
        var dt2 = new RocDateTime(113, 6, 15);

        // Act
        var result = dt1.CompareTo(dt2);

        // Assert
        await Assert.That(result).IsLessThan(0);
    }

    [Test]
    public async Task CompareTo_SameDate_ShouldReturnZero()
    {
        // Arrange
        var dt1 = new RocDateTime(113, 5, 15, 10, 30, 0);
        var dt2 = new RocDateTime(113, 5, 15, 10, 30, 0);

        // Act
        var result = dt1.CompareTo(dt2);

        // Assert
        await Assert.That(result).IsEqualTo(0);
    }

    #endregion

    #region Equality Tests

    [Test]
    public async Task Equals_SameDateTime_ShouldReturnTrue()
    {
        // Arrange
        var dt1 = new RocDateTime(113, 5, 15, 10, 30, 0);
        var dt2 = new RocDateTime(113, 5, 15, 10, 30, 0);

        // Act & Assert
        await Assert.That(dt1.Equals(dt2)).IsTrue();
    }

    [Test]
    public async Task Equals_DifferentDateTime_ShouldReturnFalse()
    {
        // Arrange
        var dt1 = new RocDateTime(113, 5, 15, 10, 30, 0);
        var dt2 = new RocDateTime(113, 5, 15, 10, 30, 1);

        // Act & Assert
        await Assert.That(dt1.Equals(dt2)).IsFalse();
    }

    [Test]
    public async Task GetHashCode_SameDateTimes_ShouldReturnSameHashCode()
    {
        // Arrange
        var dt1 = new RocDateTime(113, 5, 15, 10, 30, 0);
        var dt2 = new RocDateTime(113, 5, 15, 10, 30, 0);

        // Act & Assert
        await Assert.That(dt1.GetHashCode()).IsEqualTo(dt2.GetHashCode());
    }

    #endregion

    #region Operator Tests

    [Test]
    public async Task EqualityOperator_SameDateTimes_ShouldReturnTrue()
    {
        // Arrange
        var dt1 = new RocDateTime(113, 5, 15);
        var dt2 = new RocDateTime(113, 5, 15);

        // Act & Assert
        await Assert.That(dt1 == dt2).IsTrue();
    }

    [Test]
    public async Task InequalityOperator_DifferentDateTimes_ShouldReturnTrue()
    {
        // Arrange
        var dt1 = new RocDateTime(113, 5, 15);
        var dt2 = new RocDateTime(113, 5, 16);

        // Act & Assert
        await Assert.That(dt1 != dt2).IsTrue();
    }

    [Test]
    public async Task LessThanOperator_ShouldWorkCorrectly()
    {
        // Arrange
        var dt1 = new RocDateTime(113, 5, 15);
        var dt2 = new RocDateTime(113, 6, 15);

        // Act & Assert
        await Assert.That(dt1 < dt2).IsTrue();
    }

    [Test]
    public async Task GreaterThanOperator_ShouldWorkCorrectly()
    {
        // Arrange
        var dt1 = new RocDateTime(113, 6, 15);
        var dt2 = new RocDateTime(113, 5, 15);

        // Act & Assert
        await Assert.That(dt1 > dt2).IsTrue();
    }

    [Test]
    public async Task LessThanOrEqualOperator_ShouldWorkCorrectly()
    {
        // Arrange
        var dt1 = new RocDateTime(113, 5, 15);
        var dt2 = new RocDateTime(113, 5, 15);
        var dt3 = new RocDateTime(113, 6, 15);

        // Act & Assert
        await Assert.That(dt1 <= dt2).IsTrue();
        await Assert.That(dt1 <= dt3).IsTrue();
    }

    [Test]
    public async Task GreaterThanOrEqualOperator_ShouldWorkCorrectly()
    {
        // Arrange
        var dt1 = new RocDateTime(113, 5, 15);
        var dt2 = new RocDateTime(113, 5, 15);
        var dt3 = new RocDateTime(113, 4, 15);

        // Act & Assert
        await Assert.That(dt1 >= dt2).IsTrue();
        await Assert.That(dt1 >= dt3).IsTrue();
    }

    [Test]
    public async Task AdditionOperator_ShouldAddTimeSpan()
    {
        // Arrange
        var dt = new RocDateTime(113, 5, 15, 10, 0, 0);
        var timeSpan = TimeSpan.FromHours(2);

        // Act
        var result = dt + timeSpan;

        // Assert
        await Assert.That(result.Hour).IsEqualTo(12);
    }

    [Test]
    public async Task SubtractionOperator_TimeSpan_ShouldSubtractTimeSpan()
    {
        // Arrange
        var dt = new RocDateTime(113, 5, 15, 10, 0, 0);
        var timeSpan = TimeSpan.FromHours(2);

        // Act
        var result = dt - timeSpan;

        // Assert
        await Assert.That(result.Hour).IsEqualTo(8);
    }

    [Test]
    public async Task SubtractionOperator_RocDateTime_ShouldReturnTimeSpan()
    {
        // Arrange
        var dt1 = new RocDateTime(113, 5, 15, 12, 0, 0);
        var dt2 = new RocDateTime(113, 5, 15, 10, 0, 0);

        // Act
        var result = dt1 - dt2;

        // Assert
        await Assert.That(result).IsEqualTo(TimeSpan.FromHours(2));
    }

    [Test]
    public async Task ExplicitConversion_ToDateTime_ShouldWorkCorrectly()
    {
        // Arrange
        var rocDateTime = new RocDateTime(113, 5, 15);

        // Act
        var dateTime = (DateTime)rocDateTime;

        // Assert
        await Assert.That(dateTime).IsEqualTo(new DateTime(2024, 5, 15));
    }

    [Test]
    public async Task ExplicitConversion_FromDateTime_ShouldWorkCorrectly()
    {
        // Arrange
        var dateTime = new DateTime(2024, 5, 15);

        // Act
        var rocDateTime = (RocDateTime)dateTime;

        // Assert
        await Assert.That(rocDateTime.Year).IsEqualTo(113);
    }

    #endregion

    #region Deconstruct Tests

    [Test]
    public async Task Deconstruct_YearMonthDay_ShouldReturnCorrectValues()
    {
        // Arrange
        var rocDateTime = new RocDateTime(113, 5, 15);

        // Act
        var (year, month, day) = rocDateTime;

        // Assert
        await Assert.That(year).IsEqualTo(113);
        await Assert.That(month).IsEqualTo(5);
        await Assert.That(day).IsEqualTo(15);
    }

    [Test]
    public async Task Deconstruct_YearMonthDayHourMinuteSecond_ShouldReturnCorrectValues()
    {
        // Arrange
        var rocDateTime = new RocDateTime(113, 5, 15, 14, 30, 45);

        // Act
        rocDateTime.Deconstruct(out var year, out var month, out var day, out var hour, out var minute, out var second);

        // Assert
        await Assert.That(year).IsEqualTo(113);
        await Assert.That(month).IsEqualTo(5);
        await Assert.That(day).IsEqualTo(15);
        await Assert.That(hour).IsEqualTo(14);
        await Assert.That(minute).IsEqualTo(30);
        await Assert.That(second).IsEqualTo(45);
    }

    #endregion

    #region TryFormat Tests

    [Test]
    public async Task TryFormat_Char_ShouldFormatCorrectly()
    {
        // Arrange
        var rocDateTime = new RocDateTime(113, 5, 10, 14, 30, 45);
        var destination = new char[30];

        // Act
        var success = rocDateTime.TryFormat(destination, out int charsWritten, default, null);
        var result = new string(destination, 0, charsWritten);

        // Assert
        await Assert.That(success).IsTrue();
        await Assert.That(result).IsEqualTo("113/05/10 14:30:45");
    }

    [Test]
    public async Task TryFormat_Char_BufferTooSmall_ShouldReturnFalse()
    {
        // Arrange
        var rocDateTime = new RocDateTime(113, 5, 10, 14, 30, 45);
        var destination = new char[5];

        // Act
        var success = rocDateTime.TryFormat(destination, out _, default, null);

        // Assert
        await Assert.That(success).IsFalse();
    }

    [Test]
    public async Task TryFormat_Utf8_ShouldFormatCorrectly()
    {
        // Arrange
        var rocDateTime = new RocDateTime(113, 5, 10, 14, 30, 45);
        var destination = new byte[30];

        // Act
        var success = rocDateTime.TryFormat(destination, out int bytesWritten, default, null);
        var result = System.Text.Encoding.UTF8.GetString(destination, 0, bytesWritten);

        // Assert
        await Assert.That(success).IsTrue();
        await Assert.That(result).IsEqualTo("113/05/10 14:30:45");
    }

    #endregion
}
