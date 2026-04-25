using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlinkRush.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDeviceIdAndName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM LeaderboardRecords;");

            migrationBuilder.AddColumn<string>(
                name: "DeviceId",
                table: "LeaderboardRecords",
                type: "TEXT",
                maxLength: 128,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "LeaderboardRecords",
                type: "TEXT",
                maxLength: 64,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LeaderboardRecords_Mode_DeviceId",
                table: "LeaderboardRecords",
                columns: new[] { "Mode", "DeviceId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_LeaderboardRecords_Mode_DeviceId",
                table: "LeaderboardRecords");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "LeaderboardRecords");

            migrationBuilder.DropColumn(
                name: "DeviceId",
                table: "LeaderboardRecords");
        }
    }
}
