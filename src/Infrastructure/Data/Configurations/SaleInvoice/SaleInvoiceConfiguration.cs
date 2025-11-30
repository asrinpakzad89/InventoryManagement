
namespace Infrastructure.Data.Configurations.SaleInvoices;

public class SaleInvoiceConfiguration
    : IEntityTypeConfiguration<SaleInvoice>
{
    public void Configure(EntityTypeBuilder<SaleInvoice> builder)
    {
        builder.ToTable("SaleInvoices");
        builder.HasKey(x => x.Id);

        builder.HasMany(x => x.Items)
            .WithOne(x => x.SaleInvoice)
            .HasForeignKey(x => x.SaleInvoiceId);

        builder.HasQueryFilter(x => !x.IsDelete);
    }
}
