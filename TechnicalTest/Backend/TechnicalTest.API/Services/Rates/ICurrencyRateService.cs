namespace TechnicalTest.API.Services.Rates
{
    using TechnicalTest.Domain.Rates;

    public interface ICurrencyRateService : IDisposable
    {
        Task<List<CurrencyRate>> Get();
        Task Set(List<CurrencyRate> rates);
        Task<decimal> GetConversionRate(string fromCurrency, string toCurrency);
    }
}
