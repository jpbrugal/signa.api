using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace signa.api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "devices",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    identifier = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "offline"),
                    battery = table.Column<int>(type: "int", nullable: true),
                    last_seen_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    deleted_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_devices", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    password_hash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    is_active = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    deleted_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "device_heartbeats",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    device_id = table.Column<int>(type: "int", nullable: false),
                    battery = table.Column<int>(type: "int", nullable: true),
                    network_status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ip_address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    received_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_device_heartbeats", x => x.id);
                    table.ForeignKey(
                        name: "FK_device_heartbeats_devices_device_id",
                        column: x => x.device_id,
                        principalTable: "devices",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "media",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    size_bytes = table.Column<long>(type: "bigint", nullable: true),
                    duration_sec = table.Column<int>(type: "int", nullable: true),
                    uploaded_by = table.Column<int>(type: "int", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    deleted_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_media", x => x.id);
                    table.ForeignKey(
                        name: "FK_media_users_uploaded_by",
                        column: x => x.uploaded_by,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "playlists",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: false),
                    created_by = table.Column<int>(type: "int", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    deleted_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_playlists", x => x.id);
                    table.ForeignKey(
                        name: "FK_playlists_users_created_by",
                        column: x => x.created_by,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "device_playlists",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    device_id = table.Column<int>(type: "int", nullable: false),
                    playlist_id = table.Column<int>(type: "int", nullable: false),
                    assigned_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_device_playlists", x => x.id);
                    table.ForeignKey(
                        name: "FK_device_playlists_devices_device_id",
                        column: x => x.device_id,
                        principalTable: "devices",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_device_playlists_playlists_playlist_id",
                        column: x => x.playlist_id,
                        principalTable: "playlists",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "playlist_media",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    playlist_id = table.Column<int>(type: "int", nullable: false),
                    media_id = table.Column<int>(type: "int", nullable: false),
                    position = table.Column<int>(type: "int", nullable: false),
                    duration_sec = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_playlist_media", x => x.id);
                    table.ForeignKey(
                        name: "FK_playlist_media_media_media_id",
                        column: x => x.media_id,
                        principalTable: "media",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_playlist_media_playlists_playlist_id",
                        column: x => x.playlist_id,
                        principalTable: "playlists",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_device_heartbeats_device_id",
                table: "device_heartbeats",
                column: "device_id");

            migrationBuilder.CreateIndex(
                name: "IX_device_playlists_device_id",
                table: "device_playlists",
                column: "device_id");

            migrationBuilder.CreateIndex(
                name: "IX_device_playlists_playlist_id",
                table: "device_playlists",
                column: "playlist_id");

            migrationBuilder.CreateIndex(
                name: "IX_devices_identifier",
                table: "devices",
                column: "identifier",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_media_uploaded_by",
                table: "media",
                column: "uploaded_by");

            migrationBuilder.CreateIndex(
                name: "IX_playlist_media_media_id",
                table: "playlist_media",
                column: "media_id");

            migrationBuilder.CreateIndex(
                name: "IX_playlist_media_playlist_id",
                table: "playlist_media",
                column: "playlist_id");

            migrationBuilder.CreateIndex(
                name: "IX_playlists_created_by",
                table: "playlists",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "IX_users_email",
                table: "users",
                column: "email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "device_heartbeats");

            migrationBuilder.DropTable(
                name: "device_playlists");

            migrationBuilder.DropTable(
                name: "playlist_media");

            migrationBuilder.DropTable(
                name: "devices");

            migrationBuilder.DropTable(
                name: "media");

            migrationBuilder.DropTable(
                name: "playlists");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
