using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace signa.api.Migrations
{
    /// <inheritdoc />
    public partial class SerialNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "identifier",
                table: "devices",
                newName: "serial_number");

            migrationBuilder.RenameIndex(
                name: "IX_devices_identifier",
                table: "devices",
                newName: "IX_devices_serial_number");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "serial_number",
                table: "devices",
                newName: "identifier");

            migrationBuilder.RenameIndex(
                name: "IX_devices_serial_number",
                table: "devices",
                newName: "IX_devices_identifier");
        }
    }
}
