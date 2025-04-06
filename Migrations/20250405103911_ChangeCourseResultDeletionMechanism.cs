using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FacultySystem.Migrations
{
    /// <inheritdoc />
    public partial class ChangeCourseResultDeletionMechanism : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseResults_Trainees_TraineeId",
                table: "CourseResults");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseResults_Trainees_TraineeId",
                table: "CourseResults",
                column: "TraineeId",
                principalTable: "Trainees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseResults_Trainees_TraineeId",
                table: "CourseResults");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseResults_Trainees_TraineeId",
                table: "CourseResults",
                column: "TraineeId",
                principalTable: "Trainees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
