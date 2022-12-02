namespace TechnicalTest.Infrastructure.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using TechnicalTest.Domain.Rates;

    public class CurrencyRateRepository : ICurrencyRateRepository
    {
        private readonly TestDbContext _dbContext;
        private bool disposedValue;

        public CurrencyRateRepository(TestDbContext context) 
        {
            _dbContext = context;
        }

        public Task<List<CurrencyRate>> Get()
        {
            return _dbContext
                    .Rates
                    .ToListAsync();
        }

        public async Task Set(List<CurrencyRate> rates)
        {
            _dbContext.Rates.RemoveRange(_dbContext.Rates);
            await _dbContext.SaveChangesAsync();

            rates = rates.Select(r => new CurrencyRate { From = r.From, To = r.To, Rate = r.Rate}).ToList();
            await _dbContext.Rates.AddRangeAsync(rates);
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

        ~CurrencyRateRepository()
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
