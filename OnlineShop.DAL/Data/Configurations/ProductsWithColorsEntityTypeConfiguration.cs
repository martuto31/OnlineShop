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
    public class ProductsWithColorsEntityTypeConfiguration : IEntityTypeConfiguration<ProductsWithColors>
    {
        public void Configure(EntityTypeBuilder<ProductsWithColors> productsWithColors)
        {
            productsWithColors.HasKey(ps => new { ps.ProductId, ps.ProductColorsId });

            productsWithColors.HasOne(x => x.Product)
                .WithMany(x => x.ProductsWithColors)
                .HasForeignKey(x => x.ProductId);

            productsWithColors.HasOne(x => x.ProductColors)
                .WithMany(x => x.ProductsWithColors)
                .HasForeignKey(x => x.ProductColorsId);
        }
    }
}
