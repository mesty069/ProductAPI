using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Product.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Infrastructure.Data.Config
{
    public class CategoryConfigration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.Name).HasMaxLength(50);
            builder.Property(x => x.Description).HasMaxLength(50);

            builder.HasData(
                new Category { Id = 1, Name = "類別 1", Description = "描述 1" },
                new Category { Id = 2, Name = "類別 2", Description = "描述 2" },
                new Category { Id = 3, Name = "類別 3", Description = "描述 3" },
                new Category { Id = 4, Name = "類別 4", Description = "描述 4" }
                );
        }
    }
}
