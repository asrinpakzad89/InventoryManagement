
namespace Infrastructure.Data.Configurations.SaleInvoiceItems;

public class SaleInvoiceItemConfiguration
    : IEntityTypeConfiguration<SaleInvoiceItem>
{
    public void Configure(EntityTypeBuilder<SaleInvoiceItem> builder)
    {
        builder.ToTable("SaleInvoiceItems");
        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.SaleInvoice)
           .WithMany(x => x.Items)
           .HasForeignKey(x => x.SaleInvoiceId);

        builder.HasOne(x => x.Product)
           .WithMany()
           .HasForeignKey(x => x.ProductId);

        builder.HasQueryFilter(x => !x.IsDelete);
    }
}
