namespace TechnicalTest.Domain.Transactions
{
    public interface ITransactionRepository : IDisposable
    {
        Task<List<Transaction>> Get();
        Task<List<Transaction>> Get(string sku);
        Task Set(List<Transaction> transactions);
    }
}
