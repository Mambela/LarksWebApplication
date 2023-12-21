using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LarksWebApp.Migrations
{
    public partial class addAttributoIsReferenceToLocation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isReference",
                table: "Location",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isReference",
                table: "Location");
        }
    }
}
