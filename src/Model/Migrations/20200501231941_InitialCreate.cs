using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HappyTrip.Model.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Poi",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(maxLength: 10, nullable: true),
                    Name = table.Column<string>(maxLength: 50, nullable: true),
                    RefreshAt = table.Column<DateTime>(nullable: false),
                    TrafficNumber = table.Column<int>(nullable: false),
                    MaxTrafficNumber = table.Column<int>(nullable: false),
                    Status = table.Column<string>(maxLength: 10, nullable: true),
                    County = table.Column<string>(maxLength: 10, nullable: true),
                    Grade = table.Column<string>(maxLength: 8, nullable: true),
                    Initial = table.Column<string>(maxLength: 20, nullable: true),
                    StartTime = table.Column<string>(maxLength: 10, nullable: true),
                    EndTime = table.Column<string>(maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Poi", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PoiHistory",
                columns: table => new
                {
                    PoiId = table.Column<int>(nullable: false),
                    Date = table.Column<int>(nullable: false),
                    MinTraffic = table.Column<int>(nullable: false),
                    AvgTraffic = table.Column<int>(nullable: false),
                    MaxTraffic = table.Column<int>(nullable: false),
                    Status = table.Column<string>(maxLength: 10, nullable: true),
                    Whether = table.Column<string>(maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PoiHistory", x => new { x.PoiId, x.Date });
                });

            migrationBuilder.CreateIndex(
                name: "IX_Poi_Code",
                table: "Poi",
                column: "Code",
                unique: true,
                filter: "[Code] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_PoiHistory_Date",
                table: "PoiHistory",
                column: "Date");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Poi");

            migrationBuilder.DropTable(
                name: "PoiHistory");
        }
    }
}
