
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using onine.core.Entites;

namespace onine.Repository.Data
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Property(o => o.BuyerEmail)
                .IsRequired()
                .HasMaxLength(256);

            builder.Property(o => o.Subtotal)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.HasMany(o => o.orderItems)
                .WithOne(i => i.Order)
                .HasForeignKey(i => i.OrderId);

            builder.Property(o => o.orderStatus)
                .HasConversion<string>()
                .IsRequired();

            builder.Property(o => o.DeliveryCost)
                .HasPrecision(18, 2);

            builder.OwnsOne(o => o.ShipToAddress, address =>
            {
                address.Property(a => a.FirstName)
                    .IsRequired()
                    .HasMaxLength(100);

                address.Property(a => a.LastName)
                    .IsRequired()
                    .HasMaxLength(100);

                address.Property(a => a.Street)
                    .IsRequired()
                    .HasMaxLength(200);

                address.Property(a => a.City)
                    .IsRequired()
                    .HasMaxLength(100);

                address.Property(a => a.Country)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            builder.Navigation(o => o.ShipToAddress)
                .IsRequired();
        }
    }
}
