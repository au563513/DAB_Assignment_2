using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using HelpRequestSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace HelpRequestSystem.Data
{
    class HrsContext : DbContext
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
            mb.Entity<Student>().HasKey(student => student.StudentId);
            mb.Entity<Teacher>().HasKey(teacher => teacher.TeacherId);
            mb.Entity<Course>().HasKey(course => course.CourseId);
            mb.Entity<Assignment>().HasKey(assignment => assignment.AssignmentId);
            mb.Entity<Exercise>().HasKey(exercise => new {exercise.Lecture, exercise.Number});
        }
    }
}
