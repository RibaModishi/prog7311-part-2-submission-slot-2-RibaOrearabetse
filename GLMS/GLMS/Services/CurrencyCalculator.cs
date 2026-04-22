namespace GLMS.Services;

// pure math helper so i can test conversion without db/http/controller noise
public class CurrencyCalculator
{
    public decimal ConvertUsdToZar(decimal usdAmount, decimal zarPerUsdRate)
    {
        // guard checks so bad values fail fast
        if (usdAmount < 0)
            throw new ArgumentOutOfRangeException(nameof(usdAmount), "USD amount cannot be negative.");

        if (zarPerUsdRate <= 0)
            throw new ArgumentOutOfRangeException(nameof(zarPerUsdRate), "Exchange rate must be greater than zero.");

        // money rounding rule i use in app + tests
        return Math.Round(usdAmount * zarPerUsdRate, 2, MidpointRounding.AwayFromZero);
    }
}

// References (accessed 22 Apr 2026):
// - Math.Round docs: https://learn.microsoft.com/en-us/dotnet/api/system.math.round
// - MidpointRounding enum docs: https://learn.microsoft.com/en-us/dotnet/api/system.midpointrounding
