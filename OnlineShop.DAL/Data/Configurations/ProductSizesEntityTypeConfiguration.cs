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
    public class ProductSizesEntityTypeConfiguration : IEntityTypeConfiguration<ProductSizes>
    {
        public void Configure(EntityTypeBuilder<ProductSizes> productSizes)
        {
            productSizes.Property(x => x.Size)
                .IsRequired();
        }
    }
}
