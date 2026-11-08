using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace onine.Repository.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedDeliveryMethods : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "DeliveryMethods",
                columns: new[] { "Id", "DeliveryTime", "Description", "Price", "ShortName" },
                values: new object[,]
                {
                    { 1, "1-2 days", "UPS Delivery", 10.00m, "UPS" },
                    { 2, "2-3 days", "FedEx Delivery", 15.00m, "FedEx" },
                    { 3, "3-5 days", "DHL Delivery", 20.00m, "DHL" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "DeliveryMethods",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "DeliveryMethods",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "DeliveryMethods",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
