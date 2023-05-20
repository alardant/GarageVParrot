using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GarageVParrot.Data.Migrations
{
    public partial class fixImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsFrontImage",
                table: "Images",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFrontImage",
                table: "Images");
        }
    }
}
