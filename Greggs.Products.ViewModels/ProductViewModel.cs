namespace Greggs.Products.ViewModels;

/// <summary>
/// I decided to create a separate ViewModels project because despite the products class being simple in this example, I've found 
/// it common for the Model and ViewModels classes to be quite different in a real world application. Without a ViewModel class,
/// we would be risking serving properties to the client that are unnecessary.
/// </summary>
public class ProductViewModel
{
    public string Name { get; set; } = string.Empty;
    public decimal PriceInPounds { get; set; }
    public decimal PriceInForeignCurrency { get; set; }
    public string ForeignCurrencyCode { get; set; } = string.Empty;
}