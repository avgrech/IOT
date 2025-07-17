using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeAuthomationAPI.Migrations
{
    /// <inheritdoc />
    public partial class FixDeviceIdRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeviceStatus_Device_DeviceId1",
                table: "DeviceStatus");

            migrationBuilder.DropIndex(
                name: "IX_DeviceStatus_DeviceId1",
                table: "DeviceStatus");

            migrationBuilder.DropColumn(
                name: "DeviceId1",
                table: "DeviceStatus");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DeviceId1",
                table: "DeviceStatus",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DeviceStatus_DeviceId1",
                table: "DeviceStatus",
                column: "DeviceId1");

            migrationBuilder.AddForeignKey(
                name: "FK_DeviceStatus_Device_DeviceId1",
                table: "DeviceStatus",
                column: "DeviceId1",
                principalTable: "Device",
                principalColumn: "Id");
        }

    }
}
