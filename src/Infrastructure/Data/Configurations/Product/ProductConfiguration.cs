namespace Infrastructure.Data.Configurations.Products;

public class ProductConfiguration
    : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");
        builder.HasKey(x => x.Id);

        builder.Property(p => p.Name)
            .IsRequired();

        builder.Property(p => p.Code)
            .IsRequired();

        builder.HasIndex(p => p.Code)
            .IsUnique();

        builder.Property(p => p.Code)
            .IsRequired();

        builder.Property(p => p.Crime)
            .IsRequired();

        builder.Property(p => p.Quantity)
            .HasColumnType("decimal(18,3)");


        builder.HasOne(x => x.Supplier)
           .WithMany(x => x.Products)
           .HasForeignKey(x => x.SupplierId);

        builder.HasQueryFilter(x => !x.IsDelete);
    }
}
