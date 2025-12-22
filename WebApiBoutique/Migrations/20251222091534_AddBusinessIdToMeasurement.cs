using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApiBoutique.Migrations
{
    /// <inheritdoc />
    public partial class AddBusinessIdToMeasurement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BusinessId",
                table: "Measurements",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BusinessId",
                table: "Measurements");
        }
    }
}
