namespace Domain;

public class StockTransaction
    : BaseEntity<int>
{
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;

    public decimal QuantityChange { get; set; }

    public decimal? OldPurchasePrice { get; set; }
    public decimal? NewPurchasePrice { get; set; }

    public decimal? OldSalePrice { get; set; }
    public decimal? NewSalePrice { get; set; }

    public SourceTypeEnum SourceType { get; set; }

    public int? SourceId { get; set; }

    public DateTime Date { get; set; } = DateTime.UtcNow;

    public string? Note { get; set; }
}