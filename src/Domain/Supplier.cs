namespace Domain;

public class Supplier
    : BaseEntity<int>
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Contact { get; set; }

    public ICollection<Product> Products { get; set; } = new List<Product>();
}