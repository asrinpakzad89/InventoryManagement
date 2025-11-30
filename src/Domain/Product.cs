namespace Domain;

public class Product
    : BaseEntity<int>
{
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public ProductTypeEnum Type { get; set; }
    public decimal? Crime { get; set; }

    public int? SupplierId { get; set; }
    public Supplier? Supplier { get; set; }

    public decimal Quantity { get; set; }

    public decimal PurchasePrice { get; set; }
    public decimal SellingPrice { get; set; }

}
