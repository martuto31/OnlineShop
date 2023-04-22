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
    public class RoleEntityTypeConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> role)
        {
            role.HasMany(x => x.Users)
                .WithOne(x => x.Role);
        }
    }
}
