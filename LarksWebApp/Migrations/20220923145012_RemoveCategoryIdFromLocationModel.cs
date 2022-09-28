using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LarksWebApp.Migrations
{
    public partial class RemoveCategoryIdFromLocationModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CateogryId",
                table: "Location");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CateogryId",
                table: "Location",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
