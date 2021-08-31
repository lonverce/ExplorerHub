using Microsoft.EntityFrameworkCore.Migrations;

namespace ExplorerHub.EfCore.Migrations
{
    public partial class SetFavoriteLocationUnique : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Favorites_Name",
                table: "Favorites");

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_Location",
                table: "Favorites",
                column: "Location",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Favorites_Location",
                table: "Favorites");

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_Name",
                table: "Favorites",
                column: "Name",
                unique: true);
        }
    }
}
