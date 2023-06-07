using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LarksWebApp.Migrations
{
    public partial class AddCityMunicipalitySuburbToLocation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CityId",
                table: "Location",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MunicipalityId",
                table: "Location",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SuburbId",
                table: "Location",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Location_CityId",
                table: "Location",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Location_MunicipalityId",
                table: "Location",
                column: "MunicipalityId");

            migrationBuilder.CreateIndex(
                name: "IX_Location_SuburbId",
                table: "Location",
                column: "SuburbId");

            migrationBuilder.AddForeignKey(
                name: "FK_Location_Cities_CityId",
                table: "Location",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Location_Municipalities_MunicipalityId",
                table: "Location",
                column: "MunicipalityId",
                principalTable: "Municipalities",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Location_Suburb_SuburbId",
                table: "Location",
                column: "SuburbId",
                principalTable: "Suburb",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Location_Cities_CityId",
                table: "Location");

            migrationBuilder.DropForeignKey(
                name: "FK_Location_Municipalities_MunicipalityId",
                table: "Location");

            migrationBuilder.DropForeignKey(
                name: "FK_Location_Suburb_SuburbId",
                table: "Location");

            migrationBuilder.DropIndex(
                name: "IX_Location_CityId",
                table: "Location");

            migrationBuilder.DropIndex(
                name: "IX_Location_MunicipalityId",
                table: "Location");

            migrationBuilder.DropIndex(
                name: "IX_Location_SuburbId",
                table: "Location");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "Location");

            migrationBuilder.DropColumn(
                name: "MunicipalityId",
                table: "Location");

            migrationBuilder.DropColumn(
                name: "SuburbId",
                table: "Location");
        }
    }
}
