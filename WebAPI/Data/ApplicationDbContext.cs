using Microsoft.EntityFrameworkCore;
using StudentRegistration.Shared.Models;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace WebAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Class> Classes { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Prerequisite> Prerequisites { get; set; }
        public DbSet<Registration> Registrations { get; set;  }
        public DbSet<Semester> Semesters { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<StudentCourseHistory> StudentCourseHistory { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Class>()
                .HasIndex(c => new { c.CourseId, c.ClassSection })
                .IsUnique();

            modelBuilder.Entity<Course>()
                .HasIndex(c => new { c.DepartmentCode, c.CourseNumber })
                .IsUnique();

            modelBuilder.Entity<Registration>()
                .HasIndex(r => new { r.StudentRegistrationNumber, r.ClassId, r.SemesterId })
                .IsUnique();



            //Relationship configurations

            modelBuilder.Entity<Student>()
                .HasMany(s => s.Registrations)
                .WithOne(r => r.Student)
                .HasForeignKey(r => r.StudentRegistrationNumber)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Student>()
                .HasMany(s => s.CourseHistory)
                .WithOne(h => h.Student)
                .HasForeignKey(h => h.StudentRegistrationNumber)
                .OnDelete(DeleteBehavior.Restrict);

            
            modelBuilder.Entity<Course>()
                .HasMany(c => c.Classes)
                .WithOne(cl => cl.Course)
                .HasForeignKey(cl => cl.CourseId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Course>()
                .HasMany(c => c.Prerequisites)
                .WithOne(p => p.Course)
                .HasForeignKey(p => p.CourseId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Course>()
                .HasMany(c => c.StudentHistory)
                .WithOne(h => h.Course)
                .HasForeignKey(h => h.CourseId)
                .OnDelete(DeleteBehavior.Restrict);

            
            modelBuilder.Entity<Semester>()
                .HasMany(s => s.Registrations)
                .WithOne(r => r.Semester)
                .HasForeignKey(r => r.SemesterId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Semester>()
                .HasMany(s => s.CourseHistory)
                .WithOne(h => h.Semester)
                .HasForeignKey(h => h.SemesterId)
                .OnDelete(DeleteBehavior.Restrict);

            
            modelBuilder.Entity<Instructor>()
                .HasMany(i => i.Classes)
                .WithOne(c => c.Instructor)
                .HasForeignKey(c => c.InstructorId)
                .OnDelete(DeleteBehavior.Restrict);

            
            modelBuilder.Entity<Class>()
                .HasMany(c => c.Registrations)
                .WithOne(r => r.Class)
                .HasForeignKey(r => r.ClassId)
                .OnDelete(DeleteBehavior.Restrict);

            
            modelBuilder.Entity<Prerequisite>()
                .HasOne(p => p.RequiredCourse)
                .WithMany()
                .HasForeignKey(p => p.RequiredCourseId)
                .OnDelete(DeleteBehavior.Restrict);

            // A7ot el data f el database
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Instructors Data
            modelBuilder.Entity<Instructor>().HasData(
                new Instructor { InstructorId = 1, InstructorName = "Dr. Mai Allam", Email = "mai_allam@alexu.edu.eg" },
                new Instructor { InstructorId = 2, InstructorName = "Prof. Noha Ramzy", Email = "n.ramzy@alexu.edu.eg" },
                new Instructor { InstructorId = 3, InstructorName = "Dr. Ahmed Magdy", Email = "a.magdy@alexu.edu.eg" },
                new Instructor { InstructorId = 4, InstructorName = "Prof. Ramy Samy", Email = "ramysamy12@alexu.edu.eg" },
                new Instructor { InstructorId = 5, InstructorName = "Dr. Sara Salama", Email = "sara_salama@alexu.edu.eg" }
            );

            // Students Data
            modelBuilder.Entity<Student>().HasData(
                new Student
                {
                    RegistrationNumber = 20240001,
                    PasswordHash = "Sameh_2024_0001",
                    FullName = "Sameh Shokry",
                    College = "Engineering",
                    Department = "Computer Science",
                    DepartmentCode = "CS",
                    AdmissionYear = 2024,
                    TotalCreditHours = 45,
                    GPA = 3.75m,
                    Status = "Regular"
                },
                new Student
                {
                    RegistrationNumber = 20240002,
                    PasswordHash = "Maria_2024_0002",
                    FullName = "Maria Gabrielle",
                    College = "Business",
                    Department = "Management",
                    DepartmentCode = "MGT",
                    AdmissionYear = 2024,
                    TotalCreditHours = 45,
                    GPA = 3.20m,
                    Status = "Regular"
                },
                new Student
                {
                    RegistrationNumber = 20240003,
                    PasswordHash = "Ahmed_2024_0003",
                    FullName = "Ahmed Hassan",
                    College = "Engineering",
                    Department = "Computer Science",
                    DepartmentCode = "CS",
                    AdmissionYear = 2020,
                    TotalCreditHours = 120,
                    GPA = 3.90m,
                    Status = "Graduated"
                },
                new Student
                {
                    RegistrationNumber = 20240004,
                    PasswordHash = "Sama_2024_0004",
                    FullName = "Sama Medhat",
                    College = "Engineering",
                    Department = "Computer Science",
                    DepartmentCode = "CS",
                    AdmissionYear = 2024,
                    TotalCreditHours = 30,
                    GPA = 3.45m,
                    Status = "Regular"
                }
            );

            // Semesters Data
            modelBuilder.Entity<Semester>().HasData(
                new Semester
                {
                    SemesterId = 1,
                    SemesterName = "Fall",
                    AcademicYear = "2024-2025",
                    RegistrationStart = new DateTime(2024, 8, 1),
                    RegistrationEnd = new DateTime(2024, 8, 30),
                    IsCurrent = false
                },
                new Semester
                {
                    SemesterId = 2,
                    SemesterName = "Spring",
                    AcademicYear = "2024-2025",
                    RegistrationStart = new DateTime(2024, 12, 1),
                    RegistrationEnd = new DateTime(2025, 1, 15),
                    IsCurrent = false
                },
                new Semester
                {
                    SemesterId = 3,
                    SemesterName = "Fall",
                    AcademicYear = "2025-2026",
                    RegistrationStart = new DateTime(2025, 10, 15),
                    RegistrationEnd = new DateTime(2025, 10, 30),
                    IsCurrent = true
                }
            );

            // Courses Data
            modelBuilder.Entity<Course>().HasData(
                new Course
                {
                    CourseId = 1,
                    DepartmentCode = "CS",
                    CourseNumber = 101,
                    CourseName = "Introduction to Programming",
                    CreditHours = 3,
                    College = "Engineering",
                    Level = 100
                },
                new Course
                {
                    CourseId = 2,
                    DepartmentCode = "CS",
                    CourseNumber = 201,
                    CourseName = "Data Structures",
                    CreditHours = 3,
                    College = "Engineering",
                    Level = 200
                },
                new Course
                {
                    CourseId = 3,
                    DepartmentCode = "MATH",
                    CourseNumber = 101,
                    CourseName = "Calculus I",
                    CreditHours = 4,
                    College = "Sciences",
                    Level = 100
                },
                new Course
                {
                    CourseId = 4,
                    DepartmentCode = "CS",
                    CourseNumber = 301,
                    CourseName = "Database Systems",
                    CreditHours = 3,
                    College = "Engineering",
                    Level = 300
                },
                new Course
                {
                    CourseId = 5,
                    DepartmentCode = "ENG",
                    CourseNumber = 101,
                    CourseName = "Composition I",
                    CreditHours = 3,
                    College = "Arts",
                    Level = 100
                },
                new Course
                {
                    CourseId = 6,
                    DepartmentCode = "PHYS",
                    CourseNumber = 101,
                    CourseName = "General Physics",
                    CreditHours = 4,
                    College = "Sciences",
                    Level = 100
                },
                new Course
                {
                    CourseId = 7,
                    DepartmentCode = "CS",
                    CourseNumber = 202,
                    CourseName = "Object-Oriented Programming",
                    CreditHours = 3,
                    College = "Engineering",
                    Level = 200
                },
                new Course
                {
                    CourseId = 8,
                    DepartmentCode = "MGT",
                    CourseNumber = 101,
                    CourseName = "Introduction to Management",
                    CreditHours = 3,
                    College = "Business",
                    Level = 100
                },
                new Course
                {
                    CourseId = 9,
                    DepartmentCode = "MGT",
                    CourseNumber = 201,
                    CourseName = "Organizational Behavior",
                    CreditHours = 3,
                    College = "Business",
                    Level = 200
                }, 
                new Course
                {
                    CourseId = 10,
                    DepartmentCode = "CS",
                    CourseNumber = 204,
                    CourseName = "Math For CS",
                    CreditHours = 3,
                    College = "Engineering",
                    Level = 200
                }
            );

            // Classes Data
            modelBuilder.Entity<Class>().HasData(
                //course 1
                new Class
                {
                    ClassId = 1,
                    CourseId = 1,
                    InstructorId = 1,
                    ClassSection = "A",
                    Capacity = 30,
                    EnrolledCount = 29,
                    DaysOfWeek = "S",
                    StartTime = new TimeSpan(9, 0, 0),
                    EndTime = new TimeSpan(11, 0, 0),
                    Classroom = "ENG-101"
                },
                new Class
                {
                    ClassId = 2,
                    CourseId = 1,
                    InstructorId = 2,
                    ClassSection = "B",
                    Capacity = 30,
                    EnrolledCount = 18,
                    DaysOfWeek = "T",
                    StartTime = new TimeSpan(10, 0, 0),
                    EndTime = new TimeSpan(12, 0, 0),
                    Classroom = "ENG-102"
                },

                //course 2
                new Class
                {
                    ClassId = 3,
                    CourseId = 2,
                    InstructorId = 1,
                    ClassSection = "A",
                    Capacity = 25,
                    EnrolledCount = 22,
                    DaysOfWeek = "W",
                    StartTime = new TimeSpan(12, 0, 0),
                    EndTime = new TimeSpan(14, 0, 0),
                    Classroom = "ENG-201"
                },
                new Class
                {
                    ClassId = 8,
                    CourseId = 2,
                    InstructorId = 2,
                    ClassSection = "B",
                    Capacity = 25,
                    EnrolledCount = 24,
                    DaysOfWeek = "U",
                    StartTime = new TimeSpan(8, 0, 0),
                    EndTime = new TimeSpan(10, 0, 0),
                    Classroom = "ENG-201"
                },

                //course 3
                new Class
                {
                    ClassId = 4,
                    CourseId = 3,
                    InstructorId = 1,
                    ClassSection = "A",
                    Capacity = 40,
                    EnrolledCount = 38,
                    DaysOfWeek = "M",
                    StartTime = new TimeSpan(8, 0, 0),
                    EndTime = new TimeSpan(10, 0, 0),
                    Classroom = "SCI-105"
                },
                new Class
                {
                    ClassId = 9,
                    CourseId = 3,
                    InstructorId = 3,
                    ClassSection = "B",
                    Capacity = 40,
                    EnrolledCount = 39,
                    DaysOfWeek = "T",
                    StartTime = new TimeSpan(10, 0, 0),
                    EndTime = new TimeSpan(12, 0, 0),
                    Classroom = "SCI-105"
                },

                //course 4
                new Class
                {
                    ClassId = 10,
                    CourseId = 4,
                    InstructorId = 1,
                    ClassSection = "A",
                    Capacity = 40,
                    EnrolledCount = 38,
                    DaysOfWeek = "H",
                    StartTime = new TimeSpan(16, 0, 0),
                    EndTime = new TimeSpan(18, 0, 0),
                    Classroom = "SCI-105"
                },
                new Class
                {
                    ClassId = 11,
                    CourseId = 4,
                    InstructorId = 4,
                    ClassSection = "B",
                    Capacity = 40,
                    EnrolledCount = 38,
                    DaysOfWeek = "U",
                    StartTime = new TimeSpan(12, 0, 0),
                    EndTime = new TimeSpan(14, 0, 0),
                    Classroom = "SCI-105"
                },
                //course 5
                new Class
                {
                    ClassId = 5,
                    CourseId = 5,
                    InstructorId = 3,
                    ClassSection = "A",
                    Capacity = 35,
                    EnrolledCount = 28,
                    DaysOfWeek = "S",
                    StartTime = new TimeSpan(14, 0, 0),
                    EndTime = new TimeSpan(16, 0, 0),
                    Classroom = "ART-102"
                },
                new Class
                {
                    ClassId = 12,
                    CourseId = 5,
                    InstructorId = 1,
                    ClassSection = "B",
                    Capacity = 40,
                    EnrolledCount = 38,
                    DaysOfWeek = "M",
                    StartTime = new TimeSpan(13, 0, 0),
                    EndTime = new TimeSpan(15, 0, 0),
                    Classroom = "SCI-105"
                },

                //course 6
                new Class
                {
                    ClassId = 6,
                    CourseId = 6,
                    InstructorId = 4,
                    ClassSection = "A",
                    Capacity = 50,
                    EnrolledCount = 42,
                    DaysOfWeek = "T",
                    StartTime = new TimeSpan(8, 0, 0),
                    EndTime = new TimeSpan(10, 0, 0),
                    Classroom = "SCI-201"
                },
                new Class
                {
                    ClassId = 13,
                    CourseId = 6,
                    InstructorId = 1,
                    ClassSection = "B",
                    Capacity = 50,
                    EnrolledCount = 42,
                    DaysOfWeek = "H",
                    StartTime = new TimeSpan(8, 0, 0),
                    EndTime = new TimeSpan(10, 0, 0),
                    Classroom = "SCI-201"
                },

                //course 7
                new Class
                {
                    ClassId = 7,
                    CourseId = 7,
                    InstructorId = 5,
                    ClassSection = "A",
                    Capacity = 30,
                    EnrolledCount = 18,
                    DaysOfWeek = "S",
                    StartTime = new TimeSpan(12, 0, 0),
                    EndTime = new TimeSpan(14, 0, 0),
                    Classroom = "ENG-301"
                },
                new Class
                {
                    ClassId = 14,
                    CourseId = 7,
                    InstructorId = 1,
                    ClassSection = "B",
                    Capacity = 30,
                    EnrolledCount = 18,
                    DaysOfWeek = "W",
                    StartTime = new TimeSpan(14, 0, 0),
                    EndTime = new TimeSpan(16, 0, 0),
                    Classroom = "ENG-301"
                },

                //course 8
                new Class
                {
                    ClassId = 15,
                    CourseId = 8,
                    InstructorId = 3,
                    ClassSection = "A",
                    Capacity = 30,
                    EnrolledCount = 18,
                    DaysOfWeek = "M",
                    StartTime = new TimeSpan(12, 0, 0),
                    EndTime = new TimeSpan(14, 0, 0),
                    Classroom = "ENG-301"
                },
                new Class
                {
                    ClassId = 16,
                    CourseId = 8,
                    InstructorId = 5,
                    ClassSection = "B",
                    Capacity = 30,
                    EnrolledCount = 18,
                    DaysOfWeek = "U",
                    StartTime = new TimeSpan(14, 0, 0),
                    EndTime = new TimeSpan(16, 0, 0),
                    Classroom = "ENG-301"
                },

                //course 9
                new Class
                {
                    ClassId = 17,
                    CourseId = 9,
                    InstructorId = 3,
                    ClassSection = "A",
                    Capacity = 30,
                    EnrolledCount = 18,
                    DaysOfWeek = "T",
                    StartTime = new TimeSpan(14, 0, 0),
                    EndTime = new TimeSpan(16, 0, 0),
                    Classroom = "ENG-301"
                },
                new Class
                {
                    ClassId = 18,
                    CourseId = 9,
                    InstructorId = 5,
                    ClassSection = "B",
                    Capacity = 30,
                    EnrolledCount = 18,
                    DaysOfWeek = "S",
                    StartTime = new TimeSpan(8, 0, 0),
                    EndTime = new TimeSpan(10, 0, 0),
                    Classroom = "ENG-301"
                },

                //course 10
                new Class
                {
                    ClassId = 19,
                    CourseId = 10,
                    InstructorId = 1,
                    ClassSection = "A",
                    Capacity = 30,
                    EnrolledCount = 29,
                    DaysOfWeek = "H",
                    StartTime = new TimeSpan(16, 0, 0),
                    EndTime = new TimeSpan(18, 0, 0),
                    Classroom = "ENG-301"
                },
                new Class
                {
                    ClassId = 20,
                    CourseId = 10,
                    InstructorId = 5,
                    ClassSection = "B",
                    Capacity = 30,
                    EnrolledCount = 29,
                    DaysOfWeek = "U",
                    StartTime = new TimeSpan(10, 0, 0),
                    EndTime = new TimeSpan(12, 0, 0),
                    Classroom = "ENG-301"
                }
            );

            // Prerequisites Data
            modelBuilder.Entity<Prerequisite>().HasData(
                new Prerequisite { PrerequisiteId = 1, CourseId = 2, RequiredCourseId = 1 }, 
                new Prerequisite { PrerequisiteId = 2, CourseId = 4, RequiredCourseId = 2 }, 
                new Prerequisite { PrerequisiteId = 3, CourseId = 7, RequiredCourseId = 1 } 
            );

            // Student Course History Data
            modelBuilder.Entity<StudentCourseHistory>().HasData(
                new StudentCourseHistory
                {
                    HistoryId = 1,
                    StudentRegistrationNumber = 20240001,
                    CourseId = 1,
                    SemesterId = 3,
                    Grade = 3.8m,
                    IsCompleted = true
                },
                new StudentCourseHistory
                {
                    HistoryId = 2,
                    StudentRegistrationNumber = 20240001,
                    CourseId = 3,
                    SemesterId = 3,
                    Grade = 3.5m,
                    IsCompleted = true
                },
                new StudentCourseHistory
                {
                    HistoryId = 3,
                    StudentRegistrationNumber = 20240001,
                    CourseId = 5,
                    SemesterId = 3,
                    Grade = 3.2m,
                    IsCompleted = true
                },
                new StudentCourseHistory
                {
                    HistoryId = 4,
                    StudentRegistrationNumber = 20240002,
                    CourseId = 1,
                    SemesterId = 3,
                    Grade = 3.2m,
                    IsCompleted = true
                },
                new StudentCourseHistory
                {
                    HistoryId = 5,
                    StudentRegistrationNumber = 20240004,
                    CourseId = 1,
                    SemesterId = 3,
                    Grade = 3.6m,
                    IsCompleted = true
                }
            );

            // Registrations Data
            modelBuilder.Entity<Registration>().HasData(
                new Registration
                {
                    RegistrationId = 1,
                    StudentRegistrationNumber = 20240001,
                    ClassId = 3,
                    SemesterId = 3,
                    RegistrationDate = new DateTime(2025, 10, 15, 10, 30, 0),
                    Status = "Registered"
                },
                new Registration
                {
                    RegistrationId = 2,
                    StudentRegistrationNumber = 20240001,
                    ClassId = 5,
                    SemesterId = 3,
                    RegistrationDate = new DateTime(2025, 10, 15, 10, 35, 0),
                    Status = "Registered"
                },
                new Registration
                {
                    RegistrationId = 3,
                    StudentRegistrationNumber = 20240002,
                    ClassId = 1,
                    SemesterId = 3,
                    RegistrationDate = new DateTime(2025, 10, 16, 9, 15, 0),
                    Status = "Registered"
                },
                new Registration
                {
                    RegistrationId = 4,
                    StudentRegistrationNumber = 20240004,
                    ClassId = 3,
                    SemesterId = 3,
                    RegistrationDate = new DateTime(2025, 10, 16, 11, 20, 0),
                    Status = "Registered"
                }
            );
        }
    }
}
