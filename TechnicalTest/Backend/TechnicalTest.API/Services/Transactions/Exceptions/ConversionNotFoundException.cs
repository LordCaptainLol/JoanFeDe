namespace TechnicalTest.API.Services.Transactions.Exceptions
{
    public class ConversionNotFoundException : Exception
    {
        public ConversionNotFoundException(string fromCurrency, string toCurrency) : base(GetMessage(fromCurrency, toCurrency))
        {
        }

        private static string GetMessage(string fromCurrency, string toCurrency)
        {
            return $"There's no conversion from '{fromCurrency}' to '{toCurrency}'";
        }
    }
}
