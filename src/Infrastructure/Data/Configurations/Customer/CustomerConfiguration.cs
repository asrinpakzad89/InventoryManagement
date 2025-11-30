namespace Infrastructure.Data.Configurations.Customers;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customers");
        builder.HasKey(x => x.Id);

        builder.HasMany(x => x.SaleInvoices)
            .WithOne(x => x.Customer)
            .HasForeignKey(x => x.CustomerId);

        builder.HasQueryFilter(x => !x.IsDelete);
    }
}
