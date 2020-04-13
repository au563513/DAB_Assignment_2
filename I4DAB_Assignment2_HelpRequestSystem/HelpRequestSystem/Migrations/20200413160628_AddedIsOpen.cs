using Microsoft.EntityFrameworkCore.Migrations;

namespace HelpRequestSystem.Migrations
{
    public partial class AddedIsOpen : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsOpen",
                table: "Exercises",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsOpen",
                table: "Assignments",
                nullable: false,
                defaultValue: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsOpen",
                table: "Exercises");

            migrationBuilder.DropColumn(
                name: "IsOpen",
                table: "Assignments");
        }
    }
}
