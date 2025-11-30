
namespace Infrastructure.Data.Configurations.PurchaseInvoices;

public class PurchaseInvoiceConfiguration
    : IEntityTypeConfiguration<PurchaseInvoice>
{
    public void Configure(EntityTypeBuilder<PurchaseInvoice> builder)
    {
        builder.ToTable("PurchaseInvoices");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.InvoiceNumber)
            .IsUnicode();

        builder.HasOne(x => x.Supplier)
           .WithMany()
           .HasForeignKey(x => x.SupplierId);

        builder.HasMany(x => x.Items)
            .WithOne(x => x.PurchaseInvoice)
            .HasForeignKey(x => x.PurchaseInvoiceId);

        builder.HasQueryFilter(x => !x.IsDelete);
    }
}
