using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeAuthomationAPI.Migrations
{
    /// <inheritdoc />
    public partial class RemoveDeviceId1Manually : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop foreign key if it exists
            migrationBuilder.Sql(@"
        IF EXISTS (
            SELECT * FROM INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE 
            WHERE TABLE_NAME = 'DeviceStatus' AND COLUMN_NAME = 'DeviceId1'
        )
        BEGIN
            ALTER TABLE DeviceStatus DROP CONSTRAINT [FK_DeviceStatus_Device_DeviceId1]
        END
    ");

            // Drop index if it exists
            migrationBuilder.Sql(@"
        IF EXISTS (
            SELECT * FROM sys.indexes 
            WHERE name = 'IX_DeviceStatus_DeviceId1' AND object_id = OBJECT_ID('DeviceStatus')
        )
        BEGIN
            DROP INDEX [IX_DeviceStatus_DeviceId1] ON [DeviceStatus]
        END
    ");

            // Drop column
            migrationBuilder.Sql(@"
        IF EXISTS (
            SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE TABLE_NAME = 'DeviceStatus' AND COLUMN_NAME = 'DeviceId1'
        )
        BEGIN
            ALTER TABLE DeviceStatus DROP COLUMN DeviceId1
        END
    ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Optional: re-add if needed (unlikely)
            migrationBuilder.AddColumn<int>(
                name: "DeviceId1",
                table: "DeviceStatus",
                type: "int",
                nullable: true);
        }

    }
}
