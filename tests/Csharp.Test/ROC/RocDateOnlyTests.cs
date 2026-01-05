using Utilities.ROC;

namespace Csharp.Test.ROC;

[Category(Constants.RocDateOnlyCategory)]
internal class RocDateOnlyTests
{
    #region Constructor Tests

    [Test]
    public async Task Constructor_FromDateOnly_ShouldCreateCorrectRocDate()
    {
        // Arrange
        var ceDate = new DateOnly(2024, 1, 15);

        // Act
        var rocDate = new RocDateOnly(ceDate);

        // Assert
        await Assert.That(rocDate.Year).IsEqualTo(113);
        await Assert.That(rocDate.Month).IsEqualTo(1);
        await Assert.That(rocDate.Day).IsEqualTo(15);
    }

    [Test]
    public async Task Constructor_FromRocYearMonthDay_ShouldCreateCorrectDate()
    {
        // Arrange & Act
        var rocDate = new RocDateOnly(113, 6, 20);

        // Assert
        await Assert.That(rocDate.Year).IsEqualTo(113);
        await Assert.That(rocDate.Month).IsEqualTo(6);
        await Assert.That(rocDate.Day).IsEqualTo(20);
        await Assert.That(rocDate.CeYear).IsEqualTo(2024);
    }

    [Test]
    public async Task Constructor_RocYear1_ShouldBe1912()
    {
        // Arrange & Act
        var rocDate = new RocDateOnly(1, 1, 1);

        // Assert
        await Assert.That(rocDate.CeYear).IsEqualTo(1912);
    }

    #endregion

    #region Properties Tests

    [Test]
    public async Task Properties_ShouldReturnCorrectValues()
    {
        // Arrange
        var rocDate = new RocDateOnly(113, 7, 25);

        // Assert
        await Assert.That(rocDate.Year).IsEqualTo(113);
        await Assert.That(rocDate.CeYear).IsEqualTo(2024);
        await Assert.That(rocDate.Month).IsEqualTo(7);
        await Assert.That(rocDate.Day).IsEqualTo(25);
        await Assert.That(rocDate.DayOfWeek).IsEqualTo(DayOfWeek.Thursday);
    }

    [Test]
    public async Task DayOfYear_ShouldReturnCorrectValue()
    {
        // Arrange - 2024/03/01 是閏年的第 61 天
        var rocDate = new RocDateOnly(113, 3, 1);

        // Assert
        await Assert.That(rocDate.DayOfYear).IsEqualTo(61);
    }

    [Test]
    public async Task CeDateOnly_ShouldReturnCorrectDateOnly()
    {
        // Arrange
        var rocDate = new RocDateOnly(113, 5, 10);

        // Assert
        await Assert.That(rocDate.CeDateOnly).IsEqualTo(new DateOnly(2024, 5, 10));
    }

    [Test]
    public async Task MinValue_ShouldBe1912_01_01()
    {
        // Act
        var minValue = RocDateOnly.MinValue;

        // Assert
        await Assert.That(minValue.Year).IsEqualTo(1);
        await Assert.That(minValue.Month).IsEqualTo(1);
        await Assert.That(minValue.Day).IsEqualTo(1);
        await Assert.That(minValue.CeYear).IsEqualTo(1912);
    }

    [Test]
    public async Task MaxValue_ShouldBe9999_12_31()
    {
        // Act
        var maxValue = RocDateOnly.MaxValue;

        // Assert
        await Assert.That(maxValue.CeDateOnly).IsEqualTo(DateOnly.MaxValue);
    }

    #endregion

    #region Factory Methods Tests

    [Test]
    public async Task FromDateTime_ShouldCreateCorrectRocDate()
    {
        // Arrange
        var dateTime = new DateTime(2024, 8, 15, 10, 30, 0);

        // Act
        var rocDate = RocDateOnly.FromDateTime(dateTime);

        // Assert
        await Assert.That(rocDate.Year).IsEqualTo(113);
        await Assert.That(rocDate.Month).IsEqualTo(8);
        await Assert.That(rocDate.Day).IsEqualTo(15);
    }

    [Test]
    public async Task FromDateOnly_ShouldCreateCorrectRocDate()
    {
        // Arrange
        var dateOnly = new DateOnly(2024, 12, 25);

        // Act
        var rocDate = RocDateOnly.FromDateOnly(dateOnly);

        // Assert
        await Assert.That(rocDate.Year).IsEqualTo(113);
        await Assert.That(rocDate.Month).IsEqualTo(12);
        await Assert.That(rocDate.Day).IsEqualTo(25);
    }

    [Test]
    public async Task FromDayNumber_ShouldCreateCorrectRocDate()
    {
        // Arrange
        var dayNumber = new DateOnly(2024, 1, 1).DayNumber;

        // Act
        var rocDate = RocDateOnly.FromDayNumber(dayNumber);

        // Assert
        await Assert.That(rocDate.Year).IsEqualTo(113);
        await Assert.That(rocDate.Month).IsEqualTo(1);
        await Assert.That(rocDate.Day).IsEqualTo(1);
    }

    [Test]
    public async Task Today_ShouldReturnTodaysRocDate()
    {
        // Arrange
        var expectedCeDate = DateOnly.FromDateTime(DateTime.Today);

        // Act
        var today = RocDateOnly.Today;

        // Assert
        await Assert.That(today.CeDateOnly).IsEqualTo(expectedCeDate);
    }

    #endregion

    #region Add Methods Tests

    [Test]
    public async Task AddDays_ShouldAddCorrectDays()
    {
        // Arrange
        var rocDate = new RocDateOnly(113, 1, 15);

        // Act
        var result = rocDate.AddDays(10);

        // Assert
        await Assert.That(result.Day).IsEqualTo(25);
    }

    [Test]
    public async Task AddDays_Negative_ShouldSubtractDays()
    {
        // Arrange
        var rocDate = new RocDateOnly(113, 1, 15);

        // Act
        var result = rocDate.AddDays(-10);

        // Assert
        await Assert.That(result.Day).IsEqualTo(5);
    }

    [Test]
    public async Task AddMonths_ShouldAddCorrectMonths()
    {
        // Arrange
        var rocDate = new RocDateOnly(113, 3, 15);

        // Act
        var result = rocDate.AddMonths(5);

        // Assert
        await Assert.That(result.Month).IsEqualTo(8);
        await Assert.That(result.Year).IsEqualTo(113);
    }

    [Test]
    public async Task AddMonths_CrossYear_ShouldHandleCorrectly()
    {
        // Arrange
        var rocDate = new RocDateOnly(113, 11, 15);

        // Act
        var result = rocDate.AddMonths(3);

        // Assert
        await Assert.That(result.Month).IsEqualTo(2);
        await Assert.That(result.Year).IsEqualTo(114);
    }

    [Test]
    public async Task AddYears_ShouldAddCorrectYears()
    {
        // Arrange
        var rocDate = new RocDateOnly(113, 6, 20);

        // Act
        var result = rocDate.AddYears(5);

        // Assert
        await Assert.That(result.Year).IsEqualTo(118);
        await Assert.That(result.Month).IsEqualTo(6);
        await Assert.That(result.Day).IsEqualTo(20);
    }

    [Test]
    public async Task AddYears_Negative_ShouldSubtractYears()
    {
        // Arrange
        var rocDate = new RocDateOnly(113, 6, 20);

        // Act
        var result = rocDate.AddYears(-10);

        // Assert
        await Assert.That(result.Year).IsEqualTo(103);
    }

    #endregion

    #region Conversion Methods Tests

    [Test]
    public async Task ToDateTime_ShouldConvertCorrectly()
    {
        // Arrange
        var rocDate = new RocDateOnly(113, 5, 10);
        var time = new TimeOnly(14, 30, 0);

        // Act
        var dateTime = rocDate.ToDateTime(time);

        // Assert
        await Assert.That(dateTime).IsEqualTo(new DateTime(2024, 5, 10, 14, 30, 0));
    }

    [Test]
    public async Task ToDateTime_WithKind_ShouldConvertCorrectly()
    {
        // Arrange
        var rocDate = new RocDateOnly(113, 5, 10);
        var time = new TimeOnly(14, 30, 0);

        // Act
        var dateTime = rocDate.ToDateTime(time, DateTimeKind.Utc);

        // Assert
        await Assert.That(dateTime.Kind).IsEqualTo(DateTimeKind.Utc);
    }

    [Test]
    public async Task ToLongDateString_ShouldReturnCorrectFormat()
    {
        // Arrange
        var rocDate = new RocDateOnly(113, 5, 10);

        // Act
        var result = rocDate.ToLongDateString();

        // Assert
        await Assert.That(result).IsEqualTo("民國113年5月10日");
    }

    [Test]
    public async Task ToShortDateString_ShouldReturnCorrectFormat()
    {
        // Arrange
        var rocDate = new RocDateOnly(113, 5, 10);

        // Act
        var result = rocDate.ToShortDateString();

        // Assert
        await Assert.That(result).IsEqualTo("113/05/10");
    }

    #endregion

    #region Parse Methods Tests

    [Test]
    public async Task Parse_ValidRocFormat_ShouldParseCorrectly()
    {
        // Act
        var result = RocDateOnly.Parse("113/05/10");

        // Assert
        await Assert.That(result.Year).IsEqualTo(113);
        await Assert.That(result.Month).IsEqualTo(5);
        await Assert.That(result.Day).IsEqualTo(10);
    }

    [Test]
    public async Task Parse_ValidRocFormatNoSeparator_ShouldParseCorrectly()
    {
        // Act
        var result = RocDateOnly.Parse("1130510");

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
            RocDateOnly.Parse("invalid");
            await Task.CompletedTask;
        });
    }

    [Test]
    public async Task TryParse_ValidFormat_ShouldReturnTrue()
    {
        // Act
        var success = RocDateOnly.TryParse("113/05/10", out var result);

        // Assert
        await Assert.That(success).IsTrue();
        await Assert.That(result.Year).IsEqualTo(113);
    }

    [Test]
    public async Task TryParse_InvalidFormat_ShouldReturnFalse()
    {
        // Act
        var success = RocDateOnly.TryParse("invalid", out _);

        // Assert
        await Assert.That(success).IsFalse();
    }

    [Test]
    public async Task TryParse_Null_ShouldReturnFalse()
    {
        // Act
        var success = RocDateOnly.TryParse(null, out _);

        // Assert
        await Assert.That(success).IsFalse();
    }

    [Test]
    public async Task Parse_WithDash_ShouldParseCorrectly()
    {
        // Act
        var result = RocDateOnly.Parse("113-05-10");

        // Assert
        await Assert.That(result.Year).IsEqualTo(113);
        await Assert.That(result.Month).IsEqualTo(5);
        await Assert.That(result.Day).IsEqualTo(10);
    }

    #endregion

    #region ToString Tests

    [Test]
    public async Task ToString_Default_ShouldReturnShortDateString()
    {
        // Arrange
        var rocDate = new RocDateOnly(113, 5, 10);

        // Act
        var result = rocDate.ToString();

        // Assert
        await Assert.That(result).IsEqualTo("113/05/10");
    }

    [Test]
    public async Task ToString_WithFormat_ShouldFormatCorrectly()
    {
        // Arrange
        var rocDate = new RocDateOnly(113, 5, 10);

        // Act
        var result = rocDate.ToString("yyy年MM月dd日");

        // Assert
        await Assert.That(result).IsEqualTo("113年05月10日");
    }

    [Test]
    public async Task ToString_WithCeYearFormat_ShouldReturnCeYear()
    {
        // Arrange
        var rocDate = new RocDateOnly(113, 5, 10);

        // Act
        var result = rocDate.ToString("yyyy/MM/dd");

        // Assert - yyyy 格式輸出西元年
        await Assert.That(result).IsEqualTo("2024/05/10");
    }

    #endregion

    #region Comparison Tests

    [Test]
    public async Task CompareTo_EarlierDate_ShouldReturnPositive()
    {
        // Arrange
        var date1 = new RocDateOnly(113, 6, 15);
        var date2 = new RocDateOnly(113, 5, 15);

        // Act
        var result = date1.CompareTo(date2);

        // Assert
        await Assert.That(result).IsGreaterThan(0);
    }

    [Test]
    public async Task CompareTo_LaterDate_ShouldReturnNegative()
    {
        // Arrange
        var date1 = new RocDateOnly(113, 5, 15);
        var date2 = new RocDateOnly(113, 6, 15);

        // Act
        var result = date1.CompareTo(date2);

        // Assert
        await Assert.That(result).IsLessThan(0);
    }

    [Test]
    public async Task CompareTo_SameDate_ShouldReturnZero()
    {
        // Arrange
        var date1 = new RocDateOnly(113, 5, 15);
        var date2 = new RocDateOnly(113, 5, 15);

        // Act
        var result = date1.CompareTo(date2);

        // Assert
        await Assert.That(result).IsEqualTo(0);
    }

    #endregion

    #region Equality Tests

    [Test]
    public async Task Equals_SameDate_ShouldReturnTrue()
    {
        // Arrange
        var date1 = new RocDateOnly(113, 5, 15);
        var date2 = new RocDateOnly(113, 5, 15);

        // Act & Assert
        await Assert.That(date1.Equals(date2)).IsTrue();
    }

    [Test]
    public async Task Equals_DifferentDate_ShouldReturnFalse()
    {
        // Arrange
        var date1 = new RocDateOnly(113, 5, 15);
        var date2 = new RocDateOnly(113, 5, 16);

        // Act & Assert
        await Assert.That(date1.Equals(date2)).IsFalse();
    }

    [Test]
    public async Task GetHashCode_SameDates_ShouldReturnSameHashCode()
    {
        // Arrange
        var date1 = new RocDateOnly(113, 5, 15);
        var date2 = new RocDateOnly(113, 5, 15);

        // Act & Assert
        await Assert.That(date1.GetHashCode()).IsEqualTo(date2.GetHashCode());
    }

    #endregion

    #region Operator Tests

    [Test]
    public async Task EqualityOperator_SameDates_ShouldReturnTrue()
    {
        // Arrange
        var date1 = new RocDateOnly(113, 5, 15);
        var date2 = new RocDateOnly(113, 5, 15);

        // Act & Assert
        await Assert.That(date1 == date2).IsTrue();
    }

    [Test]
    public async Task InequalityOperator_DifferentDates_ShouldReturnTrue()
    {
        // Arrange
        var date1 = new RocDateOnly(113, 5, 15);
        var date2 = new RocDateOnly(113, 5, 16);

        // Act & Assert
        await Assert.That(date1 != date2).IsTrue();
    }

    [Test]
    public async Task LessThanOperator_ShouldWorkCorrectly()
    {
        // Arrange
        var date1 = new RocDateOnly(113, 5, 15);
        var date2 = new RocDateOnly(113, 6, 15);

        // Act & Assert
        await Assert.That(date1 < date2).IsTrue();
    }

    [Test]
    public async Task GreaterThanOperator_ShouldWorkCorrectly()
    {
        // Arrange
        var date1 = new RocDateOnly(113, 6, 15);
        var date2 = new RocDateOnly(113, 5, 15);

        // Act & Assert
        await Assert.That(date1 > date2).IsTrue();
    }

    [Test]
    public async Task LessThanOrEqualOperator_ShouldWorkCorrectly()
    {
        // Arrange
        var date1 = new RocDateOnly(113, 5, 15);
        var date2 = new RocDateOnly(113, 5, 15);
        var date3 = new RocDateOnly(113, 6, 15);

        // Act & Assert
        await Assert.That(date1 <= date2).IsTrue();
        await Assert.That(date1 <= date3).IsTrue();
    }

    [Test]
    public async Task GreaterThanOrEqualOperator_ShouldWorkCorrectly()
    {
        // Arrange
        var date1 = new RocDateOnly(113, 5, 15);
        var date2 = new RocDateOnly(113, 5, 15);
        var date3 = new RocDateOnly(113, 4, 15);

        // Act & Assert
        await Assert.That(date1 >= date2).IsTrue();
        await Assert.That(date1 >= date3).IsTrue();
    }

    [Test]
    public async Task ExplicitConversion_ToDateOnly_ShouldWorkCorrectly()
    {
        // Arrange
        var rocDate = new RocDateOnly(113, 5, 15);

        // Act
        var dateOnly = (DateOnly)rocDate;

        // Assert
        await Assert.That(dateOnly).IsEqualTo(new DateOnly(2024, 5, 15));
    }

    [Test]
    public async Task ExplicitConversion_FromDateOnly_ShouldWorkCorrectly()
    {
        // Arrange
        var dateOnly = new DateOnly(2024, 5, 15);

        // Act
        var rocDate = (RocDateOnly)dateOnly;

        // Assert
        await Assert.That(rocDate.Year).IsEqualTo(113);
    }

    #endregion

    #region Deconstruct Tests

    [Test]
    public async Task Deconstruct_ShouldReturnCorrectValues()
    {
        // Arrange
        var rocDate = new RocDateOnly(113, 5, 15);

        // Act
        var (year, month, day) = rocDate;

        // Assert
        await Assert.That(year).IsEqualTo(113);
        await Assert.That(month).IsEqualTo(5);
        await Assert.That(day).IsEqualTo(15);
    }

    #endregion

    #region TryFormat Tests

    [Test]
    public async Task TryFormat_Char_ShouldFormatCorrectly()
    {
        // Arrange
        var rocDate = new RocDateOnly(113, 5, 10);
        var destination = new char[20];

        // Act
        var success = rocDate.TryFormat(destination, out int charsWritten, default, null);
        var result = new string(destination, 0, charsWritten);

        // Assert
        await Assert.That(success).IsTrue();
        await Assert.That(result).IsEqualTo("113/05/10");
    }

    [Test]
    public async Task TryFormat_Char_BufferTooSmall_ShouldReturnFalse()
    {
        // Arrange
        var rocDate = new RocDateOnly(113, 5, 10);
        var destination = new char[5];

        // Act
        var success = rocDate.TryFormat(destination, out _, default, null);

        // Assert
        await Assert.That(success).IsFalse();
    }

    [Test]
    public async Task TryFormat_Utf8_ShouldFormatCorrectly()
    {
        // Arrange
        var rocDate = new RocDateOnly(113, 5, 10);
        var destination = new byte[20];

        // Act
        var success = rocDate.TryFormat(destination, out int bytesWritten, default, null);
        var result = System.Text.Encoding.UTF8.GetString(destination, 0, bytesWritten);

        // Assert
        await Assert.That(success).IsTrue();
        await Assert.That(result).IsEqualTo("113/05/10");
    }

    #endregion
}
