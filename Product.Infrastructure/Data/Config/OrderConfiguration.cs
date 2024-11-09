using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Product.Core.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Infrastructure.Data.Config
{
    public class OrderConfiguration :IEntityTypeConfiguration<Core.Entities.Order.Order>
    {
        public void Configure(EntityTypeBuilder<Core.Entities.Order.Order> builder)
        {
            builder.OwnsOne(x => x.ShipToAddress, n => { n.WithOwner(); });
            builder.Property(x => x.OrderStatus).HasConversion(o => o.ToString(), o => (OrderStatus)Enum.Parse(typeof(OrderStatus), o.ToString()));
            builder.HasMany(x => x.OrderItems).WithOne().OnDelete(DeleteBehavior.Cascade);
            //builder.Property(x => x.Subtotal).HasColumnType("deciaml(18,2)");
        }

    }
}
