using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeAuthomationAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialSync : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Parameters_Devices_DeviceId",
                table: "Parameters");

            migrationBuilder.RenameColumn(
                name: "DeviceId",
                table: "Parameters",
                newName: "DeviceTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Parameters_DeviceId",
                table: "Parameters",
                newName: "IX_Parameters_DeviceTypeId");

            migrationBuilder.AddColumn<int>(
                name: "DeviceTypeId",
                table: "Devices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "DeviceTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceTypes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Devices_DeviceTypeId",
                table: "Devices",
                column: "DeviceTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Devices_DeviceTypes_DeviceTypeId",
                table: "Devices",
                column: "DeviceTypeId",
                principalTable: "DeviceTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Parameters_DeviceTypes_DeviceTypeId",
                table: "Parameters",
                column: "DeviceTypeId",
                principalTable: "DeviceTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Devices_DeviceTypes_DeviceTypeId",
                table: "Devices");

            migrationBuilder.DropForeignKey(
                name: "FK_Parameters_DeviceTypes_DeviceTypeId",
                table: "Parameters");

            migrationBuilder.DropTable(
                name: "DeviceTypes");

            migrationBuilder.DropIndex(
                name: "IX_Devices_DeviceTypeId",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "DeviceTypeId",
                table: "Devices");

            migrationBuilder.RenameColumn(
                name: "DeviceTypeId",
                table: "Parameters",
                newName: "DeviceId");

            migrationBuilder.RenameIndex(
                name: "IX_Parameters_DeviceTypeId",
                table: "Parameters",
                newName: "IX_Parameters_DeviceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Parameters_Devices_DeviceId",
                table: "Parameters",
                column: "DeviceId",
                principalTable: "Devices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
