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
    public class ImageUriEntityTypeConfiguration : IEntityTypeConfiguration<ImageUri>
    {
        public void Configure(EntityTypeBuilder<ImageUri> image)
        {
            image.HasOne(x => x.Product)
                .WithMany(x => x.Pictures);
        }
    }
}
