using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eShopSolution.Data.Migrations
{
    public partial class SeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AppConfigs",
                columns: new[] { "Key", "Value" },
                values: new object[,]
                {
                    { "HomeTitle", "This is homepage of eShop" },
                    { "HomeKeyword", "This is keyword of eShop" },
                    { "HomeTag", "This is tag of eShop" },
                    { "HomeTitle2", "This is Title2 of eShop" },
                    { "HomeDescription", "This is description of eShop" }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "IsShowOnHome", "ParentId", "SortOrder", "Status" },
                values: new object[,]
                {
                    { 1, true, null, 1, 1 },
                    { 2, true, null, 2, 1 }
                });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Id", "IsDefault", "Name" },
                values: new object[,]
                {
                    { "vi-VN", true, "Tiếng Việt" },
                    { "en-US", false, "Tiếng Anh" },
                    { "ru-RU", false, "Tiếng Nga" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "DateCreate", "OriginalPrice", "Price", "SeoAlias" },
                values: new object[,]
                {
                    { 1, new DateTime(2021, 3, 29, 14, 59, 5, 112, DateTimeKind.Local).AddTicks(3879), 10000m, 20000m, "" },
                    { 2, new DateTime(2021, 3, 29, 14, 59, 5, 113, DateTimeKind.Local).AddTicks(8098), 15000m, 23000m, "" }
                });

            migrationBuilder.InsertData(
                table: "CategoryTranslations",
                columns: new[] { "Id", "CategoryId", "LanguageId", "Name", "SeoAlias", "SeoDescription", "SeoTitle" },
                values: new object[,]
                {
                    { 1, 1, "vi-VN", "Sơ mi nam", "so-mi-nam", "Sản phẩm thời trang nam", "Sản phẩm thời trang nam" },
                    { 4, 2, "vi-VN", "Váy nữ", "vay-nu", "Sản phẩm thời trang nữ", "Sản phẩm thời trang nữ" },
                    { 2, 1, "en-US", "Men Shirt", "men-shirt", "The shirt product for men", "The shirt product for men" },
                    { 5, 2, "en-US", "Women dress", "women-dress", "The dress product for women", "The dress product for women" },
                    { 3, 1, "ru-RU", "Мужская рубашка", "Мужская-рубашка", "Рубашка мужская", "Рубашка мужская" },
                    { 6, 2, "ru-RU", "Женское платье", "Женское-платье", "Женское платье", "Женское платье" }
                });

            migrationBuilder.InsertData(
                table: "ProductInCatergories",
                columns: new[] { "CategoryId", "ProductId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 2 }
                });

            migrationBuilder.InsertData(
                table: "ProductTranslations",
                columns: new[] { "Id", "Description", "Details", "LanguageId", "Name", "ProductId", "SeoAlias", "SeoDescription", "SeoTitle" },
                values: new object[,]
                {
                    { 1, "", "Mô tả sản phẩm", "vi-VN", "Áo sơ mi nam trắng Việt Tiến", 1, "ao-so-mi-nam-trang-viet-tien", "Sản phẩm thời trang nam", "Sản phẩm thời trang nam" },
                    { 2, "", "Product description", "en-US", "Viet Tien men shirt", 1, "viet-tien-men-shirt", "The shirt product for men", "The shirt product for men" },
                    { 3, "", "Описание продукта", "ru-RU", "Мужская рубашка", 1, "viet-tien-Мужская-рубашка", "Рубашка мужская", "Рубашка мужская" },
                    { 4, "", "Mô tả sản phẩm", "vi-VN", "Váy hồng Việt Fashion", 2, "vay-hong-viet-fashion", "Sản phẩm thời trang nữ", "Sản phẩm thời trang nữ" },
                    { 5, "", "Product description", "en-US", "Viet fashio pink dress", 2, "viet-fashio-pink-dress", "The dress product for women", "The dress product for women" },
                    { 6, "", "Описание продукта", "ru-RU", "Розовое платье вьетнамской моды", 2, "Розовое-платье-вьетнамской-моды", "Женское платье", "Женское платье" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AppConfigs",
                keyColumn: "Key",
                keyValue: "HomeDescription");

            migrationBuilder.DeleteData(
                table: "AppConfigs",
                keyColumn: "Key",
                keyValue: "HomeKeyword");

            migrationBuilder.DeleteData(
                table: "AppConfigs",
                keyColumn: "Key",
                keyValue: "HomeTag");

            migrationBuilder.DeleteData(
                table: "AppConfigs",
                keyColumn: "Key",
                keyValue: "HomeTitle");

            migrationBuilder.DeleteData(
                table: "AppConfigs",
                keyColumn: "Key",
                keyValue: "HomeTitle2");

            migrationBuilder.DeleteData(
                table: "CategoryTranslations",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "CategoryTranslations",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "CategoryTranslations",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "CategoryTranslations",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "CategoryTranslations",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "CategoryTranslations",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "ProductInCatergories",
                keyColumns: new[] { "CategoryId", "ProductId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "ProductInCatergories",
                keyColumns: new[] { "CategoryId", "ProductId" },
                keyValues: new object[] { 2, 2 });

            migrationBuilder.DeleteData(
                table: "ProductTranslations",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ProductTranslations",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ProductTranslations",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ProductTranslations",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "ProductTranslations",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "ProductTranslations",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyValue: "en-US");

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyValue: "ru-RU");

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyValue: "vi-VN");

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
