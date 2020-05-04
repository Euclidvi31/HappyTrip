using Microsoft.EntityFrameworkCore.Migrations;

namespace HappyTrip.Model.Migrations
{
    public partial class AddPoiComfort : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Comfort",
                table: "Poi",
                maxLength: 8,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comfort",
                table: "Poi");
        }
    }
}
