using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LS_Projekt_ASP_2026.Migrations
{
    /// <inheritdoc />
    public partial class AddStudioRoomToAudioProject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StudioRoomId",
                table: "AudioProjects",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AudioProjects_StudioRoomId",
                table: "AudioProjects",
                column: "StudioRoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_AudioProjects_StudioRooms_StudioRoomId",
                table: "AudioProjects",
                column: "StudioRoomId",
                principalTable: "StudioRooms",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AudioProjects_StudioRooms_StudioRoomId",
                table: "AudioProjects");

            migrationBuilder.DropIndex(
                name: "IX_AudioProjects_StudioRoomId",
                table: "AudioProjects");

            migrationBuilder.DropColumn(
                name: "StudioRoomId",
                table: "AudioProjects");
        }
    }
}
