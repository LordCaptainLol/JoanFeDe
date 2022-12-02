namespace TechnicalTest.API.Services.Rates
{
    using TechnicalTest.Domain.Transactions;

    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly ICurrencyRateService _rateService;
        private bool disposedValue;

        public TransactionService(
            ITransactionRepository transactionRepository,
            ICurrencyRateService rateService)
        {
            _transactionRepository = transactionRepository;
            _rateService = rateService;
        }

        public Task<List<Transaction>> Get()
        {
            return _transactionRepository.Get();
        }

        public Task Set(List<Transaction> transactions)
        {
            return _transactionRepository.Set(transactions);
        }

        public async Task<decimal> GetTransactionAmount(string sku, string toCurrency = "EUR")
        {
            var totalAmount = 0m;

            var transactions = await _transactionRepository.Get(sku);

            foreach (var transaction in transactions)
            {
                if (transaction.Currency == toCurrency)
                {
                    totalAmount += transaction.Amount;
                }
                else
                {
                    var conversionRate = await _rateService.GetConversionRate(transaction.Currency, toCurrency);
                    totalAmount += conversionRate * transaction.Amount;
                }
            }

            return totalAmount;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                _transactionRepository?.Dispose();
                _rateService?.Dispose();
                disposedValue = true;
            }
        }

        ~TransactionService()
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
