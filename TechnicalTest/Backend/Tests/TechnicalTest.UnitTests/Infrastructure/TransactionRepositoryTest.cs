namespace TechnicalTest.UnitTests.Infrastructure
{
    using TechnicalTest.Domain.Rates;
    using TechnicalTest.Domain.Transactions;
    using TechnicalTest.Infrastructure;
    using TechnicalTest.Infrastructure.Repositories;
    using TechnicalTest.UnitTests.Utils;

    [TestClass]
    public class TransactionRepositoryTest
    {
        private readonly TestDbContext _dbContext;
        private readonly ITransactionRepository _transactionRepository;

        private Transaction _transaction01;
        private Transaction _transaction02;
        private Transaction _transaction03;

        public TransactionRepositoryTest()
        {
            _dbContext = TestContextProvider.GetDbContext();
            _transactionRepository = new TransactionRepository(_dbContext);
        }

        [TestMethod]
        public async Task Get_should_return_all_items_in_table()
        {
            await InitializeData();
            var allTransactions = await _transactionRepository.Get();

            Assert.AreEqual(3, allTransactions.Count);
        }

        [TestMethod]
        public async Task Get_BySku_should_return_items_with_specified_sku()
        {
            await InitializeData();
            var allTransactions = await _transactionRepository.Get("T123");

            Assert.AreEqual(2, allTransactions.Count);
        }

        [TestMethod]
        public async Task Set_should_remove_all_transactions()
        {
            await InitializeData();
            var transaction04 = new Transaction { Sku = "R001", Amount = 0m, Currency = "A" };
            var transaction05 = new Transaction { Sku = "R001", Amount = 0m, Currency = "A" };
            var transaction06 = new Transaction { Sku = "F002", Amount = 0m, Currency = "A" };
            var transaction07 = new Transaction { Sku = "F002", Amount = 0m, Currency = "A" };
            var transactions = new List<Transaction> { transaction04, transaction05, transaction06, transaction07 };

            await _transactionRepository.Set(transactions);

            var allTransactions = await _transactionRepository.Get();

            Assert.IsFalse(allTransactions.Any(r => r.Sku == "T123"));
            Assert.IsFalse(allTransactions.Any(r => r.Sku == "B001"));
        }

        [TestMethod]
        public async Task Set_should_add_new_rates()
        {
            await InitializeData();
            var transaction04 = new Transaction { Sku = "R001", Amount = 0m, Currency = "A" };
            var transaction05 = new Transaction { Sku = "R001", Amount = 0m, Currency = "A" };
            var transaction06 = new Transaction { Sku = "F002", Amount = 0m, Currency = "A" };
            var transaction07 = new Transaction { Sku = "F002", Amount = 0m, Currency = "A" };
            var transactions = new List<Transaction> { transaction04, transaction05, transaction06, transaction07 };

            await _transactionRepository.Set(transactions);

            var allTransactions = await _transactionRepository.Get();

            Assert.AreEqual(4, allTransactions.Count);
            Assert.AreEqual(2, allTransactions.Count(r => r.Sku == "R001"));
            Assert.AreEqual(2, allTransactions.Count(r => r.Sku == "F002"));
        }

        private async Task InitializeData()
        {
            _dbContext.Transactions.RemoveRange(_dbContext.Transactions);
            await _dbContext.SaveChangesAsync();

            _transaction01 = new Transaction { Sku = "T123", Amount = 11.3m, Currency = "A" };
            _transaction02 = new Transaction { Sku = "T123", Amount = 33.1m, Currency = "B" };
            _transaction03 = new Transaction { Sku = "B001", Amount = 22.5m, Currency = "C" };

            await _dbContext.Transactions.AddRangeAsync(new List<Transaction> { _transaction01, _transaction02, _transaction03 });
            await _dbContext.SaveChangesAsync();
        }
    }
}
