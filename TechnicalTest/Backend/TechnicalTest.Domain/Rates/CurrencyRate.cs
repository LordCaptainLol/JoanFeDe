namespace TechnicalTest.Domain.Rates
{
    using System.ComponentModel.DataAnnotations;

    public class CurrencyRate
    {
        [Key]
        public int Id { get; set; }

        public string From { get; set; }

        public string To { get; set; }

        public decimal Rate { get; set; }
    }
}
