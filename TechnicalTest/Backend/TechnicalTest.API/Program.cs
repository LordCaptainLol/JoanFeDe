namespace TechnicalTest.API
{
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using TechnicalTest.API.Services.Parsers;
    using TechnicalTest.API.Services.Rates;
    using TechnicalTest.Domain.Rates;
    using TechnicalTest.Domain.Transactions;
    using TechnicalTest.Infrastructure;
    using TechnicalTest.Infrastructure.Repositories;

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddScoped<ITransactionService, TransactionService>();
            builder.Services.AddScoped<ICurrencyRateService, CurrencyRateService>();
            builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
            builder.Services.AddScoped<ICurrencyRateRepository, CurrencyRateRepository>();
            builder.Services.AddScoped<ICurrencyConverter, CurrencyConverter>();
            builder.Services.AddScoped<IXmlFileParser<CurrencyRate>, XmlFileParser<CurrencyRate>>();
            builder.Services.AddScoped<IXmlFileParser<Transaction>, XmlFileParser<Transaction>>();
            builder.Services.AddScoped<IJsonFileParser<CurrencyRate>, JsonFileParser<CurrencyRate>>();
            builder.Services.AddScoped<IJsonFileParser<Transaction>, JsonFileParser<Transaction>>();

            builder.Services.AddDbContext<TestDbContext>();

            var app = builder.Build()
                .MigrateDatabase();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}

