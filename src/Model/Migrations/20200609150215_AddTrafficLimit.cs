using Microsoft.EntityFrameworkCore.Migrations;

namespace HappyTrip.Model.Migrations
{
    public partial class AddTrafficLimit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TrafficLimit",
                table: "PoiHistory",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TrafficLimit",
                table: "PoiHistory");
        }
    }
}
