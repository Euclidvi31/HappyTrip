using Microsoft.EntityFrameworkCore.Migrations;

namespace HappyTrip.Model.Migrations
{
    public partial class AddDayInformation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DayInformation",
                columns: table => new
                {
                    Date = table.Column<int>(nullable: false),
                    HolidayDetail = table.Column<int>(nullable: false),
                    IsHoliday = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DayInformation", x => x.Date);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DayInformation");
        }
    }
}
