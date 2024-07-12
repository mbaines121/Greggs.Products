namespace Greggs.Products.Models
{
    public class CurrencyPair
    {
        public string BaseCurrency { get; set; }
        public string QuoteCurrency { get; set; }
        public decimal ExchangeRate { get; set; }
        public DateTime ValidFromDate { get; set; }
    }
}
