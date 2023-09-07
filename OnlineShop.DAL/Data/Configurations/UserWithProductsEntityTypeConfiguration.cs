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
    public class UserWithProductsEntityTypeConfiguration : IEntityTypeConfiguration<UserWithProducts>
    {
        public void Configure(EntityTypeBuilder<UserWithProducts> usersWithProducts)
        {
            usersWithProducts.HasKey(ps => new { ps.UserId, ps.ProductId });

            usersWithProducts.HasOne(x => x.Product)
                .WithMany(x => x.Users)
                .HasForeignKey(x => x.ProductId);

            usersWithProducts.HasOne(x => x.User)
                .WithMany(x => x.Products)
                .HasForeignKey(x => x.UserId);
        }
    }
}
