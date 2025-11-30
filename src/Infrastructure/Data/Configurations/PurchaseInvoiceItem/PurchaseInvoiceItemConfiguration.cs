
namespace Infrastructure.Data.Configurations.PurchaseInvoiceItems;

public class PurchaseInvoiceItemConfiguration
    : IEntityTypeConfiguration<PurchaseInvoiceItem>
{
    public void Configure(EntityTypeBuilder<PurchaseInvoiceItem> builder)
    {
        builder.ToTable("PurchaseInvoiceItems");
        builder.HasKey(x => x.Id);

        builder.Property(i => i.Quantity)
            .HasColumnType("decimal(18,3)");

        builder.Property(i => i.Quantity)
            .HasColumnType("decimal(18,3)");


        builder.HasOne(x => x.PurchaseInvoice)
           .WithMany(x=>x.Items)
           .HasForeignKey(x => x.PurchaseInvoiceId);

        builder.HasOne(x => x.Product)
           .WithMany()
           .HasForeignKey(x => x.ProductId);

        builder.HasQueryFilter(x => !x.IsDelete);

    }
}
