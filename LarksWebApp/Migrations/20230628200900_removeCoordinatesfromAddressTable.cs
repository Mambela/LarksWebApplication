using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LarksWebApp.Migrations
{
    public partial class removeCoordinatesfromAddressTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "lat",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "lng",
                table: "Addresses");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Addresses",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "Addresses",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "lat",
                table: "Addresses",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "lng",
                table: "Addresses",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
