namespace TechnicalTest.UnitTests.Utils
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using TechnicalTest.Infrastructure;

    public class TestContextProvider
    {
        private static TestDbContext _databaseContext;

        public static TestDbContext GetDbContext()
        {
            if (_databaseContext != null)
            {
                return _databaseContext;
            }

            var configuration = new ConfigurationBuilder().Build();
            var dbContextOptions = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase("" + DateTime.Now.ToFileTimeUtc())
                .Options;

            var context = new TestDbContext(configuration, dbContextOptions);
            //context.Database.EnsureCreated();
            _databaseContext = context;

            return context;
        }
    }
}
