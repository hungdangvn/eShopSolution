using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eShopSolution.Data.Migrations
{
    public partial class SeedingMoreRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("dece1100-4907-4145-8db9-d0e6e18f80e5"),
                column: "ConcurrencyStamp",
                value: "c8d0e9f5-fc76-47b0-8471-58be5e52863b");

            migrationBuilder.InsertData(
                table: "AppRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Description", "Name", "NormalizedName" },
                values: new object[] { new Guid("c649e019-9887-4739-9404-a0ddd827690d"), "b6903666-b4c3-4936-85bc-046e4769ae2b", "Kiem duyet role", "moderator", "moderator" });

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("83d9e276-25c4-4ef9-82fc-27a678bfc6ce"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "7a83d4ac-86ee-4492-9b23-4650f92a344d", "AQAAAAEAACcQAAAAEP9xZPveyihcAGLUKRKYdRHy/S/7729dbFCTQzuCNCZ2Smomd2XfZ1cE1oUAnz1hgg==" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 10, 8, 16, 50, 51, 111, DateTimeKind.Local).AddTicks(1153));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateCreated",
                value: new DateTime(2021, 10, 8, 16, 50, 51, 114, DateTimeKind.Local).AddTicks(3467));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("c649e019-9887-4739-9404-a0ddd827690d"));

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("dece1100-4907-4145-8db9-d0e6e18f80e5"),
                column: "ConcurrencyStamp",
                value: "1d5e0d65-0d18-45f6-9e68-a9749e609df5");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("83d9e276-25c4-4ef9-82fc-27a678bfc6ce"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "af391c99-8f29-4d76-8601-2d7a0458b0c1", "AQAAAAEAACcQAAAAEGD8cRud7OgIZVbS3Xclb7fd6UMoq1Er1aDyxilT23oVF+pRqj8ja62RgLuibGcS2w==" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 3, 30, 14, 52, 40, 587, DateTimeKind.Local).AddTicks(5312));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateCreated",
                value: new DateTime(2021, 3, 30, 14, 52, 40, 588, DateTimeKind.Local).AddTicks(9407));
        }
    }
}
