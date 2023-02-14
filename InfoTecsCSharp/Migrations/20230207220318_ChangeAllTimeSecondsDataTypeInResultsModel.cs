using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InfoTecsCSharp.Migrations
{
    /// <inheritdoc />
    public partial class ChangeAllTimeSecondsDataTypeInResultsModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AllTime",
                table: "Results");

            migrationBuilder.AddColumn<int>(
                name: "AllTimeSeconds",
                table: "Results",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AllTimeSeconds",
                table: "Results");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "AllTime",
                table: "Results",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }
    }
}
