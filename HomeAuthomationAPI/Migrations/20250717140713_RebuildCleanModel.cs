using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeAuthomationAPI.Migrations
{
    /// <inheritdoc />
    public partial class RebuildCleanModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeviceStatuses_Devices_DeviceId1",
                table: "DeviceStatuses");

            migrationBuilder.DropIndex(
                name: "IX_DeviceStatuses_DeviceId1",
                table: "DeviceStatuses");

            migrationBuilder.DropColumn(
                name: "DeviceId1",
                table: "DeviceStatuses");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DeviceId1",
                table: "DeviceStatuses",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DeviceStatuses_DeviceId1",
                table: "DeviceStatuses",
                column: "DeviceId1");

            migrationBuilder.AddForeignKey(
                name: "FK_DeviceStatuses_Devices_DeviceId1",
                table: "DeviceStatuses",
                column: "DeviceId1",
                principalTable: "Devices",
                principalColumn: "Id");
        }
    }
}
