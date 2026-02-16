using Blazor;
using Utilities.ROC;

namespace Csharp.Test.ROC;

[Category(Constants.RocBlazorCategory)]
internal class RocDateInputConverterTests
{
    [Test]
    public async Task ToInputDateValue_WithRocDateTime_ShouldReturnIsoDateString()
    {
        var value = new RocDateTime(113, 6, 20);

        var result = RocDateInputConverter.ToInputDateValue(value);

        await Assert.That(result).IsEqualTo("2024-06-20");
    }

    [Test]
    public async Task TryParseInputDateValue_WithIsoDateString_ShouldParseToRocDateTime()
    {
        var success = RocDateInputConverter.TryParseInputDateValue("2024-06-20", out var result);

        await Assert.That(success).IsTrue();
        await Assert.That(result.HasValue).IsTrue();
        await Assert.That(result!.Value.Year).IsEqualTo(113);
        await Assert.That(result.Value.Month).IsEqualTo(6);
        await Assert.That(result.Value.Day).IsEqualTo(20);
    }
}
