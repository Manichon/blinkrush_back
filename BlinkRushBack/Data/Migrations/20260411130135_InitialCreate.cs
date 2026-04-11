using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlinkRush.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LeaderboardRecords",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Mode = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    Value = table.Column<double>(type: "REAL", nullable: false),
                    OccurredAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaderboardRecords", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LeaderboardRecords_Mode",
                table: "LeaderboardRecords",
                column: "Mode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LeaderboardRecords");
        }
    }
}
