namespace TechnicalTest.UnitTests.API.Services.Rates
{
    using Moq;
    using TechnicalTest.API.Services.Rates;
    using TechnicalTest.Domain.Rates;

    [TestClass]
    public class RateServiceTest
    {
        private readonly Mock<ICurrencyRateRepository> _rateRepository;
        private readonly Mock<ICurrencyConverter> _currencyConverter;
        private readonly CurrencyRateService _rateService;

        public RateServiceTest()
        {
            _rateRepository = new Mock<ICurrencyRateRepository>();
            _currencyConverter = new Mock<ICurrencyConverter>();

            _rateService = new CurrencyRateService(
                _rateRepository.Object,
                _currencyConverter.Object);
        }

        [TestMethod]
        public async Task Get_should_return_everything_what_repository_returns()
        {
            StubRepository();

            var allRates = await _rateService.Get();

            Assert.AreEqual(2, allRates.Count());
            Assert.IsTrue(allRates.Any(r => r.From == "A" && r.To == "B"));
            Assert.IsTrue(allRates.Any(r => r.From == "B" && r.To == "A"));
        }

        [TestMethod]
        public async Task Set_should_send_all_rates_to_repository()
        {
            var rate01 = new CurrencyRate { From = "A", To = "B", Rate = 1.2m };
            var rate02 = new CurrencyRate { From = "B", To = "A", Rate = 0.8m };
            var rates = new List<CurrencyRate> { rate01, rate02 };

            await _rateService.Set(rates);

            _rateRepository.Verify(r => r.Set(rates));
        }

        public void StubRepository()
        {
            var rate01 = new CurrencyRate { From = "A", To = "B", Rate = 1.2m };
            var rate02 = new CurrencyRate { From = "B", To = "A", Rate = 0.8m };

            var rates = new List<CurrencyRate> { rate01, rate02 };
            _rateRepository.Setup(r => r.Get()).ReturnsAsync(rates);
        }
    }
}
