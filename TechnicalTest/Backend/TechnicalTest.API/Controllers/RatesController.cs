namespace TechnicalTest.API.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Text.Json;
    using TechnicalTest.API.Services.Parsers;
    using TechnicalTest.API.Services.Rates;
    using TechnicalTest.Domain.Rates;

    [ApiController]
    [Route("[controller]")]
    public class RatesController : ControllerBase
    {
        private readonly ILogger<RatesController> _logger;
        private readonly ICurrencyRateService _rateService;
        private readonly IJsonFileParser<CurrencyRate> _jsonFileParser;

        public RatesController(
            ILogger<RatesController> logger,
            ICurrencyRateService rateService,
            IJsonFileParser<CurrencyRate> jsonFileParser)
        {
            _logger = logger;
            _rateService = rateService;
            _jsonFileParser = jsonFileParser;
        }

        [HttpPost]
        [Route("Add/Json")]
        public async Task AddFromJsonFile(IFormFile formFile)
        {
            List<CurrencyRate> rates = null;
            try
            {
                rates = _jsonFileParser.ParseFromFile(formFile);
            }
            catch (JsonException ex)
            {
                _logger.LogTrace($"Invalid json file", ex.Message);
                return;
            }

            if (rates == null)
            {
                _logger.LogTrace($"Empty rates file");
                return;
            }

            await _rateService.Set(rates).ConfigureAwait(false);
        }

        [HttpPost]
        [Route("Add")]
        public async Task Add(List<CurrencyRate> rates)
        {
            if (rates == null)
            {
                _logger.LogTrace($"Empty rates list");
                return;
            }

            await _rateService.Set(rates).ConfigureAwait(false);
        }

        [HttpGet]
        [Route("Get")]
        public async Task<string> Get()
        {
            var rates = await _rateService.Get().ConfigureAwait(false);

            return JsonSerializer.Serialize(rates);
        }
    }
}