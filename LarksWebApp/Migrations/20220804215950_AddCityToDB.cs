using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LarksWebApp.Migrations
{
    public partial class AddCityToDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CityModel_Provinces_ProvinceId",
                table: "CityModel");

            migrationBuilder.DropForeignKey(
                name: "FK_Neighbourhoods_CityModel_CityId",
                table: "Neighbourhoods");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CityModel",
                table: "CityModel");

            migrationBuilder.RenameTable(
                name: "CityModel",
                newName: "Cities");

            migrationBuilder.RenameIndex(
                name: "IX_CityModel_ProvinceId",
                table: "Cities",
                newName: "IX_Cities_ProvinceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Cities",
                table: "Cities",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Cities_Provinces_ProvinceId",
                table: "Cities",
                column: "ProvinceId",
                principalTable: "Provinces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Neighbourhoods_Cities_CityId",
                table: "Neighbourhoods",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cities_Provinces_ProvinceId",
                table: "Cities");

            migrationBuilder.DropForeignKey(
                name: "FK_Neighbourhoods_Cities_CityId",
                table: "Neighbourhoods");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Cities",
                table: "Cities");

            migrationBuilder.RenameTable(
                name: "Cities",
                newName: "CityModel");

            migrationBuilder.RenameIndex(
                name: "IX_Cities_ProvinceId",
                table: "CityModel",
                newName: "IX_CityModel_ProvinceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CityModel",
                table: "CityModel",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CityModel_Provinces_ProvinceId",
                table: "CityModel",
                column: "ProvinceId",
                principalTable: "Provinces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Neighbourhoods_CityModel_CityId",
                table: "Neighbourhoods",
                column: "CityId",
                principalTable: "CityModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
