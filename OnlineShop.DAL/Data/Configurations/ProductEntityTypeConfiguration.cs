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
    public class ProductEntityTypeConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> product)
        {
            product
                .HasIndex(x => x.Id)
                .IsUnique();

            product.Property(x => x.Name)
                .IsRequired(true)
                .HasMaxLength(ConfigurationConstants.ProductNameMaxLength)
                .IsUnicode(true);

            product.HasMany(x => x.Reviews)
                .WithOne(x => x.Product);

            product.Property(x => x.Sales)
                .HasDefaultValue(0);

            product.Property(x => x.CreatedOn)
                .HasDefaultValue(DateTime.UtcNow);
        }
    }
}
