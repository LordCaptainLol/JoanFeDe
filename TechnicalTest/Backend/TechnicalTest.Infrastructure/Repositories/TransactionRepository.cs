namespace TechnicalTest.Infrastructure.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using TechnicalTest.Domain.Transactions;

    public class TransactionRepository : ITransactionRepository
    {
        private readonly TestDbContext _dbContext;
        private bool disposedValue;

        public TransactionRepository(TestDbContext context)
        {
            _dbContext = context;
        }

        public Task<List<Transaction>> Get()
        {
            return _dbContext
                    .Transactions
                    .ToListAsync();
        }

        public Task<List<Transaction>> Get(string sku)
        {
            return _dbContext
                .Transactions
                .Where(t => t.Sku == sku)
                .ToListAsync();
        }

        public async Task Set(List<Transaction> transactions)
        {
            _dbContext.Transactions.RemoveRange(_dbContext.Transactions);
            await _dbContext.SaveChangesAsync();

            transactions = transactions.Select(t => new Transaction { Sku = t.Sku, Amount = t.Amount, Currency = t.Currency }).ToList();
            await _dbContext.Transactions.AddRangeAsync(transactions);
            await _dbContext.SaveChangesAsync();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                _dbContext?.Dispose();
                disposedValue = true;
            }
        }

        ~TransactionRepository()
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
