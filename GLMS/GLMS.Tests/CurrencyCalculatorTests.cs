using GLMS.Services;

namespace GLMS.Tests;

public class CurrencyCalculatorTests
{
    // happy path with easy numbers so expected value is obvious
    [Fact]
    public void ConvertUsdToZar_ReturnsExpectedRoundedValue()
    {
        var calc = new CurrencyCalculator();
        var result = calc.ConvertUsdToZar(10m, 18.50m);
        Assert.Equal(185.00m, result);
    }

    // confirm i always save/compare with 2 decimal money rounding
    [Fact]
    public void ConvertUsdToZar_RoundsToTwoDecimals()
    {
        var calc = new CurrencyCalculator();
        var result = calc.ConvertUsdToZar(1m, 18.1234m);
        Assert.Equal(18.12m, result);
    }

    // business guard: negative usd amount should not be accepted
    [Fact]
    public void ConvertUsdToZar_Throws_WhenUsdNegative()
    {
        var calc = new CurrencyCalculator();
        Assert.Throws<ArgumentOutOfRangeException>(() => calc.ConvertUsdToZar(-1m, 18.50m));
    }

    // business guard: invalid rate (0 or negative) should fail
    [Fact]
    public void ConvertUsdToZar_Throws_WhenRateIsZeroOrNegative()
    {
        var calc = new CurrencyCalculator();
        Assert.Throws<ArgumentOutOfRangeException>(() => calc.ConvertUsdToZar(10m, 0m));
        Assert.Throws<ArgumentOutOfRangeException>(() => calc.ConvertUsdToZar(10m, -1m));
    }
}

// References (accessed 22 Apr 2026):
// - xUnit getting started: https://xunit.net/docs/getting-started/v2/getting-started
// - Assert.Throws docs: https://api.xunit.net/v3/2.0.1/v3.2.0.1-Xunit.Assert.Throws.html
