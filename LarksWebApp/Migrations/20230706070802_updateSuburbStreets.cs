using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LarksWebApp.Migrations
{
    public partial class updateSuburbStreets : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SuburbStreets_Suburb_NeighbourhoodId",
                table: "SuburbStreets");

            migrationBuilder.DropIndex(
                name: "IX_SuburbStreets_NeighbourhoodId",
                table: "SuburbStreets");

            migrationBuilder.DropColumn(
                name: "NeighbourhoodId",
                table: "SuburbStreets");

            migrationBuilder.CreateIndex(
                name: "IX_SuburbStreets_SuburbId",
                table: "SuburbStreets",
                column: "SuburbId");

            migrationBuilder.AddForeignKey(
                name: "FK_SuburbStreets_Suburb_SuburbId",
                table: "SuburbStreets",
                column: "SuburbId",
                principalTable: "Suburb",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SuburbStreets_Suburb_SuburbId",
                table: "SuburbStreets");

            migrationBuilder.DropIndex(
                name: "IX_SuburbStreets_SuburbId",
                table: "SuburbStreets");

            migrationBuilder.AddColumn<int>(
                name: "NeighbourhoodId",
                table: "SuburbStreets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SuburbStreets_NeighbourhoodId",
                table: "SuburbStreets",
                column: "NeighbourhoodId");

            migrationBuilder.AddForeignKey(
                name: "FK_SuburbStreets_Suburb_NeighbourhoodId",
                table: "SuburbStreets",
                column: "NeighbourhoodId",
                principalTable: "Suburb",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
