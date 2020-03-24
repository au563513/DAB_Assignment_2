using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using HelpRequestSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace HelpRequestSystem.Data
{
    public class HrsContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder ob)
        {
            string username = "sa";
            string password = "S4ltsalt";
            ob.UseSqlServer("Data Source=localhost,1433;Database=HelpRequestdb;User ID="+username+";Password="+password+";");
        }

        //DBset
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<Exercise> Exercises { get; set; }

        protected override void OnModelCreating(ModelBuilder mb)
        {
            #region Setup PK

            mb.Entity<Student>().HasKey(student => student.StudentId);
            mb.Entity<Teacher>().HasKey(teacher => teacher.TeacherId);
            mb.Entity<Course>().HasKey(course => course.CourseId);
            mb.Entity<Assignment>().HasKey(assignment => assignment.AssignmentId);
            mb.Entity<Exercise>().HasKey(exercise => new {exercise.Lecture, exercise.Number});
            mb.Entity<StudentAssignment>().HasKey(sa=>new {sa.StudentId,sa.AssignmentId});
            mb.Entity<StudentCourse>().HasKey(sc=>new {sc.StudentId,sc.CourseId});

            #endregion



            #region SQL Attribute constraints

            mb.Entity<Student>().Property(s => s.StudentId).ValueGeneratedNever();

            mb.Entity<Exercise>().Property(e => e.Lecture).HasMaxLength(40);

            #endregion



            #region Setup FK and relations

            //Student - Assignment (Many to Many)
            //Assignment - Student
            mb.Entity<StudentAssignment>()
                .HasOne(sa=>sa.Student)
                .WithMany(s=>s.StudentAssignments)
                .HasForeignKey(sa=>sa.StudentId);
            mb.Entity<StudentAssignment>()
                .HasOne(sa=>sa.Assignment)
                .WithMany(s=>s.StudentAssignments)
                .HasForeignKey(sa=>sa.AssignmentId);

            //Student - Course (Many to Many)
            //Course - Student
            mb.Entity<StudentCourse>()
                .HasOne(sc => sc.Student)
                .WithMany(s => s.StudentCourses)
                .HasForeignKey(sc => sc.StudentId);
            mb.Entity<StudentCourse>()
                .HasOne(sc => sc.Course)
                .WithMany(c => c.StudentCourses)
                .HasForeignKey(sc => sc.CourseId);

            //Student - Exercise (One to Many)
            mb.Entity<Exercise>()
                .HasOne(e => e.Student)
                .WithMany(s => s.Exercises)
                .HasForeignKey(e => e.StudentId);

            //Course - Exercise (One to Many)
            mb.Entity<Exercise>()
                .HasOne(e => e.Course)
                .WithMany(s => s.Exercises)
                .HasForeignKey(e => e.CourseId);

            //Teacher - Exercise (One to Many)
            mb.Entity<Exercise>()
                .HasOne(e => e.Teacher)
                .WithMany(s => s.Exercises)
                .OnDelete(DeleteBehavior.ClientNoAction)
                .HasForeignKey(e => e.TeacherId);

            //Teacher - Assignment (One to Many)
            mb.Entity<Assignment>()
                .HasOne(a => a.Teacher)
                .WithMany(t => t.Assignments)
                .OnDelete(DeleteBehavior.NoAction)
                .HasForeignKey(a => a.TeacherId);

            //Course - Assignment (One to Many)
            mb.Entity<Assignment>()
                .HasOne(a => a.Course)
                .WithMany(c => c.Assignments)
                .HasForeignKey(a => a.CourseId);

            //Course - Teacher (One to Many)
            mb.Entity<Teacher>()
                .HasOne(t=>t.Course)
                .WithMany(c=>c.Teachers)
                .HasForeignKey(t=>t.CourseId);

            #endregion

        }
    }
}
