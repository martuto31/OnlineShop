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
    public class ProductColorEntityTypeConfiguration : IEntityTypeConfiguration<ProductColors>
    {
        public void Configure(EntityTypeBuilder<ProductColors> productColor)
        {
            productColor.HasOne(x => x.Product)
                .WithMany(x => x.ProductColors);

            productColor.Property(x => x.Color)
                .IsRequired();
        }
    }
}
