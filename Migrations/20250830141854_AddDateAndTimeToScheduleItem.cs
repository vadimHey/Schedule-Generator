using System;
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
            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "ScheduleItems",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<DateOnly>(
                name: "Date",
                table: "ScheduleItems",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<TimeOnly>(
                name: "EndTime",
                table: "ScheduleItems",
                type: "time without time zone",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));

            migrationBuilder.AddColumn<TimeOnly>(
                name: "StartTime",
                table: "ScheduleItems",
                type: "time without time zone",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "ScheduleItems");

            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "ScheduleItems");

            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "ScheduleItems");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "ScheduleItems",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
