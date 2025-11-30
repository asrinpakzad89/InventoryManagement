namespace Infrastructure.Data.Configurations.StockTransactions;

public class StockTransactionConfiguration 
    : IEntityTypeConfiguration<StockTransaction>
{
    public void Configure(EntityTypeBuilder<StockTransaction> builder)
    {
        builder.ToTable("StockTransactions");
        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.Product)
           .WithMany()
           .HasForeignKey(x => x.ProductId);

        builder.HasQueryFilter(x => !x.IsDelete);
    }
}
