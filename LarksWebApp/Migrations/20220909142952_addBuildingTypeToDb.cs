using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LarksWebApp.Migrations
{
    public partial class addBuildingTypeToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BuildingTypeId",
                table: "Addresses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "BuildingType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Designation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Lang = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuildingType", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_BuildingTypeId",
                table: "Addresses",
                column: "BuildingTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_BuildingType_BuildingTypeId",
                table: "Addresses",
                column: "BuildingTypeId",
                principalTable: "BuildingType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_BuildingType_BuildingTypeId",
                table: "Addresses");

            migrationBuilder.DropTable(
                name: "BuildingType");

            migrationBuilder.DropIndex(
                name: "IX_Addresses_BuildingTypeId",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "BuildingTypeId",
                table: "Addresses");
        }
    }
}
