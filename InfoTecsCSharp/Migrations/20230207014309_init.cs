using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InfoTecsCSharp.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Files",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Files", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Results",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileDescriptionId = table.Column<int>(type: "int", nullable: false),
                    AllTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    FirstOperation = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AverageOperationTime = table.Column<float>(type: "real", nullable: false),
                    AverageValue = table.Column<float>(type: "real", nullable: false),
                    MedianValue = table.Column<float>(type: "real", nullable: false),
                    MaximumValue = table.Column<float>(type: "real", nullable: false),
                    MinimumValue = table.Column<float>(type: "real", nullable: false),
                    RowsCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Results", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Results_Files_FileDescriptionId",
                        column: x => x.FileDescriptionId,
                        principalTable: "Files",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TableEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileDescriptionId = table.Column<int>(type: "int", nullable: false),
                    OperationDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OperationSeconds = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TableEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TableEntries_Files_FileDescriptionId",
                        column: x => x.FileDescriptionId,
                        principalTable: "Files",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Results_FileDescriptionId",
                table: "Results",
                column: "FileDescriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_TableEntries_FileDescriptionId",
                table: "TableEntries",
                column: "FileDescriptionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Results");

            migrationBuilder.DropTable(
                name: "TableEntries");

            migrationBuilder.DropTable(
                name: "Files");
        }
    }
}
