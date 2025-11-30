namespace Infrastructure.Data.Configurations.Suppliers;

public class SupplierConfiguration 
    : IEntityTypeConfiguration<Supplier>
{
    public void Configure(EntityTypeBuilder<Supplier> builder)
    {
        builder.ToTable("Suppliers");
        builder.HasKey(x => x.Id);

        builder.HasMany(x => x.Products)
           .WithOne(x=>x.Supplier)
           .HasForeignKey(x => x.SupplierId);

        builder.HasQueryFilter(x => !x.IsDelete);
    }
}
