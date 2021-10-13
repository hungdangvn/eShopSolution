using eShopSolution.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.Data.Configurations
{
    public class ProductInCategoryConfiguration : IEntityTypeConfiguration<ProductInCategory>
    {
        public void Configure(EntityTypeBuilder<ProductInCategory> builder)
        {
            builder.ToTable("ProductInCatergories");
            builder.HasKey(t => new { t.CategoryId, t.ProductId });

            builder.HasOne(t => t.Product).WithMany(pc => pc.ProductInCategories) //Khoa ngoai ProducId: Mot Product thuoc nhieu Catergory
                .HasForeignKey(pc => pc.ProductId); //Tro den ProducId

            builder.HasOne(t => t.Category).WithMany(pc => pc.ProductInCategories) //Khoa ngoai CategoryId: Mot Catergory co nhieu Product
                .HasForeignKey(pc => pc.CategoryId); //Tro de Catergory
        }
    }
}
