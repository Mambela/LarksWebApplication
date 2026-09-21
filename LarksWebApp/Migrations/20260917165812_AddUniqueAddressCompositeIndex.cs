using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LarksWebApp.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueAddressCompositeIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Addresses_CountryID",
                table: "Addresses");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_CountryID_ProvinceID_MunicipalityID_SuburbId_Stre~",
                table: "Addresses",
                columns: new[] { "CountryID", "ProvinceID", "MunicipalityID", "SuburbId", "StreetID", "StreetNumber" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Addresses_CountryID_ProvinceID_MunicipalityID_SuburbId_Stre~",
                table: "Addresses");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_CountryID",
                table: "Addresses",
                column: "CountryID");
        }
    }
}
