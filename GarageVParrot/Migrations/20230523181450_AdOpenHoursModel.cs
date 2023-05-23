using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GarageVParrot.Migrations
{
    public partial class AdOpenHoursModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "openHours",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MondayOpenHours = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TuesdayOpenHours = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WednesdayOpenHours = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ThursdayOpenHours = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FridayOpenHours = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SaturdayOpenHours = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SundayOpenHours = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_openHours", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "openHours");
        }
    }
}
