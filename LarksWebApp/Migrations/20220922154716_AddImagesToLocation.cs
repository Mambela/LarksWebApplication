using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LarksWebApp.Migrations
{
    public partial class AddImagesToLocation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LocationImage_Location_LocationModelId",
                table: "LocationImage");

            migrationBuilder.DropIndex(
                name: "IX_LocationImage_LocationModelId",
                table: "LocationImage");

            migrationBuilder.DropColumn(
                name: "LocationModelId",
                table: "LocationImage");

            migrationBuilder.AddColumn<int>(
                name: "LocationId",
                table: "LocationImage",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_LocationImage_LocationId",
                table: "LocationImage",
                column: "LocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_LocationImage_Location_LocationId",
                table: "LocationImage",
                column: "LocationId",
                principalTable: "Location",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LocationImage_Location_LocationId",
                table: "LocationImage");

            migrationBuilder.DropIndex(
                name: "IX_LocationImage_LocationId",
                table: "LocationImage");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "LocationImage");

            migrationBuilder.AddColumn<int>(
                name: "LocationModelId",
                table: "LocationImage",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LocationImage_LocationModelId",
                table: "LocationImage",
                column: "LocationModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_LocationImage_Location_LocationModelId",
                table: "LocationImage",
                column: "LocationModelId",
                principalTable: "Location",
                principalColumn: "Id");
        }
    }
}
