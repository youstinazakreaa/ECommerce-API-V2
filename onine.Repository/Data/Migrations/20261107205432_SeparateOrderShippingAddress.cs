using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace onine.Repository.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeparateOrderShippingAddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Address_ShipToAddressId",
                table: "Orders");

           

            migrationBuilder.DropIndex(
                name: "IX_Orders_ShipToAddressId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ShipToAddressId",
                table: "Orders");

            migrationBuilder.AddColumn<string>(
                name: "ShipToAddress_City",
                table: "Orders",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ShipToAddress_Country",
                table: "Orders",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ShipToAddress_FirstName",
                table: "Orders",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ShipToAddress_LastName",
                table: "Orders",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ShipToAddress_Street",
                table: "Orders",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShipToAddress_City",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ShipToAddress_Country",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ShipToAddress_FirstName",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ShipToAddress_LastName",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ShipToAddress_Street",
                table: "Orders");

            migrationBuilder.AddColumn<int>(
                name: "ShipToAddressId",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);


           

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ShipToAddressId",
                table: "Orders",
                column: "ShipToAddressId");

         

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Address_ShipToAddressId",
                table: "Orders",
                column: "ShipToAddressId",
                principalTable: "Address",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
