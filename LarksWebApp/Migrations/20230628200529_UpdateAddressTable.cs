using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LarksWebApp.Migrations
{
    public partial class UpdateAddressTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_BuildingType_BuildingTypeId",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_Municipalities_MunicipalityID",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_Provinces_ProvinceID",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_Streets_StreetID",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_Suburb_NeighbourhoodId",
                table: "Addresses");

            migrationBuilder.DropIndex(
                name: "IX_Addresses_NeighbourhoodId",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "NeighbourhoodId",
                table: "Addresses");

            migrationBuilder.AlterColumn<string>(
                name: "StreetNumber",
                table: "Addresses",
                type: "varchar(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(10)",
                oldMaxLength: 10)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "StreetID",
                table: "Addresses",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ProvinceID",
                table: "Addresses",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "MunicipalityID",
                table: "Addresses",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "BuildingTypeId",
                table: "Addresses",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "SuburbId",
                table: "Addresses",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_SuburbId",
                table: "Addresses",
                column: "SuburbId");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_BuildingType_BuildingTypeId",
                table: "Addresses",
                column: "BuildingTypeId",
                principalTable: "BuildingType",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_Municipalities_MunicipalityID",
                table: "Addresses",
                column: "MunicipalityID",
                principalTable: "Municipalities",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_Provinces_ProvinceID",
                table: "Addresses",
                column: "ProvinceID",
                principalTable: "Provinces",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_Streets_StreetID",
                table: "Addresses",
                column: "StreetID",
                principalTable: "Streets",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_Suburb_SuburbId",
                table: "Addresses",
                column: "SuburbId",
                principalTable: "Suburb",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_BuildingType_BuildingTypeId",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_Municipalities_MunicipalityID",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_Provinces_ProvinceID",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_Streets_StreetID",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_Suburb_SuburbId",
                table: "Addresses");

            migrationBuilder.DropIndex(
                name: "IX_Addresses_SuburbId",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "SuburbId",
                table: "Addresses");

            migrationBuilder.UpdateData(
                table: "Addresses",
                keyColumn: "StreetNumber",
                keyValue: null,
                column: "StreetNumber",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "StreetNumber",
                table: "Addresses",
                type: "varchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(10)",
                oldMaxLength: 10,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "StreetID",
                table: "Addresses",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ProvinceID",
                table: "Addresses",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MunicipalityID",
                table: "Addresses",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BuildingTypeId",
                table: "Addresses",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NeighbourhoodId",
                table: "Addresses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_NeighbourhoodId",
                table: "Addresses",
                column: "NeighbourhoodId");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_BuildingType_BuildingTypeId",
                table: "Addresses",
                column: "BuildingTypeId",
                principalTable: "BuildingType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_Municipalities_MunicipalityID",
                table: "Addresses",
                column: "MunicipalityID",
                principalTable: "Municipalities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_Provinces_ProvinceID",
                table: "Addresses",
                column: "ProvinceID",
                principalTable: "Provinces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_Streets_StreetID",
                table: "Addresses",
                column: "StreetID",
                principalTable: "Streets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_Suburb_NeighbourhoodId",
                table: "Addresses",
                column: "NeighbourhoodId",
                principalTable: "Suburb",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
