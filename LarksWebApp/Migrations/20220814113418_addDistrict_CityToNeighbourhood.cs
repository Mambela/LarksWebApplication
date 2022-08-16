using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LarksWebApp.Migrations
{
    public partial class addDistrict_CityToNeighbourhood : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Neighbourhoods_Cities_CityId",
                table: "Neighbourhoods");

            migrationBuilder.AlterColumn<int>(
                name: "CityId",
                table: "Neighbourhoods",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "DistrictId",
                table: "Neighbourhoods",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Neighbourhoods_DistrictId",
                table: "Neighbourhoods",
                column: "DistrictId");

            migrationBuilder.AddForeignKey(
                name: "FK_Neighbourhoods_Cities_CityId",
                table: "Neighbourhoods",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Neighbourhoods_Districts_DistrictId",
                table: "Neighbourhoods",
                column: "DistrictId",
                principalTable: "Districts",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Neighbourhoods_Cities_CityId",
                table: "Neighbourhoods");

            migrationBuilder.DropForeignKey(
                name: "FK_Neighbourhoods_Districts_DistrictId",
                table: "Neighbourhoods");

            migrationBuilder.DropIndex(
                name: "IX_Neighbourhoods_DistrictId",
                table: "Neighbourhoods");

            migrationBuilder.DropColumn(
                name: "DistrictId",
                table: "Neighbourhoods");

            migrationBuilder.AlterColumn<int>(
                name: "CityId",
                table: "Neighbourhoods",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Neighbourhoods_Cities_CityId",
                table: "Neighbourhoods",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
