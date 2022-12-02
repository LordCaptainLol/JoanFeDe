namespace TechnicalTest.API.Services.Rates
{
    using TechnicalTest.Domain.Rates;

    public class CurrencyRateService : ICurrencyRateService
    {
        private readonly ICurrencyRateRepository _rateRepository;
        private readonly ICurrencyConverter _currencyConverter;
        private bool disposedValue;

        public CurrencyRateService(
            ICurrencyRateRepository rateRepository,
            ICurrencyConverter currencyConverter)
        {
            _rateRepository = rateRepository;
            _currencyConverter = currencyConverter;
        }

        public Task Set(List<CurrencyRate> rates)
        {
            return _rateRepository.Set(rates);
        }

        public Task<List<CurrencyRate>> Get()
        {
            return _rateRepository.Get();
        }

        public Task<decimal> GetConversionRate(string fromCurrency, string toCurrency)
        {
            return _currencyConverter.CalculateConversionRate(fromCurrency, toCurrency);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                _rateRepository?.Dispose();
                _currencyConverter?.Dispose();
                disposedValue = true;
            }
        }

        ~CurrencyRateService()
        {
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
