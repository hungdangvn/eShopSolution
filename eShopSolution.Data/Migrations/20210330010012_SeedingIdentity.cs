using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eShopSolution.Data.Migrations
{
    public partial class SeedingIdentity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AppRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Description", "Name", "NormalizedName" },
                values: new object[] { new Guid("dece1100-4907-4145-8db9-d0e6e18f80e5"), "3a3ecf40-38dc-413d-9ef0-561e3c21150f", "Administrator role", "admin", "admin" });

            migrationBuilder.InsertData(
                table: "AppUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("dece1100-4907-4145-8db9-d0e6e18f80e5"), new Guid("83d9e276-25c4-4ef9-82fc-27a678bfc6ce") });

            migrationBuilder.InsertData(
                table: "AppUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Dob", "Email", "EmailConfirmed", "FirtName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("83d9e276-25c4-4ef9-82fc-27a678bfc6ce"), 0, "44caa0d1-53bb-4115-8654-e3609c256f1c", new DateTime(1976, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "dangmanh.hung@msn.com", true, "Hung", "Dang", false, null, "dangmanh.hung@msn.com", "admin", "AQAAAAEAACcQAAAAEMqztp9D8hySFteAMV+jGJ79B0sHVI9zuY6PI/8Gy5lh+dm4L65Yw9MTZEdnHni+jg==", null, false, "", false, "admin" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreate",
                value: new DateTime(2021, 3, 30, 8, 0, 11, 620, DateTimeKind.Local).AddTicks(2643));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateCreate",
                value: new DateTime(2021, 3, 30, 8, 0, 11, 621, DateTimeKind.Local).AddTicks(8454));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("dece1100-4907-4145-8db9-d0e6e18f80e5"));

            migrationBuilder.DeleteData(
                table: "AppUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("dece1100-4907-4145-8db9-d0e6e18f80e5"), new Guid("83d9e276-25c4-4ef9-82fc-27a678bfc6ce") });

            migrationBuilder.DeleteData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("83d9e276-25c4-4ef9-82fc-27a678bfc6ce"));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreate",
                value: new DateTime(2021, 3, 30, 7, 35, 52, 34, DateTimeKind.Local).AddTicks(6850));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateCreate",
                value: new DateTime(2021, 3, 30, 7, 35, 52, 36, DateTimeKind.Local).AddTicks(742));
        }
    }
}
