namespace TechnicalTest.API.Services.Rates
{
    using TechnicalTest.API.Services.Transactions.Exceptions;
    using TechnicalTest.Domain.Rates;

    public class CurrencyConverter : ICurrencyConverter
    {
        private bool _isInitialized = false;
        private Dictionary<string, List<(string, decimal)>> _convertorDictionary;
        private bool disposedValue;
        private readonly ICurrencyRateRepository _rateRepository;

        public CurrencyConverter(ICurrencyRateRepository rateRepository)
        {
            _rateRepository = rateRepository;
        }

        public async Task<decimal> CalculateConversionRate(string fromCurrency, string toCurrency)
        {
            await Initialize();

            var startingPath = new List<(string toCurrency, decimal rateValue)>() { (fromCurrency, 1) };

            var foundPath = GetConvertorTreeGivenCurrency(fromCurrency, toCurrency, startingPath);

            if (!foundPath.found)
            {
                throw new ConversionNotFoundException(fromCurrency, toCurrency);
            }

            return CalculateRate(foundPath);
        }

        private async Task Initialize()
        {
            if (_isInitialized) return;

            var rates = await _rateRepository.Get();
            _convertorDictionary = GetConvertorDictionary(rates);

            _isInitialized = true;
        }

        private Dictionary<string, List<(string toCurrency, decimal rateValue)>> GetConvertorDictionary(List<CurrencyRate> rates)
        {
            var convertorDictionary = new Dictionary<string, List<(string toCurrency, decimal rateValue)>>();

            foreach (var rate in rates)
            {
                if (convertorDictionary.ContainsKey(rate.From))
                {
                    var currenciesList = convertorDictionary[rate.From];
                    if (!currenciesList.Any(c => c.toCurrency == rate.To))
                    {
                        currenciesList.Add((rate.To, rate.Rate));
                    }
                }
                else
                {
                    convertorDictionary.Add(rate.From, new List<(string, decimal)> { (rate.To, rate.Rate) });
                }
            }

            return convertorDictionary;
        }

        private (bool found, List<(string toCurrency, decimal rateValue)> path) GetConvertorTreeGivenCurrency(string currency, string toCurrency, List<(string toCurrency, decimal rateValue)> evaluatedPath)
        {
            var pathFound = false;
            var path = new List<(string toCurrency, decimal rateValue)>();

            if (_convertorDictionary.ContainsKey(currency))
            {
                if (!evaluatedPath.Any(c => c.toCurrency == toCurrency))
                {
                    foreach ((string toCurrency, decimal rateValue) convertion in _convertorDictionary[currency])
                    {
                        if (evaluatedPath.Any(p => p.toCurrency == convertion.toCurrency))
                        {
                            continue;
                        }

                        var checkingPath = new List<(string toCurrency, decimal rateValue)>(evaluatedPath)
                        {
                            convertion
                        };

                        if (convertion.toCurrency == toCurrency)
                        {
                            pathFound = true;
                            path = new List<(string toCurrency, decimal rateValue)>(checkingPath);
                            break;
                        }
                        else
                        {
                            var find = GetConvertorTreeGivenCurrency(convertion.toCurrency, toCurrency, checkingPath);
                            if (find.found)
                            {
                                pathFound = true;
                                path = new List<(string toCurrency, decimal rateValue)>(find.path);
                                break;
                            }
                        }
                    }
                }
            }

            return (pathFound, path);
        }

        private decimal CalculateRate((bool found, List<(string currency, decimal rateValue)> path) foundPath)
        {
            if (foundPath.found)
            {
                var rate = 1m;
                foundPath.path.ToList().ForEach(p => rate *= p.rateValue);

                return rate;
            }

            return 0m;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _convertorDictionary?.Clear();
                    _isInitialized = false;
                }
                _rateRepository?.Dispose();
                disposedValue = true;
            }
        }

        ~CurrencyConverter()
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
