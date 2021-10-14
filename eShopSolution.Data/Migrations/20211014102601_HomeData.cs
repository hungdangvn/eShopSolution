using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eShopSolution.Data.Migrations
{
    public partial class HomeData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsFeatured",
                table: "Products",
                type: "bit",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("3d474cd6-912b-4df7-bbd9-da31fa3061c6"),
                column: "ConcurrencyStamp",
                value: "9e891efb-4ac9-43f1-9733-b9d57eb38269");

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("c649e019-9887-4739-9404-a0ddd827690d"),
                column: "ConcurrencyStamp",
                value: "55855d7a-00df-413d-a486-72a902544439");

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("dece1100-4907-4145-8db9-d0e6e18f80e5"),
                column: "ConcurrencyStamp",
                value: "cfa60592-9719-46a4-874c-ad3fd5feb161");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("83d9e276-25c4-4ef9-82fc-27a678bfc6ce"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "11c227b7-7605-4321-8023-c45b8688a165", "AQAAAAEAACcQAAAAEBJQF9d4H3YcLKIbf/SnGXQD7kMSiQn8vuQSuXPeRSrFvwsGzJ9doROOXQTKIPKQvw==" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 10, 14, 17, 26, 0, 415, DateTimeKind.Local).AddTicks(9877));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateCreated",
                value: new DateTime(2021, 10, 14, 17, 26, 0, 418, DateTimeKind.Local).AddTicks(7956));

            migrationBuilder.InsertData(
                table: "Slides",
                columns: new[] { "Id", "Description", "Image", "Name", "SortOrder", "Status", "Url" },
                values: new object[,]
                {
                    { 1, "Cras justo odio, dapibus ac facilisis in, egestas eget quam. Donec id elit non mi porta gravida at eget metus. Nullam id dolor id nibh ultricies vehicula ut id elit.", "/themes/images/carousel/1.png", "Second Thumbnail label", 1, 1, "#" },
                    { 2, "Cras justo odio, dapibus ac facilisis in, egestas eget quam. Donec id elit non mi porta gravida at eget metus. Nullam id dolor id nibh ultricies vehicula ut id elit.", "/themes/images/carousel/2.png", "Second Thumbnail label", 2, 1, "#" },
                    { 3, "Cras justo odio, dapibus ac facilisis in, egestas eget quam. Donec id elit non mi porta gravida at eget metus. Nullam id dolor id nibh ultricies vehicula ut id elit.", "/themes/images/carousel/3.png", "Second Thumbnail label", 3, 1, "#" },
                    { 4, "Cras justo odio, dapibus ac facilisis in, egestas eget quam. Donec id elit non mi porta gravida at eget metus. Nullam id dolor id nibh ultricies vehicula ut id elit.", "/themes/images/carousel/4.png", "Second Thumbnail label", 4, 1, "#" },
                    { 5, "Cras justo odio, dapibus ac facilisis in, egestas eget quam. Donec id elit non mi porta gravida at eget metus. Nullam id dolor id nibh ultricies vehicula ut id elit.", "/themes/images/carousel/5.png", "Second Thumbnail label", 5, 1, "#" },
                    { 6, "Cras justo odio, dapibus ac facilisis in, egestas eget quam. Donec id elit non mi porta gravida at eget metus. Nullam id dolor id nibh ultricies vehicula ut id elit.", "/themes/images/carousel/6.png", "Second Thumbnail label", 1, 1, "#" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Slides",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Slides",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Slides",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Slides",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Slides",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Slides",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DropColumn(
                name: "IsFeatured",
                table: "Products");

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("3d474cd6-912b-4df7-bbd9-da31fa3061c6"),
                column: "ConcurrencyStamp",
                value: "f578d420-3da9-4856-90b8-aa009ef0321d");

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("c649e019-9887-4739-9404-a0ddd827690d"),
                column: "ConcurrencyStamp",
                value: "eace8914-187c-4838-832f-d42a110c7bb5");

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("dece1100-4907-4145-8db9-d0e6e18f80e5"),
                column: "ConcurrencyStamp",
                value: "5524993e-638b-4b7a-8179-b9ea2a43441a");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("83d9e276-25c4-4ef9-82fc-27a678bfc6ce"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "c62eb2ba-7323-4406-a7f7-461ad60f6061", "AQAAAAEAACcQAAAAEFDdcae2P+Gb3+2s4AvCBDG8Y8IaePLbBaG1eOCld/mxYcsqwD87UuTSwrfgZLeyFQ==" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 10, 14, 16, 0, 35, 429, DateTimeKind.Local).AddTicks(8227));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateCreated",
                value: new DateTime(2021, 10, 14, 16, 0, 35, 432, DateTimeKind.Local).AddTicks(4444));
        }
    }
}
