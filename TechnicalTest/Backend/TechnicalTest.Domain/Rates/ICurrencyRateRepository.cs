namespace TechnicalTest.Domain.Rates
{
    public interface ICurrencyRateRepository : IDisposable
    {
        Task<List<CurrencyRate>> Get();
        Task Set(List<CurrencyRate> rates);
    }
}
