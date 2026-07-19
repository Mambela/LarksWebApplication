using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LarksWebApp.Migrations
{
    /// <inheritdoc />
    public partial class MakeSuburbMunicipalityIdRequired : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Suburb_Municipalities_MunicipalityId",
                table: "Suburb");

            migrationBuilder.AlterColumn<Guid>(
                name: "MunicipalityId",
                table: "Suburb",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Suburb_Municipalities_MunicipalityId",
                table: "Suburb",
                column: "MunicipalityId",
                principalTable: "Municipalities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Suburb_Municipalities_MunicipalityId",
                table: "Suburb");

            migrationBuilder.AlterColumn<Guid>(
                name: "MunicipalityId",
                table: "Suburb",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_Suburb_Municipalities_MunicipalityId",
                table: "Suburb",
                column: "MunicipalityId",
                principalTable: "Municipalities",
                principalColumn: "Id");
        }
    }
}
