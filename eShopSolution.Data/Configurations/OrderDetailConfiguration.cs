using eShopSolution.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.Data.Configurations
{
    public class OrderDetailConfiguration : IEntityTypeConfiguration<OrderDetail>
    {
        public void Configure(EntityTypeBuilder<OrderDetail> builder)
        {
            builder.ToTable("OrderDetails");

            builder.HasKey(t => new { t.OrderId, t.ProductId });

            builder.HasOne(t => t.Order)            //1 Order có nhiều OrderDetails thông qua khóa ngoại OrderId
                .WithMany(od => od.OrderDetails)
                .HasForeignKey(t => t.OrderId);

            builder.HasOne(t => t.Product)      //1 Product xuat hien nhieu lan trong bang OrderDetails thông qua khóa ngoại ProductId
                .WithMany(pd => pd.OrderDetails)
                .HasForeignKey(t => t.ProductId);
        }
    }
}
