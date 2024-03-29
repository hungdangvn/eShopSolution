﻿using eShopSolution.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.Data.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");
            builder.HasKey(t => t.Id);

            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(x => x.OrderDate);//.HasDefaultValue(DateTime.Now);  tam thoi bo

            builder.Property(x => x.ShipEmail)
                .IsRequired()                
                .IsUnicode(false)
                .HasMaxLength(50);//config truong varchar(50) thay vì nvarchar
            builder.Property(x => x.ShipAddress).IsRequired().HasMaxLength(200);
            builder.Property(x => x.ShipName).IsRequired().HasMaxLength(200);

            builder.Property(x => x.ShipPhoneNumber).IsRequired().HasMaxLength(200)
                .IsUnicode(false);

            builder.HasOne(x => x.AppUser).WithMany(x => x.Orders).HasForeignKey(x => x.UserId);
        }
    }
}
