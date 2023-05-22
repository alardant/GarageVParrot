using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GarageVParrot.Data.Migrations
{
    public partial class ModifyCar : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ImageCarUpload");

            migrationBuilder.AddColumn<string>(
                name: "CoverImage",
                table: "Cars",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "ImageListCar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImagePath = table.Column<bool>(type: "bit", nullable: false),
                    CarId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImageListCar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImageListCar_Cars_CarId",
                        column: x => x.CarId,
                        principalTable: "Cars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ImageListCar_CarId",
                table: "ImageListCar",
                column: "CarId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ImageListCar");

            migrationBuilder.DropColumn(
                name: "CoverImage",
                table: "Cars");

            migrationBuilder.CreateTable(
                name: "ImageCarUpload",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CarId = table.Column<int>(type: "int", nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsFrontImage = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImageCarUpload", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImageCarUpload_Cars_CarId",
                        column: x => x.CarId,
                        principalTable: "Cars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ImageCarUpload_CarId",
                table: "ImageCarUpload",
                column: "CarId");
        }
    }
}
