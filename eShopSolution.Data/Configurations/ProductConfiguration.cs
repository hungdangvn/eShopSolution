using eShopSolution.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.Data.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Price)
                .IsRequired(); //Mac dinh IsRequired là true
            builder.Property(x => x.OriginalPrice)
                .IsRequired();
            builder.Property(x => x.SeoAlias)
                .IsRequired();
            builder.Property(x => x.DateCreated)
                .HasColumnType("Date");
            //    .HasDefaultValueSql("GetDate()"); //Bo di vi moi lan update database no cu change
            builder.Property(x => x.Stock)
                .IsRequired()
                .HasDefaultValue(0);
            builder.Property(x => x.ViewCount)
                .IsRequired()
                .HasDefaultValue(0);
            
    }
    }
}
