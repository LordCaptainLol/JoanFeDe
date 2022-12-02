namespace TechnicalTest.UnitTests.Infrastructure
{
    using TechnicalTest.Domain.Rates;
    using TechnicalTest.Infrastructure;
    using TechnicalTest.Infrastructure.Repositories;
    using TechnicalTest.UnitTests.Utils;

    [TestClass]
    public class RateRepositoryTest
    {
        private readonly TestDbContext _dbContext;
        private readonly ICurrencyRateRepository _rateRepository;

        private CurrencyRate _rate01;
        private CurrencyRate _rate02;
        private CurrencyRate _rate03;

        public RateRepositoryTest()
        {
            _dbContext = TestContextProvider.GetDbContext();
            _rateRepository = new CurrencyRateRepository(_dbContext);
        }

        [TestMethod]
        public async Task Get_should_return_all_items_in_table()
        {
            await InitializeData();
            var allRates = await _rateRepository.Get();

            Assert.AreEqual(3, allRates.Count);
        }

        [TestMethod]
        public async Task Set_should_remove_all_rates()
        {
            await InitializeData();
            var rate04 = new CurrencyRate { From = "F", To = "G", Rate = 1.3m };
            var rate05 = new CurrencyRate { From = "G", To = "H", Rate = 1.1m };
            var rate06 = new CurrencyRate { From = "H", To = "F", Rate = 0.9m };
            var rate07 = new CurrencyRate { From = "F", To = "H", Rate = 0.9m };
            var rates = new List<CurrencyRate> { rate04, rate05, rate06, rate07 };

            await _rateRepository.Set(rates);

            var allRates = await _rateRepository.Get();

            Assert.IsFalse(allRates.Any(r => r.From == "A"));
            Assert.IsFalse(allRates.Any(r => r.From == "B"));
            Assert.IsFalse(allRates.Any(r => r.From == "C"));
        }

        [TestMethod]
        public async Task Set_should_add_new_rates()
        {
            await InitializeData();

            var rate04 = new CurrencyRate { From = "F", To = "G", Rate = 1.3m };
            var rate05 = new CurrencyRate { From = "G", To = "H", Rate = 1.1m };
            var rate06 = new CurrencyRate { From = "H", To = "F", Rate = 0.9m };
            var rate07 = new CurrencyRate { From = "F", To = "H", Rate = 0.9m };
            var rates = new List<CurrencyRate> { rate04, rate05, rate06, rate07 };

            await _rateRepository.Set(rates);

            var allRates = await _rateRepository.Get();

            Assert.AreEqual(4, allRates.Count);
            Assert.AreEqual(2, allRates.Count(r => r.From == "F"));
            Assert.IsTrue(allRates.Any(r => r.From == "G"));
            Assert.IsTrue(allRates.Any(r => r.From == "H"));
        }

        private async Task InitializeData()
        {
            _dbContext.Rates.RemoveRange(_dbContext.Rates);
            await _dbContext.SaveChangesAsync();

            _rate01 = new CurrencyRate { From = "A", To = "B", Rate = 1.3m };
            _rate02 = new CurrencyRate { From = "B", To = "A", Rate = 1.1m };
            _rate03 = new CurrencyRate { From = "C", To = "B", Rate = 0.9m };

            await _dbContext.Rates.AddRangeAsync(new List<CurrencyRate> { _rate01, _rate02, _rate03 });
            await _dbContext.SaveChangesAsync();
        }
    }
}
