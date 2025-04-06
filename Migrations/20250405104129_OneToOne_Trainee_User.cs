using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FacultySystem.Migrations
{
    /// <inheritdoc />
    public partial class OneToOne_Trainee_User : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Trainees",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Trainees_UserId",
                table: "Trainees",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Trainees_AspNetUsers_UserId",
                table: "Trainees",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trainees_AspNetUsers_UserId",
                table: "Trainees");

            migrationBuilder.DropIndex(
                name: "IX_Trainees_UserId",
                table: "Trainees");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Trainees");
        }
    }
}
