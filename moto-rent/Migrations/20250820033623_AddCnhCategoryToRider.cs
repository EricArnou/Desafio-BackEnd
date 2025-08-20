using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace moto_rent.Migrations
{
    /// <inheritdoc />
    public partial class AddCnhCategoryToRider : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CnhCategory",
                table: "Riders",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ImageCnh",
                table: "Riders",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CnhCategory",
                table: "Riders");

            migrationBuilder.DropColumn(
                name: "ImageCnh",
                table: "Riders");
        }
    }
}
