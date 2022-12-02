namespace TechnicalTest.UnitTests.API.Services.Transactions
{
    using Moq;
    using TechnicalTest.API.Services.Rates;
    using TechnicalTest.Domain.Transactions;

    [TestClass]
    public class TransactionServiceTest
    {
        private readonly Mock<ITransactionRepository> _transactionRepository;
        private readonly Mock<ICurrencyRateService> _rateService;
        private readonly TransactionService _transcationService;

        public TransactionServiceTest() 
        {
            _transactionRepository = new Mock<ITransactionRepository>();
            _rateService = new Mock<ICurrencyRateService>();

            _transcationService = new TransactionService(
                _transactionRepository.Object,
                _rateService.Object);
        }

        [TestMethod]
        public async Task Get_should_return_everything_what_repository_returns()
        {
            StubRepository();

            var allTransactions = await _transcationService.Get();

            Assert.AreEqual(4, allTransactions.Count());
            Assert.IsTrue(allTransactions.Any(r => r.Sku == "A"));
            Assert.IsTrue(allTransactions.Any(r => r.Sku == "B"));
            Assert.IsTrue(allTransactions.Any(r => r.Sku == "C"));
        }

        [TestMethod]
        public async Task GetTransactionAmount_should_return_everything_what_repository_returns()
        {
            var conversionRate = 0.5m;
            StubConvertor(conversionRate);
            StubRepository();

            var transactionAmount = await _transcationService.GetTransactionAmount("B", "2");

            Assert.AreEqual(300 * conversionRate, transactionAmount);
        }

        [TestMethod]
        public async Task Set_should_send_all_transactions_to_repository()
        {
            var transaction01 = new Transaction { Sku = "A", Amount = 0m, Currency = "1" };
            var transaction02 = new Transaction { Sku = "B", Amount = 0m, Currency = "1" };

            var transactions = new List<Transaction> { transaction01, transaction02 };

            await _transcationService.Set(transactions);

            _transactionRepository.Verify(r => r.Set(transactions));
        }

        public void StubRepository()
        {
            var transaction01 = new Transaction { Sku = "A", Amount = 0m, Currency = "1" };
            var transaction02 = new Transaction { Sku = "B", Amount = 100m, Currency = "1" };
            var transaction03 = new Transaction { Sku = "B", Amount = 200m, Currency = "1" };
            var transaction04 = new Transaction { Sku = "C", Amount = 0m, Currency = "1" };

            var transactions = new List<Transaction> { transaction01, transaction02, transaction03, transaction04 };

            _transactionRepository.Setup(r => r.Get()).ReturnsAsync(transactions);
            _transactionRepository.Setup(r => r.Get("A")).ReturnsAsync(new List<Transaction> { transaction01 });
            _transactionRepository.Setup(r => r.Get("B")).ReturnsAsync(new List<Transaction> { transaction02, transaction03 });
            _transactionRepository.Setup(r => r.Get("C")).ReturnsAsync(new List<Transaction> { transaction04 });
        }

        public void StubConvertor(decimal conversionRate)
        {
            _rateService.Setup(r => r.GetConversionRate(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(conversionRate);
        }
    }
}
