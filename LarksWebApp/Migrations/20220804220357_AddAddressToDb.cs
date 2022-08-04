using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LarksWebApp.Migrations
{
    public partial class AddAddressToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AddressModel_Countries_CountryID",
                table: "AddressModel");

            migrationBuilder.DropForeignKey(
                name: "FK_AddressModel_Municipalities_MunicipalityID",
                table: "AddressModel");

            migrationBuilder.DropForeignKey(
                name: "FK_AddressModel_Neighbourhoods_NeighbourhoodId",
                table: "AddressModel");

            migrationBuilder.DropForeignKey(
                name: "FK_AddressModel_Provinces_ProvinceID",
                table: "AddressModel");

            migrationBuilder.DropForeignKey(
                name: "FK_AddressModel_Streets_StreetID",
                table: "AddressModel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AddressModel",
                table: "AddressModel");

            migrationBuilder.RenameTable(
                name: "AddressModel",
                newName: "Addresses");

            migrationBuilder.RenameIndex(
                name: "IX_AddressModel_StreetID",
                table: "Addresses",
                newName: "IX_Addresses_StreetID");

            migrationBuilder.RenameIndex(
                name: "IX_AddressModel_ProvinceID",
                table: "Addresses",
                newName: "IX_Addresses_ProvinceID");

            migrationBuilder.RenameIndex(
                name: "IX_AddressModel_NeighbourhoodId",
                table: "Addresses",
                newName: "IX_Addresses_NeighbourhoodId");

            migrationBuilder.RenameIndex(
                name: "IX_AddressModel_MunicipalityID",
                table: "Addresses",
                newName: "IX_Addresses_MunicipalityID");

            migrationBuilder.RenameIndex(
                name: "IX_AddressModel_CountryID",
                table: "Addresses",
                newName: "IX_Addresses_CountryID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Addresses",
                table: "Addresses",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_Countries_CountryID",
                table: "Addresses",
                column: "CountryID",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_Municipalities_MunicipalityID",
                table: "Addresses",
                column: "MunicipalityID",
                principalTable: "Municipalities",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_Neighbourhoods_NeighbourhoodId",
                table: "Addresses",
                column: "NeighbourhoodId",
                principalTable: "Neighbourhoods",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_Provinces_ProvinceID",
                table: "Addresses",
                column: "ProvinceID",
                principalTable: "Provinces",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_Streets_StreetID",
                table: "Addresses",
                column: "StreetID",
                principalTable: "Streets",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_Countries_CountryID",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_Municipalities_MunicipalityID",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_Neighbourhoods_NeighbourhoodId",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_Provinces_ProvinceID",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_Streets_StreetID",
                table: "Addresses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Addresses",
                table: "Addresses");

            migrationBuilder.RenameTable(
                name: "Addresses",
                newName: "AddressModel");

            migrationBuilder.RenameIndex(
                name: "IX_Addresses_StreetID",
                table: "AddressModel",
                newName: "IX_AddressModel_StreetID");

            migrationBuilder.RenameIndex(
                name: "IX_Addresses_ProvinceID",
                table: "AddressModel",
                newName: "IX_AddressModel_ProvinceID");

            migrationBuilder.RenameIndex(
                name: "IX_Addresses_NeighbourhoodId",
                table: "AddressModel",
                newName: "IX_AddressModel_NeighbourhoodId");

            migrationBuilder.RenameIndex(
                name: "IX_Addresses_MunicipalityID",
                table: "AddressModel",
                newName: "IX_AddressModel_MunicipalityID");

            migrationBuilder.RenameIndex(
                name: "IX_Addresses_CountryID",
                table: "AddressModel",
                newName: "IX_AddressModel_CountryID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AddressModel",
                table: "AddressModel",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AddressModel_Countries_CountryID",
                table: "AddressModel",
                column: "CountryID",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AddressModel_Municipalities_MunicipalityID",
                table: "AddressModel",
                column: "MunicipalityID",
                principalTable: "Municipalities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AddressModel_Neighbourhoods_NeighbourhoodId",
                table: "AddressModel",
                column: "NeighbourhoodId",
                principalTable: "Neighbourhoods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AddressModel_Provinces_ProvinceID",
                table: "AddressModel",
                column: "ProvinceID",
                principalTable: "Provinces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AddressModel_Streets_StreetID",
                table: "AddressModel",
                column: "StreetID",
                principalTable: "Streets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
