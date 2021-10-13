using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eShopSolution.Data.Migrations
{
    public partial class AddProductImageTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateCreate",
                table: "Products");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "Products",
                type: "Date",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<string>(
                name: "ShipPhoneNumber",
                table: "Orders",
                type: "varchar(200)",
                unicode: false,
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(max)",
                oldUnicode: false,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ShipName",
                table: "Orders",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ShipAddress",
                table: "Orders",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "Products");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreate",
                table: "Products",
                type: "Date",
                nullable: false,
                defaultValueSql: "GetDate()");

            migrationBuilder.AlterColumn<string>(
                name: "ShipPhoneNumber",
                table: "Orders",
                type: "varchar(max)",
                unicode: false,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(200)",
                oldUnicode: false,
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "ShipName",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "ShipAddress",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("dece1100-4907-4145-8db9-d0e6e18f80e5"),
                column: "ConcurrencyStamp",
                value: "3a3ecf40-38dc-413d-9ef0-561e3c21150f");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("83d9e276-25c4-4ef9-82fc-27a678bfc6ce"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "44caa0d1-53bb-4115-8654-e3609c256f1c", "AQAAAAEAACcQAAAAEMqztp9D8hySFteAMV+jGJ79B0sHVI9zuY6PI/8Gy5lh+dm4L65Yw9MTZEdnHni+jg==" });

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
    }
}
