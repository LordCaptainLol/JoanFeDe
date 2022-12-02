namespace TechnicalTest.Domain.Transactions
{
    using System.ComponentModel.DataAnnotations;

    public class Transaction
    {
        [Key]
        public int Id { get; set; }

        public string Sku { get; set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; }
    }
}
