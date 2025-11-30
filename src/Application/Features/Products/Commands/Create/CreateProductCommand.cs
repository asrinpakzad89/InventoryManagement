namespace Application.Features.Products.Commands.Create;

public class CreateProductCommand : IRequest<ProductIdDto>
{
    public string Code { get; set; }
    public string Name { get; set; }
    public ProductTypeEnum Type { get; set; }

    public int? SupplierId { get; set; }
    public decimal? Crime { get; set; }
    public decimal Quantity { get; set; }
    public decimal PurchasePrice { get; set; }
    public decimal SellingPrice { get; set; }

}
