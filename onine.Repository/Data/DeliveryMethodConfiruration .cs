using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using onine.core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace onine.Repository.Data
{
    public class DeliveryMethodConfiruration : IEntityTypeConfiguration<DeliveryMethod>
    {
        public void Configure(EntityTypeBuilder<DeliveryMethod> builder)
        {
            builder.ToTable("DeliveryMethods");
            builder.Property(d => d.ShortName).IsRequired().HasMaxLength(100);
            builder.Property(d => d.Description).IsRequired().HasMaxLength(200);

            builder.Property(d => d.Price).HasColumnType("decimal(18,2)");
            builder.Property(d => d.DeliveryTime).IsRequired().HasMaxLength(100);

        }
    }
}
