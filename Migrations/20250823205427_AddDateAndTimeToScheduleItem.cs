using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ScheduleGenerator.Migrations
{
    /// <inheritdoc />
    public partial class AddDateAndTimeToScheduleItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Data",
                table: "ScheduleItems",
                newName: "Date");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Date",
                table: "ScheduleItems",
                newName: "Data");
        }
    }
}
