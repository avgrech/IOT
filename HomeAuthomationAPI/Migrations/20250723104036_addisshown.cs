using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeAuthomationAPI.Migrations
{
    /// <inheritdoc />
    public partial class addisshown : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsShown",
                table: "Parameters",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsShown",
                table: "Parameters");
        }
    }
}
