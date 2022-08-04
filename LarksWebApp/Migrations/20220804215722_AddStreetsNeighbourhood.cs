using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LarksWebApp.Migrations
{
    public partial class AddStreetsNeighbourhood : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AddressModel_StreetModel_StreetID",
                table: "AddressModel");

            migrationBuilder.DropForeignKey(
                name: "FK_NeighbourhoodStreets_StreetModel_StreetsID",
                table: "NeighbourhoodStreets");

            migrationBuilder.DropForeignKey(
                name: "FK_StreetCoordinates_StreetModel_StreetId",
                table: "StreetCoordinates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StreetModel",
                table: "StreetModel");

            migrationBuilder.RenameTable(
                name: "StreetModel",
                newName: "Streets");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Streets",
                table: "Streets",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AddressModel_Streets_StreetID",
                table: "AddressModel",
                column: "StreetID",
                principalTable: "Streets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NeighbourhoodStreets_Streets_StreetsID",
                table: "NeighbourhoodStreets",
                column: "StreetsID",
                principalTable: "Streets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StreetCoordinates_Streets_StreetId",
                table: "StreetCoordinates",
                column: "StreetId",
                principalTable: "Streets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AddressModel_Streets_StreetID",
                table: "AddressModel");

            migrationBuilder.DropForeignKey(
                name: "FK_NeighbourhoodStreets_Streets_StreetsID",
                table: "NeighbourhoodStreets");

            migrationBuilder.DropForeignKey(
                name: "FK_StreetCoordinates_Streets_StreetId",
                table: "StreetCoordinates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Streets",
                table: "Streets");

            migrationBuilder.RenameTable(
                name: "Streets",
                newName: "StreetModel");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StreetModel",
                table: "StreetModel",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AddressModel_StreetModel_StreetID",
                table: "AddressModel",
                column: "StreetID",
                principalTable: "StreetModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NeighbourhoodStreets_StreetModel_StreetsID",
                table: "NeighbourhoodStreets",
                column: "StreetsID",
                principalTable: "StreetModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StreetCoordinates_StreetModel_StreetId",
                table: "StreetCoordinates",
                column: "StreetId",
                principalTable: "StreetModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
