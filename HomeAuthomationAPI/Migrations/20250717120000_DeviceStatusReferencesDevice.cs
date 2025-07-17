using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeAuthomationAPI.Migrations
{
    /// <inheritdoc />
    public partial class DeviceStatusReferencesDevice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RouterDeviceId",
                table: "DeviceStatuses");

            migrationBuilder.AddColumn<int>(
                name: "DeviceId",
                table: "DeviceStatuses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_DeviceStatuses_DeviceId",
                table: "DeviceStatuses",
                column: "DeviceId");

            migrationBuilder.AddForeignKey(
                name: "FK_DeviceStatuses_Devices_DeviceId",
                table: "DeviceStatuses",
                column: "DeviceId",
                principalTable: "Devices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeviceStatuses_Devices_DeviceId",
                table: "DeviceStatuses");

            migrationBuilder.DropIndex(
                name: "IX_DeviceStatuses_DeviceId",
                table: "DeviceStatuses");

            migrationBuilder.DropColumn(
                name: "DeviceId",
                table: "DeviceStatuses");

            migrationBuilder.AddColumn<string>(
                name: "RouterDeviceId",
                table: "DeviceStatuses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
