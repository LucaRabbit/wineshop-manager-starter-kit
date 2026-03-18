namespace WineshopManagerStarterKit.Models;

public class Wine
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public TypeWine Type { get; set; }
    public int quantity { get; set; }
    public float Price { get; set; }
    public float TaxRate { get; set; }
    public int WarningPoint { get; set; }

}
