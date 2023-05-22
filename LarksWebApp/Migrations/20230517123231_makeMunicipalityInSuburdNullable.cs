using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LarksWebApp.Migrations
{
    public partial class makeMunicipalityInSuburdNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Suburb_Municipalities_MunicipalityId",
                table: "Suburb");

            migrationBuilder.AlterColumn<int>(
                name: "MunicipalityId",
                table: "Suburb",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Suburb_Municipalities_MunicipalityId",
                table: "Suburb",
                column: "MunicipalityId",
                principalTable: "Municipalities",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Suburb_Municipalities_MunicipalityId",
                table: "Suburb");

            migrationBuilder.AlterColumn<int>(
                name: "MunicipalityId",
                table: "Suburb",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Suburb_Municipalities_MunicipalityId",
                table: "Suburb",
                column: "MunicipalityId",
                principalTable: "Municipalities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
