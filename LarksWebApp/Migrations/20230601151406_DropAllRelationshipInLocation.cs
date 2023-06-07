using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LarksWebApp.Migrations
{
    public partial class DropAllRelationshipInLocation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropIndex(
                name: "IX_Location_SubcategoryId",
                table: "Location");

            migrationBuilder.DropColumn(
                name: "SubcategoryId",
                table: "Location");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "Location",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "AddressId",
                table: "Location",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Location_Addresses_AddressId",
                table: "Location",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Location_Categories_CategoryId",
                table: "Location",
                column: "CategoryId",
                principalTable: "Categories",
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

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "Location",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AddressId",
                table: "Location",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SubcategoryId",
                table: "Location",
                type: "int",
                nullable: false,
                defaultValue: 0);

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
                onDelete: ReferentialAction.Cascade);
        }
    }
}
