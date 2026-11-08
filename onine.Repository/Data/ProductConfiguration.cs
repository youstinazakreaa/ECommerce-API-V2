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
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasOne(p => p.ProductType)
                .WithMany();
            builder.HasOne(p => p.ProductBrand)
                .WithMany();
            builder.Property(p => p.Price)
                .HasColumnType("decimal(18,2)");
            builder.Property(p => p.StockQuantatiy)
                .IsRequired()
                .HasDefaultValue(0);
            builder .ToTable(T=> T.HasCheckConstraint("CK_Product_StockQuantatiy", "[StockQuantatiy] >= 0"));



        }
    }
}
