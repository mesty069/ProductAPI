using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Product.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Infrastructure.Data.Config
{
    public class ProductConfigration : IEntityTypeConfiguration<Products>
    {
        public void Configure(EntityTypeBuilder<Products> builder)
        {
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.Name).HasMaxLength(128);
            builder.Property(x => x.Description).HasMaxLength(128);
            builder.Property(x => x.Price).HasColumnType("decimal(18,2)");

            //Seeding 
            builder.HasData(
                new Products { Id = 1, Name = "商品 1", Description = "描述 1", Price = 100, CategoryId = 1, ProductPicture = "https://" },
                new Products { Id = 2, Name = "商品 2", Description = "描述 2", Price = 300, CategoryId = 1, ProductPicture = "https://" },
                new Products { Id = 3, Name = "商品 3", Description = "描述 3", Price = 500, CategoryId = 3, ProductPicture = "https://" },
                new Products { Id = 4, Name = "商品 4", Description = "描述 4", Price = 900, CategoryId = 2, ProductPicture = "https://" }
                );
        }
    }
}
