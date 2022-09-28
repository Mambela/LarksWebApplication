using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LarksWebApp.Migrations
{
    public partial class AddRelationshipsToLocation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LocationModelId",
                table: "LocationImage",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AddressId",
                table: "Location",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Location",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CateogryId",
                table: "Location",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SubcategoryId",
                table: "Location",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_LocationImage_LocationModelId",
                table: "LocationImage",
                column: "LocationModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Location_AddressId",
                table: "Location",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Location_CategoryId",
                table: "Location",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Location_SubcategoryId",
                table: "Location",
                column: "SubcategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Location_Addresses_AddressId",
                table: "Location",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Location_Categories_CategoryId",
                table: "Location",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Location_Subcategories_SubcategoryId",
                table: "Location",
                column: "SubcategoryId",
                principalTable: "Subcategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_LocationImage_Location_LocationModelId",
                table: "LocationImage",
                column: "LocationModelId",
                principalTable: "Location",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Location_Addresses_AddressId",
                table: "Location");

            migrationBuilder.DropForeignKey(
                name: "FK_Location_Categories_CategoryId",
                table: "Location");

            migrationBuilder.DropForeignKey(
                name: "FK_Location_Subcategories_SubcategoryId",
                table: "Location");

            migrationBuilder.DropForeignKey(
                name: "FK_LocationImage_Location_LocationModelId",
                table: "LocationImage");

            migrationBuilder.DropIndex(
                name: "IX_LocationImage_LocationModelId",
                table: "LocationImage");

            migrationBuilder.DropIndex(
                name: "IX_Location_AddressId",
                table: "Location");

            migrationBuilder.DropIndex(
                name: "IX_Location_CategoryId",
                table: "Location");

            migrationBuilder.DropIndex(
                name: "IX_Location_SubcategoryId",
                table: "Location");

            migrationBuilder.DropColumn(
                name: "LocationModelId",
                table: "LocationImage");

            migrationBuilder.DropColumn(
                name: "AddressId",
                table: "Location");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Location");

            migrationBuilder.DropColumn(
                name: "CateogryId",
                table: "Location");

            migrationBuilder.DropColumn(
                name: "SubcategoryId",
                table: "Location");
        }
    }
}
