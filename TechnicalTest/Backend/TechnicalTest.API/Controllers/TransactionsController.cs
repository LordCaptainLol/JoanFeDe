namespace TechnicalTest.API.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Text.Json;
    using TechnicalTest.API.Services.Parsers;
    using TechnicalTest.API.Services.Rates;
    using TechnicalTest.API.Services.Transactions.Exceptions;
    using TechnicalTest.Domain.Transactions;

    [ApiController]
    [Route("[controller]")]
    public class TransactionsController : ControllerBase
    {

        private readonly ILogger<TransactionsController> _logger;
        private readonly ITransactionService _transactionService;
        private readonly IJsonFileParser<Transaction> _jsonFileParser;

        public TransactionsController(
            ILogger<TransactionsController> logger,
            ITransactionService transactionService,
            IJsonFileParser<Transaction> jsonFileParser)
        {
            _logger = logger;
            _transactionService = transactionService;
            _jsonFileParser = jsonFileParser;
        }

        [HttpPost]
        [Route("Add/Json")]
        public async Task AddFromJsonFile(IFormFile formFile)
        {
            List<Transaction> transactions = null;
            try
            {
                transactions = _jsonFileParser.ParseFromFile(formFile);
            }
            catch (JsonException ex)
            {
                _logger.LogTrace($"Invalid json file", ex.Message);
                return;
            }

            if (transactions == null)
            {
                _logger.LogTrace($"Empty transactions list");
                return;
            }
            await _transactionService.Set(transactions).ConfigureAwait(false);
        }

        [HttpPost]
        [Route("Add")]
        public async Task Add(List<Transaction> transactions)
        {
            if (transactions == null)
            {
                _logger.LogTrace($"Empty transactions list");
                return;
            }
            await _transactionService.Set(transactions).ConfigureAwait(false);
        }

        [HttpGet]
        [Route("Get")]
        public async Task<string> Get()
        {
            var rates = await _transactionService.Get().ConfigureAwait(false);

            return JsonSerializer.Serialize(rates);
        }

        [HttpGet]
        [Route("TransactionAmount/{sku}")]
        public async Task<decimal> GetTransactionAmount(string sku)
        {
            var toCurrency = "EUR";

            try
            {
                var transactionAmount = await _transactionService.GetTransactionAmount(sku, toCurrency).ConfigureAwait(false);
                return Math.Round(transactionAmount, 2);
            }
            catch (ConversionNotFoundException ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
        }
    }
}
