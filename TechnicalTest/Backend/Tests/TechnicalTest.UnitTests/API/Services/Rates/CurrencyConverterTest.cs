namespace TechnicalTest.UnitTests.API.Services.Rates
{
    using Moq;
    using TechnicalTest.API.Services.Rates;
    using TechnicalTest.API.Services.Transactions.Exceptions;
    using TechnicalTest.Domain.Rates;

    [TestClass]
    public class CurrencyConverterTest
    {
        private readonly Mock<ICurrencyRateRepository> _rateRepository;
        private readonly ICurrencyConverter _currencyConverter;

        public CurrencyConverterTest() 
        {
            _rateRepository= new Mock<ICurrencyRateRepository>();

            _currencyConverter = new CurrencyConverter(_rateRepository.Object);
        }

        [TestInitialize]
        public void TestInitialize()
        {
            var rates = new List<CurrencyRate> 
            {
                new CurrencyRate { From = "EUR", To = "USD", Rate = 1.359m },
                new CurrencyRate { From = "CAD", To = "EUR", Rate = 0.732m },
                new CurrencyRate { From = "USD", To = "EUR", Rate = 0.736m },
                new CurrencyRate { From = "EUR", To = "CAD", Rate = 1.366m },
                new CurrencyRate { From = "LIB", To = "RUB", Rate = 1.366m }
            };
            _rateRepository.Setup(r => r.Get()).ReturnsAsync(rates);
        }

        public static IEnumerable<object[]> TestData => new[] 
        {
            new object[] { "EUR", "USD", 1.359m },
            new object[] { "EUR", "CAD", 1.366m },
            new object[] { "USD", "EUR", 0.736m },
            new object[] { "USD", "CAD", 1.005376m },
            new object[] { "CAD", "USD", 0.994788m },
            new object[] { "CAD", "EUR", 0.732m }
        };
        [TestMethod]
        [DynamicData(nameof(TestData))]
        public async Task CalculateConversionRate_should_return_expected_conversion_rate(string fromCurrency, string toCurrency, decimal expectedRate)
        {
            var conversionRate = await _currencyConverter.CalculateConversionRate(fromCurrency, toCurrency);

            Assert.AreEqual(expectedRate, conversionRate);
        }

        [TestMethod]
        public void CalculateConversionRate_should_throw_ConversionNotFoundException_when_dont_find_conversion()
        {
            Assert.ThrowsExceptionAsync<ConversionNotFoundException>(() => _currencyConverter.CalculateConversionRate("A", "B"));
        }
    }
}
