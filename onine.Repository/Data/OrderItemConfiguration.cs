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
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.Property(o => o.Price).HasColumnType("decimal(18,2)");
   
            builder.OwnsOne(i=> i.productItemOrdered, io =>
            {
                io.WithOwner();
                io.Property(i => i.ProductId).IsRequired();
                io.Property(i => i.ProductName).IsRequired();
                io.Property(i => i.PictureUrl).IsRequired();
            });
   

        }
}
}
