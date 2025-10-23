using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class editCourses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Classes",
                keyColumn: "ClassId",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Classes",
                keyColumn: "ClassId",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Classes",
                keyColumn: "ClassId",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Classes",
                keyColumn: "ClassId",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Classes",
                keyColumn: "ClassId",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Classes",
                keyColumn: "ClassId",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "CourseId",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "CourseId",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "CourseId",
                keyValue: 13);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Courses",
                columns: new[] { "CourseId", "College", "CourseName", "CourseNumber", "CreditHours", "DepartmentCode", "Level" },
                values: new object[,]
                {
                    { 11, "Engineering", "Introduction to Web Development", 105, 3, "CS", 100 },
                    { 12, "Engineering", "Introduction to Probability", 107, 2, "CS", 100 },
                    { 13, "Engineering", "Software Engineering Theory", 205, 2, "CS", 200 }
                });

            migrationBuilder.InsertData(
                table: "Classes",
                columns: new[] { "ClassId", "Capacity", "ClassSection", "Classroom", "CourseId", "DaysOfWeek", "EndTime", "EnrolledCount", "InstructorId", "StartTime" },
                values: new object[,]
                {
                    { 21, 40, "A", "ENG-301", 11, "W", new TimeSpan(0, 12, 0, 0, 0), 39, 3, new TimeSpan(0, 10, 0, 0, 0) },
                    { 22, 30, "B", "ENG-301", 11, "M", new TimeSpan(0, 16, 0, 0, 0), 28, 2, new TimeSpan(0, 14, 0, 0, 0) },
                    { 23, 30, "A", "ENG-301", 12, "U", new TimeSpan(0, 16, 0, 0, 0), 18, 2, new TimeSpan(0, 14, 0, 0, 0) },
                    { 24, 30, "B", "ENG-301", 12, "H", new TimeSpan(0, 16, 0, 0, 0), 18, 3, new TimeSpan(0, 14, 0, 0, 0) },
                    { 25, 30, "A", "ENG-301", 13, "T", new TimeSpan(0, 14, 0, 0, 0), 25, 4, new TimeSpan(0, 12, 0, 0, 0) },
                    { 26, 30, "B", "ENG-301", 13, "S", new TimeSpan(0, 12, 0, 0, 0), 20, 5, new TimeSpan(0, 10, 0, 0, 0) }
                });
        }
    }
}
