using eShopSolution.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.Data.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppConfig>().HasData(
               new AppConfig() { Key = "HomeTitle", Value = "This is homepage of eShop" },
               new AppConfig() { Key = "HomeKeyword", Value = "This is keyword of eShop" },
               new AppConfig() { Key = "HomeTag", Value = "This is tag of eShop" },
               new AppConfig() { Key = "HomeTitle2", Value = "This is Title2 of eShop" },
               new AppConfig() { Key = "HomeDescription", Value = "This is description of eShop" }
               );

            modelBuilder.Entity<Language>().HasData(
                new Language() { Id = "vi", Name = "Tiếng Việt", IsDefault = true },
                new Language() { Id = "en", Name = "English", IsDefault = false },
                new Language() { Id = "ru", Name = "Russian", IsDefault = false }
                );

            modelBuilder.Entity<Category>().HasData(
                new Category()
                {
                    Id = 1,
                    IsShowOnHome = true,
                    ParentId = null,
                    SortOrder = 1,
                    Status = Enums.Status.Active
                },
                new Category()
                {
                    Id = 2,
                    IsShowOnHome = true,
                    ParentId = null,
                    SortOrder = 2,
                    Status = Enums.Status.Active,
                });
            modelBuilder.Entity<CategoryTranslation>().HasData(
                new CategoryTranslation() { Id = 1, CategoryId = 1, Name = "Sơ mi nam", LanguageId = "vi", SeoAlias = "so-mi-nam", SeoDescription = "Sản phẩm thời trang nam", SeoTitle = "Sản phẩm thời trang nam" },
                new CategoryTranslation() { Id = 2, CategoryId = 1, Name = "Men Shirt", LanguageId = "en", SeoAlias = "men-shirt", SeoDescription = "The shirt product for men", SeoTitle = "The shirt product for men" },
                new CategoryTranslation() { Id = 3, CategoryId = 1, Name = "Мужская рубашка", LanguageId = "ru", SeoAlias = "Мужская-рубашка", SeoDescription = "Рубашка мужская", SeoTitle = "Рубашка мужская" },
                new CategoryTranslation() { Id = 4, CategoryId = 2, Name = "Váy nữ", LanguageId = "vi", SeoAlias = "vay-nu", SeoDescription = "Sản phẩm thời trang nữ", SeoTitle = "Sản phẩm thời trang nữ" },
                new CategoryTranslation() { Id = 5, CategoryId = 2, Name = "Women dress", LanguageId = "en", SeoAlias = "women-dress", SeoDescription = "The dress product for women", SeoTitle = "The dress product for women" },
                new CategoryTranslation() { Id = 6, CategoryId = 2, Name = "Женское платье", LanguageId = "ru", SeoAlias = "Женское-платье", SeoDescription = "Женское платье", SeoTitle = "Женское платье" }
                );
            modelBuilder.Entity<Product>().HasData(
                new Product()
                {
                    Id = 1,
                    DateCreated = DateTime.Now,
                    OriginalPrice = 10000,
                    Price = 20000,
                    SeoAlias = "",
                    Stock = 0,
                    ViewCount = 0
                },
                new Product()
                {
                    Id = 2,
                    DateCreated = DateTime.Now,
                    OriginalPrice = 15000,
                    Price = 23000,
                    SeoAlias = "",
                    Stock = 0,
                    ViewCount = 0
                });
            modelBuilder.Entity<ProductTranslation>().HasData(
                new ProductTranslation() { Id = 1, ProductId = 1, Name = "Áo sơ mi nam trắng Việt Tiến", LanguageId = "vi", SeoAlias = "ao-so-mi-nam-trang-viet-tien", SeoDescription = "Sản phẩm thời trang nam", SeoTitle = "Sản phẩm thời trang nam", Details = "Mô tả sản phẩm", Description = "" },
                new ProductTranslation() { Id = 2, ProductId = 1, Name = "Viet Tien men shirt", LanguageId = "en", SeoAlias = "viet-tien-men-shirt", SeoDescription = "The shirt product for men", SeoTitle = "The shirt product for men", Details = "Product description", Description = "" },
                new ProductTranslation() { Id = 3, ProductId = 1, Name = "Мужская рубашка", LanguageId = "ru", SeoAlias = "viet-tien-Мужская-рубашка", SeoDescription = "Рубашка мужская", SeoTitle = "Рубашка мужская", Details = "Описание продукта", Description = "" },
                new ProductTranslation() { Id = 4, ProductId = 2, Name = "Váy hồng Việt Fashion", LanguageId = "vi", SeoAlias = "vay-hong-viet-fashion", SeoDescription = "Sản phẩm thời trang nữ", SeoTitle = "Sản phẩm thời trang nữ", Details = "Mô tả sản phẩm", Description = "" },
                new ProductTranslation() { Id = 5, ProductId = 2, Name = "Viet fashio pink dress", LanguageId = "en", SeoAlias = "viet-fashio-pink-dress", SeoDescription = "The dress product for women", SeoTitle = "The dress product for women", Details = "Product description", Description = "" },
                new ProductTranslation() { Id = 6, ProductId = 2, Name = "Розовое платье вьетнамской моды", LanguageId = "ru", SeoAlias = "Розовое-платье-вьетнамской-моды", SeoDescription = "Женское платье", SeoTitle = "Женское платье", Details = "Описание продукта", Description = "" }
                );
            modelBuilder.Entity<ProductInCategory>().HasData(
                new ProductInCategory() { ProductId = 1, CategoryId = 1 },
                new ProductInCategory() { ProductId = 2, CategoryId = 2 }
                );

            // any guid
            var ROLE_ID = new Guid("DECE1100-4907-4145-8DB9-D0E6E18F80E5");
            var ADMIN_ID = new Guid("83D9E276-25C4-4EF9-82FC-27A678BFC6CE");

            modelBuilder.Entity<AppRole>().HasData(
                new AppRole() { Id = ROLE_ID, Name = "admin", NormalizedName = "admin", Description = "Administrator role" },
                new AppRole() { Id = new Guid("C649E019-9887-4739-9404-A0DDD827690D"), Name = "moderator", NormalizedName = "moderator", Description = "Kiem duyet role" },
                new AppRole() { Id = new Guid("3D474CD6-912B-4DF7-BBD9-DA31FA3061C6"), Name = "user", NormalizedName = "user", Description = "user using app" }
                );

            var hasher = new PasswordHasher<AppUser>();
            modelBuilder.Entity<AppUser>().HasData(new AppUser
            {
                Id = ADMIN_ID,
                UserName = "admin",
                NormalizedUserName = "admin",
                Email = "dangmanh.hung@msn.com",
                NormalizedEmail = "dangmanh.hung@msn.com",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "Admin@123"),
                SecurityStamp = string.Empty,
                FirtName = "Hung",
                LastName = "Dang",
                Dob = new DateTime(1976, 03, 10)
            });

            modelBuilder.Entity<IdentityUserRole<Guid>>().HasData(new IdentityUserRole<Guid>
            {
                RoleId = ROLE_ID,
                UserId = ADMIN_ID
            });
        }
    }
}