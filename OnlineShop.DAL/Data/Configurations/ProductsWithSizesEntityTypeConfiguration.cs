using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.DAL.Data.Configurations
{
    public class ProductsWithSizesEntityTypeConfiguration : IEntityTypeConfiguration<ProductsWithSizes>
    {
        public void Configure(EntityTypeBuilder<ProductsWithSizes> productsWithSizes)
        {
            productsWithSizes.HasKey(ps => new { ps.ProductId, ps.ProductSizesId });

            productsWithSizes.HasOne(x => x.Product)
                .WithMany(x => x.ProductsWithSizes)
                .HasForeignKey(x => x.ProductId);

            productsWithSizes.HasOne(x => x.ProductSizes)
                .WithMany(x => x.ProductsWithSizes)
                .HasForeignKey(x => x.ProductSizesId);
        }
    }
}
