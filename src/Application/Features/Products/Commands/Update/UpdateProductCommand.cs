namespace Application.Features.Products.Commands.Update;

public class UpdateProductCommand :IRequest<Unit>
{
    public int Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public ProductTypeEnum Type { get; set; }
    public int? SupplierId { get; set; }
    public decimal? Crime { get; set; }
    public decimal Quantity { get; set; }
    public decimal PurchasePrice { get; set; }
    public decimal SellingPrice { get; set; }

}
