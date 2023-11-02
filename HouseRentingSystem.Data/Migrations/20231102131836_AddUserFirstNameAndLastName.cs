using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HouseRentingSystem.Data.Migrations
{
    public partial class AddUserFirstNameAndLastName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Houses",
                keyColumn: "Id",
                keyValue: new Guid("387e561e-f2f1-48bf-921e-6d8154cec797"));

            migrationBuilder.DeleteData(
                table: "Houses",
                keyColumn: "Id",
                keyValue: new Guid("b5d70824-2803-4775-9818-921d4d822602"));

            migrationBuilder.DeleteData(
                table: "Houses",
                keyColumn: "Id",
                keyValue: new Guid("e2c771f4-f458-4b13-933a-48822a336065"));

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(12)",
                maxLength: 12,
                nullable: false,
                defaultValue: "Test");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: false,
                defaultValue: "Testov");

            migrationBuilder.InsertData(
                table: "Houses",
                columns: new[] { "Id", "Address", "AgentId", "CategoryId", "Description", "ImageUrl", "PricePerMonth", "RenterId", "Title" },
                values: new object[] { new Guid("12d78b44-6267-4b12-ba15-f730719a5fde"), "North London, UK (near the border)", new Guid("c23c8dd9-c699-4334-9ee9-d277018fcbf4"), 3, "A big house for your whole family. Don't miss to buy a house with three bedrooms.", "https://www.luxury-architecture.net/wp-content/uploads/2017/12/1513217889-7597-FAIRWAYS-010.jpg", 2100.00m, new Guid("869d277a-b45b-4617-bddb-08dbb495efac"), "Big House Marina" });

            migrationBuilder.InsertData(
                table: "Houses",
                columns: new[] { "Id", "Address", "AgentId", "CategoryId", "Description", "ImageUrl", "PricePerMonth", "RenterId", "Title" },
                values: new object[] { new Guid("376e9540-62f0-49a2-ac19-234abae223b2"), "Near the Sea Garden in Burgas, Bulgaria", new Guid("c23c8dd9-c699-4334-9ee9-d277018fcbf4"), 2, "It has the best comfort you will ever ask for. With two bedrooms,it is great for your family.", "https://cf.bstatic.com/xdata/images/hotel/max1024x768/179489660.jpg?k=2029f6d9589b49c95dcc9503a265e292c2cdfcb5277487a0050397c3f8dd545a&o=&hp=1", 1200.00m, null, "Family House Comfort" });

            migrationBuilder.InsertData(
                table: "Houses",
                columns: new[] { "Id", "Address", "AgentId", "CategoryId", "Description", "ImageUrl", "PricePerMonth", "RenterId", "Title" },
                values: new object[] { new Guid("6764dbee-12cf-4caf-806c-dcf63e6b6f69"), "Boyana Neighbourhood, Sofia, Bulgaria", new Guid("c23c8dd9-c699-4334-9ee9-d277018fcbf4"), 2, "This luxurious house is everything you will need. It is just excellent.", "https://i.pinimg.com/originals/a6/f5/85/a6f5850a77633c56e4e4ac4f867e3c00.jpg", 2000.00m, null, "Grand House" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Houses",
                keyColumn: "Id",
                keyValue: new Guid("12d78b44-6267-4b12-ba15-f730719a5fde"));

            migrationBuilder.DeleteData(
                table: "Houses",
                keyColumn: "Id",
                keyValue: new Guid("376e9540-62f0-49a2-ac19-234abae223b2"));

            migrationBuilder.DeleteData(
                table: "Houses",
                keyColumn: "Id",
                keyValue: new Guid("6764dbee-12cf-4caf-806c-dcf63e6b6f69"));

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");

            migrationBuilder.InsertData(
                table: "Houses",
                columns: new[] { "Id", "Address", "AgentId", "CategoryId", "CreatedOn", "Description", "ImageUrl", "IsActive", "PricePerMonth", "RenterId", "Title" },
                values: new object[] { new Guid("387e561e-f2f1-48bf-921e-6d8154cec797"), "Boyana Neighbourhood, Sofia, Bulgaria", new Guid("c23c8dd9-c699-4334-9ee9-d277018fcbf4"), 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "This luxurious house is everything you will need. It is just excellent.", "https://i.pinimg.com/originals/a6/f5/85/a6f5850a77633c56e4e4ac4f867e3c00.jpg", false, 2000.00m, null, "Grand House" });

            migrationBuilder.InsertData(
                table: "Houses",
                columns: new[] { "Id", "Address", "AgentId", "CategoryId", "CreatedOn", "Description", "ImageUrl", "IsActive", "PricePerMonth", "RenterId", "Title" },
                values: new object[] { new Guid("b5d70824-2803-4775-9818-921d4d822602"), "Near the Sea Garden in Burgas, Bulgaria", new Guid("c23c8dd9-c699-4334-9ee9-d277018fcbf4"), 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "It has the best comfort you will ever ask for. With two bedrooms,it is great for your family.", "https://cf.bstatic.com/xdata/images/hotel/max1024x768/179489660.jpg?k=2029f6d9589b49c95dcc9503a265e292c2cdfcb5277487a0050397c3f8dd545a&o=&hp=1", false, 1200.00m, null, "Family House Comfort" });

            migrationBuilder.InsertData(
                table: "Houses",
                columns: new[] { "Id", "Address", "AgentId", "CategoryId", "CreatedOn", "Description", "ImageUrl", "IsActive", "PricePerMonth", "RenterId", "Title" },
                values: new object[] { new Guid("e2c771f4-f458-4b13-933a-48822a336065"), "North London, UK (near the border)", new Guid("c23c8dd9-c699-4334-9ee9-d277018fcbf4"), 3, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "A big house for your whole family. Don't miss to buy a house with three bedrooms.", "https://www.luxury-architecture.net/wp-content/uploads/2017/12/1513217889-7597-FAIRWAYS-010.jpg", false, 2100.00m, new Guid("869d277a-b45b-4617-bddb-08dbb495efac"), "Big House Marina" });
        }
    }
}
