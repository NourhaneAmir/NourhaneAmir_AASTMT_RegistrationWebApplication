using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    CourseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DepartmentCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CourseNumber = table.Column<int>(type: "int", nullable: false),
                    CourseName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreditHours = table.Column<int>(type: "int", nullable: false),
                    College = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.CourseId);
                });

            migrationBuilder.CreateTable(
                name: "Instructors",
                columns: table => new
                {
                    InstructorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InstructorName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Instructors", x => x.InstructorId);
                });

            migrationBuilder.CreateTable(
                name: "Semesters",
                columns: table => new
                {
                    SemesterId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SemesterName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    AcademicYear = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: false),
                    RegistrationStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RegistrationEnd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsCurrent = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Semesters", x => x.SemesterId);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    RegistrationNumber = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    College = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Department = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DepartmentCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    AdmissionYear = table.Column<int>(type: "int", maxLength: 20, nullable: false),
                    TotalCreditHours = table.Column<int>(type: "int", nullable: false),
                    GPA = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.RegistrationNumber);
                });

            migrationBuilder.CreateTable(
                name: "Prerequisites",
                columns: table => new
                {
                    PrerequisiteId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    RequiredCourseId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prerequisites", x => x.PrerequisiteId);
                    table.ForeignKey(
                        name: "FK_Prerequisites_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "CourseId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Prerequisites_Courses_RequiredCourseId",
                        column: x => x.RequiredCourseId,
                        principalTable: "Courses",
                        principalColumn: "CourseId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Classes",
                columns: table => new
                {
                    ClassId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    InstructorId = table.Column<int>(type: "int", nullable: false),
                    ClassSection = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    Capacity = table.Column<int>(type: "int", nullable: false),
                    EnrolledCount = table.Column<int>(type: "int", nullable: false),
                    DaysOfWeek = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    Classroom = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classes", x => x.ClassId);
                    table.ForeignKey(
                        name: "FK_Classes_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "CourseId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Classes_Instructors_InstructorId",
                        column: x => x.InstructorId,
                        principalTable: "Instructors",
                        principalColumn: "InstructorId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StudentCourseHistory",
                columns: table => new
                {
                    HistoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentRegistrationNumber = table.Column<int>(type: "int", nullable: false),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    SemesterId = table.Column<int>(type: "int", nullable: false),
                    Grade = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentCourseHistory", x => x.HistoryId);
                    table.ForeignKey(
                        name: "FK_StudentCourseHistory_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "CourseId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StudentCourseHistory_Semesters_SemesterId",
                        column: x => x.SemesterId,
                        principalTable: "Semesters",
                        principalColumn: "SemesterId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StudentCourseHistory_Students_StudentRegistrationNumber",
                        column: x => x.StudentRegistrationNumber,
                        principalTable: "Students",
                        principalColumn: "RegistrationNumber",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Registrations",
                columns: table => new
                {
                    RegistrationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentRegistrationNumber = table.Column<int>(type: "int", nullable: false),
                    ClassId = table.Column<int>(type: "int", nullable: false),
                    SemesterId = table.Column<int>(type: "int", nullable: false),
                    RegistrationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Registrations", x => x.RegistrationId);
                    table.ForeignKey(
                        name: "FK_Registrations_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "ClassId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Registrations_Semesters_SemesterId",
                        column: x => x.SemesterId,
                        principalTable: "Semesters",
                        principalColumn: "SemesterId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Registrations_Students_StudentRegistrationNumber",
                        column: x => x.StudentRegistrationNumber,
                        principalTable: "Students",
                        principalColumn: "RegistrationNumber",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Courses",
                columns: new[] { "CourseId", "College", "CourseName", "CourseNumber", "CreditHours", "DepartmentCode", "Level" },
                values: new object[,]
                {
                    { 1, "Engineering", "Introduction to Programming", 101, 3, "CS", 100 },
                    { 2, "Engineering", "Data Structures", 201, 3, "CS", 200 },
                    { 3, "Sciences", "Calculus I", 101, 4, "MATH", 100 },
                    { 4, "Engineering", "Database Systems", 301, 3, "CS", 300 },
                    { 5, "Arts", "Composition I", 101, 3, "ENG", 100 },
                    { 6, "Sciences", "General Physics", 101, 4, "PHYS", 100 },
                    { 7, "Engineering", "Object-Oriented Programming", 202, 3, "CS", 200 },
                    { 8, "Business", "Introduction to Management", 101, 3, "MGT", 100 },
                    { 9, "Business", "Organizational Behavior", 201, 3, "MGT", 200 },
                    { 10, "Engineering", "Math For CS", 204, 3, "CS", 200 },
                    { 11, "Engineering", "Introduction to Web Development", 105, 3, "CS", 100 },
                    { 12, "Engineering", "Introduction to Probability", 107, 2, "CS", 100 },
                    { 13, "Engineering", "Software Engineering Theory", 205, 2, "CS", 200 }
                });

            migrationBuilder.InsertData(
                table: "Instructors",
                columns: new[] { "InstructorId", "Email", "InstructorName" },
                values: new object[,]
                {
                    { 1, "mai_allam@alexu.edu.eg", "Dr. Mai Allam" },
                    { 2, "n.ramzy@alexu.edu.eg", "Prof. Noha Ramzy" },
                    { 3, "a.magdy@alexu.edu.eg", "Dr. Ahmed Magdy" },
                    { 4, "ramysamy12@alexu.edu.eg", "Prof. Ramy Samy" },
                    { 5, "sara_salama@alexu.edu.eg", "Dr. Sara Salama" }
                });

            migrationBuilder.InsertData(
                table: "Semesters",
                columns: new[] { "SemesterId", "AcademicYear", "IsCurrent", "RegistrationEnd", "RegistrationStart", "SemesterName" },
                values: new object[,]
                {
                    { 1, "2024-2025", false, new DateTime(2024, 8, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Fall" },
                    { 2, "2024-2025", false, new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Spring" },
                    { 3, "2025-2026", true, new DateTime(2025, 10, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 10, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Fall" }
                });

            migrationBuilder.InsertData(
                table: "Students",
                columns: new[] { "RegistrationNumber", "AdmissionYear", "College", "Department", "DepartmentCode", "FullName", "GPA", "PasswordHash", "Status", "TotalCreditHours" },
                values: new object[,]
                {
                    { 20240001, 2024, "Engineering", "Computer Science", "CS", "Sameh Shokry", 3.75m, "Sameh_2024_0001", "Regular", 45 },
                    { 20240002, 2024, "Business", "Management", "MGT", "Maria Gabrielle", 3.20m, "Maria_2024_0002", "Regular", 45 },
                    { 20240003, 2020, "Engineering", "Computer Science", "CS", "Ahmed Hassan", 3.90m, "Ahmed_2024_0003", "Graduated", 120 },
                    { 20240004, 2024, "Engineering", "Computer Science", "CS", "Sama Medhat", 3.45m, "Sama_2024_0004", "Regular", 30 }
                });

            migrationBuilder.InsertData(
                table: "Classes",
                columns: new[] { "ClassId", "Capacity", "ClassSection", "Classroom", "CourseId", "DaysOfWeek", "EndTime", "EnrolledCount", "InstructorId", "StartTime" },
                values: new object[,]
                {
                    { 1, 30, "A", "ENG-101", 1, "S", new TimeSpan(0, 11, 0, 0, 0), 29, 1, new TimeSpan(0, 9, 0, 0, 0) },
                    { 2, 30, "B", "ENG-102", 1, "T", new TimeSpan(0, 12, 0, 0, 0), 18, 2, new TimeSpan(0, 10, 0, 0, 0) },
                    { 3, 25, "A", "ENG-201", 2, "W", new TimeSpan(0, 14, 0, 0, 0), 22, 1, new TimeSpan(0, 12, 0, 0, 0) },
                    { 4, 40, "A", "SCI-105", 3, "M", new TimeSpan(0, 10, 0, 0, 0), 38, 1, new TimeSpan(0, 8, 0, 0, 0) },
                    { 5, 35, "A", "ART-102", 5, "S", new TimeSpan(0, 16, 0, 0, 0), 28, 3, new TimeSpan(0, 14, 0, 0, 0) },
                    { 6, 50, "A", "SCI-201", 6, "T", new TimeSpan(0, 10, 0, 0, 0), 42, 4, new TimeSpan(0, 8, 0, 0, 0) },
                    { 7, 30, "A", "ENG-301", 7, "S", new TimeSpan(0, 14, 0, 0, 0), 18, 5, new TimeSpan(0, 12, 0, 0, 0) },
                    { 8, 25, "B", "ENG-201", 2, "U", new TimeSpan(0, 10, 0, 0, 0), 24, 2, new TimeSpan(0, 8, 0, 0, 0) },
                    { 9, 40, "B", "SCI-105", 3, "T", new TimeSpan(0, 12, 0, 0, 0), 39, 3, new TimeSpan(0, 10, 0, 0, 0) },
                    { 10, 40, "A", "SCI-105", 4, "H", new TimeSpan(0, 18, 0, 0, 0), 38, 1, new TimeSpan(0, 16, 0, 0, 0) },
                    { 11, 40, "B", "SCI-105", 4, "U", new TimeSpan(0, 14, 0, 0, 0), 38, 4, new TimeSpan(0, 12, 0, 0, 0) },
                    { 12, 40, "B", "SCI-105", 5, "M", new TimeSpan(0, 15, 0, 0, 0), 38, 1, new TimeSpan(0, 13, 0, 0, 0) },
                    { 13, 50, "B", "SCI-201", 6, "H", new TimeSpan(0, 10, 0, 0, 0), 42, 1, new TimeSpan(0, 8, 0, 0, 0) },
                    { 14, 30, "B", "ENG-301", 7, "W", new TimeSpan(0, 16, 0, 0, 0), 18, 1, new TimeSpan(0, 14, 0, 0, 0) },
                    { 15, 30, "A", "ENG-301", 8, "M", new TimeSpan(0, 14, 0, 0, 0), 18, 3, new TimeSpan(0, 12, 0, 0, 0) },
                    { 16, 30, "B", "ENG-301", 8, "U", new TimeSpan(0, 16, 0, 0, 0), 18, 5, new TimeSpan(0, 14, 0, 0, 0) },
                    { 17, 30, "A", "ENG-301", 9, "T", new TimeSpan(0, 16, 0, 0, 0), 18, 3, new TimeSpan(0, 14, 0, 0, 0) },
                    { 18, 30, "B", "ENG-301", 9, "S", new TimeSpan(0, 10, 0, 0, 0), 18, 5, new TimeSpan(0, 8, 0, 0, 0) },
                    { 19, 30, "A", "ENG-301", 10, "H", new TimeSpan(0, 18, 0, 0, 0), 29, 1, new TimeSpan(0, 16, 0, 0, 0) },
                    { 20, 30, "B", "ENG-301", 10, "U", new TimeSpan(0, 12, 0, 0, 0), 29, 5, new TimeSpan(0, 10, 0, 0, 0) },
                    { 21, 40, "A", "ENG-301", 11, "W", new TimeSpan(0, 12, 0, 0, 0), 39, 3, new TimeSpan(0, 10, 0, 0, 0) },
                    { 22, 30, "B", "ENG-301", 11, "M", new TimeSpan(0, 16, 0, 0, 0), 28, 2, new TimeSpan(0, 14, 0, 0, 0) },
                    { 23, 30, "A", "ENG-301", 12, "U", new TimeSpan(0, 16, 0, 0, 0), 18, 2, new TimeSpan(0, 14, 0, 0, 0) },
                    { 24, 30, "B", "ENG-301", 12, "H", new TimeSpan(0, 16, 0, 0, 0), 18, 3, new TimeSpan(0, 14, 0, 0, 0) },
                    { 25, 30, "A", "ENG-301", 13, "T", new TimeSpan(0, 14, 0, 0, 0), 25, 4, new TimeSpan(0, 12, 0, 0, 0) },
                    { 26, 30, "B", "ENG-301", 13, "S", new TimeSpan(0, 12, 0, 0, 0), 20, 5, new TimeSpan(0, 10, 0, 0, 0) }
                });

            migrationBuilder.InsertData(
                table: "Prerequisites",
                columns: new[] { "PrerequisiteId", "CourseId", "RequiredCourseId" },
                values: new object[,]
                {
                    { 1, 2, 1 },
                    { 2, 4, 2 },
                    { 3, 7, 1 }
                });

            migrationBuilder.InsertData(
                table: "StudentCourseHistory",
                columns: new[] { "HistoryId", "CourseId", "Grade", "IsCompleted", "SemesterId", "StudentRegistrationNumber" },
                values: new object[,]
                {
                    { 1, 1, 3.8m, true, 3, 20240001 },
                    { 2, 3, 3.5m, true, 3, 20240001 },
                    { 3, 5, 3.2m, true, 3, 20240001 },
                    { 4, 1, 3.2m, true, 3, 20240002 },
                    { 5, 1, 3.6m, true, 3, 20240004 }
                });

            migrationBuilder.InsertData(
                table: "Registrations",
                columns: new[] { "RegistrationId", "ClassId", "RegistrationDate", "SemesterId", "Status", "StudentRegistrationNumber" },
                values: new object[,]
                {
                    { 1, 3, new DateTime(2025, 10, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), 3, "Registered", 20240001 },
                    { 2, 5, new DateTime(2025, 10, 15, 10, 35, 0, 0, DateTimeKind.Unspecified), 3, "Registered", 20240001 },
                    { 3, 1, new DateTime(2025, 10, 16, 9, 15, 0, 0, DateTimeKind.Unspecified), 3, "Registered", 20240002 },
                    { 4, 3, new DateTime(2025, 10, 16, 11, 20, 0, 0, DateTimeKind.Unspecified), 3, "Registered", 20240004 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Classes_CourseId_ClassSection",
                table: "Classes",
                columns: new[] { "CourseId", "ClassSection" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Classes_InstructorId",
                table: "Classes",
                column: "InstructorId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_DepartmentCode_CourseNumber",
                table: "Courses",
                columns: new[] { "DepartmentCode", "CourseNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Prerequisites_CourseId",
                table: "Prerequisites",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Prerequisites_RequiredCourseId",
                table: "Prerequisites",
                column: "RequiredCourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Registrations_ClassId",
                table: "Registrations",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_Registrations_SemesterId",
                table: "Registrations",
                column: "SemesterId");

            migrationBuilder.CreateIndex(
                name: "IX_Registrations_StudentRegistrationNumber_ClassId_SemesterId",
                table: "Registrations",
                columns: new[] { "StudentRegistrationNumber", "ClassId", "SemesterId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudentCourseHistory_CourseId",
                table: "StudentCourseHistory",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentCourseHistory_SemesterId",
                table: "StudentCourseHistory",
                column: "SemesterId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentCourseHistory_StudentRegistrationNumber",
                table: "StudentCourseHistory",
                column: "StudentRegistrationNumber");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Prerequisites");

            migrationBuilder.DropTable(
                name: "Registrations");

            migrationBuilder.DropTable(
                name: "StudentCourseHistory");

            migrationBuilder.DropTable(
                name: "Classes");

            migrationBuilder.DropTable(
                name: "Semesters");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropTable(
                name: "Instructors");
        }
    }
}
