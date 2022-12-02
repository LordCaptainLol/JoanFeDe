namespace TechnicalTest.Infrastructure
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using TechnicalTest.Domain.Rates;
    using TechnicalTest.Domain.Transactions;

    public class TestDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public DbSet<CurrencyRate> Rates { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        public TestDbContext(
            IConfiguration configuration,
            DbContextOptions<TestDbContext> options) : base(options)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connString = _configuration.GetConnectionString("TestConnectionString");

            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(connString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CurrencyRate>();
            modelBuilder.Entity<Transaction>();
        }
    }
}
