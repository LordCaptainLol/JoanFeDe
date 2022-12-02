namespace TechnicalTest.API.Services.Rates
{
    public interface ICurrencyConverter : IDisposable
    {
        Task<decimal> CalculateConversionRate(string fromCurrency, string toCurrency);
    }
}
