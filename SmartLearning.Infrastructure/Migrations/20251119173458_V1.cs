using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartLearning.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class V1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Grades_AspNetUsers_Std_Id",
                table: "Grades");

            migrationBuilder.DropForeignKey(
                name: "FK_Grades_Quizzes_Quize_Id",
                table: "Grades");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentAnswer_Questions_QuestionsQuestion_Id",
                table: "StudentAnswer");

            migrationBuilder.DropTable(
                name: "AttendanceLessons");

            migrationBuilder.AlterColumn<int>(
                name: "QuestionsQuestion_Id",
                table: "StudentAnswer",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Question_Id",
                table: "StudentAnswer",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Course_Id",
                table: "Grades",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Lesson_Id",
                table: "Attendance",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Lesson_Id1",
                table: "Attendance",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "JobTitle",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfStudents",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Rating",
                table: "AspNetUsers",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "YoutubeChannelUrl",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Grades_Course_Id",
                table: "Grades",
                column: "Course_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Attendance_Lesson_Id1",
                table: "Attendance",
                column: "Lesson_Id1");

            migrationBuilder.AddForeignKey(
                name: "FK_Attendance_Lessons_Lesson_Id1",
                table: "Attendance",
                column: "Lesson_Id1",
                principalTable: "Lessons",
                principalColumn: "Lesson_Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Grades_AspNetUsers_Std_Id",
                table: "Grades",
                column: "Std_Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Grades_Courses_Course_Id",
                table: "Grades",
                column: "Course_Id",
                principalTable: "Courses",
                principalColumn: "Crs_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Grades_Quizzes_Quize_Id",
                table: "Grades",
                column: "Quize_Id",
                principalTable: "Quizzes",
                principalColumn: "Quiz_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentAnswer_Questions_QuestionsQuestion_Id",
                table: "StudentAnswer",
                column: "QuestionsQuestion_Id",
                principalTable: "Questions",
                principalColumn: "Question_Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attendance_Lessons_Lesson_Id1",
                table: "Attendance");

            migrationBuilder.DropForeignKey(
                name: "FK_Grades_AspNetUsers_Std_Id",
                table: "Grades");

            migrationBuilder.DropForeignKey(
                name: "FK_Grades_Courses_Course_Id",
                table: "Grades");

            migrationBuilder.DropForeignKey(
                name: "FK_Grades_Quizzes_Quize_Id",
                table: "Grades");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentAnswer_Questions_QuestionsQuestion_Id",
                table: "StudentAnswer");

            migrationBuilder.DropIndex(
                name: "IX_Grades_Course_Id",
                table: "Grades");

            migrationBuilder.DropIndex(
                name: "IX_Attendance_Lesson_Id1",
                table: "Attendance");

            migrationBuilder.DropColumn(
                name: "Question_Id",
                table: "StudentAnswer");

            migrationBuilder.DropColumn(
                name: "Course_Id",
                table: "Grades");

            migrationBuilder.DropColumn(
                name: "Lesson_Id",
                table: "Attendance");

            migrationBuilder.DropColumn(
                name: "Lesson_Id1",
                table: "Attendance");

            migrationBuilder.DropColumn(
                name: "JobTitle",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "NumberOfStudents",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Rating",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "YoutubeChannelUrl",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "QuestionsQuestion_Id",
                table: "StudentAnswer",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "AttendanceLessons",
                columns: table => new
                {
                    Attendance_Id = table.Column<int>(type: "int", nullable: false),
                    Lesson_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttendanceLessons", x => new { x.Attendance_Id, x.Lesson_Id });
                    table.ForeignKey(
                        name: "FK_AttendanceLessons_Attendance_Attendance_Id",
                        column: x => x.Attendance_Id,
                        principalTable: "Attendance",
                        principalColumn: "Attendance_Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AttendanceLessons_Lessons_Lesson_Id",
                        column: x => x.Lesson_Id,
                        principalTable: "Lessons",
                        principalColumn: "Lesson_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceLessons_Lesson_Id",
                table: "AttendanceLessons",
                column: "Lesson_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Grades_AspNetUsers_Std_Id",
                table: "Grades",
                column: "Std_Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Grades_Quizzes_Quize_Id",
                table: "Grades",
                column: "Quize_Id",
                principalTable: "Quizzes",
                principalColumn: "Quiz_Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentAnswer_Questions_QuestionsQuestion_Id",
                table: "StudentAnswer",
                column: "QuestionsQuestion_Id",
                principalTable: "Questions",
                principalColumn: "Question_Id");
        }
    }
}
