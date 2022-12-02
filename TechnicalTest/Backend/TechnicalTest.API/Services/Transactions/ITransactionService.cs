namespace TechnicalTest.API.Services.Rates
{
    using TechnicalTest.Domain.Transactions;

    public interface ITransactionService : IDisposable
    {
        Task<List<Transaction>> Get();
        Task<decimal> GetTransactionAmount(string sku, string toCurrency = "EUR");
        Task Set(List<Transaction> rates);
    }
}
