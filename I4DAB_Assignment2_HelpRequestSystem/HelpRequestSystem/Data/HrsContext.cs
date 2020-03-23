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
            ob.UseSqlServer("ConnString?");
        }

        //DBset
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<Exercise> Type { get; set; }

        protected override void OnModelCreating(ModelBuilder mb)
        {
            /*          Setup PK            */
            mb.Entity<Student>().HasKey(student => student.StudentId);
            mb.Entity<Teacher>().HasKey(teacher => teacher.TeacherId);
            mb.Entity<Course>().HasKey(course => course.CourseId);
            mb.Entity<Assignment>().HasKey(assignment => assignment.AssignmentId);
            mb.Entity<Exercise>().HasKey(exercise => new {exercise.Lecture, exercise.Number});

            /*          Setup SQL attribute constraints         */


            /*          Setup FK and relations          */

            //Student - Assignment (Many to Many)
            mb.Entity<StudentAssignment>()
                .HasOne(sa=>sa.Student)
                .WithMany(s=>s.StudentAssignments)
                .HasForeignKey(sa=>sa.StudentId);
            mb.Entity<StudentAssignment>()
                .HasOne(sa=>sa.Assignment)
                .WithMany(s=>s.StudentAssignments)
                .HasForeignKey(sa=>sa.AssigmentId);

            //Student - Course


        }
    }
}
