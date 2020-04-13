using System;
using System.Linq;
using HelpRequestSystem.Services;
using HelpRequestSystem.Data;
using HelpRequestSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace HelpRequestSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            //ClearDatabase();
            //SeedDatabase();

            using (var c = new HrsContext())
            {
                var list = c.Students.AsNoTracking().ToList();
                
                foreach (var student in list)
                {
                    Console.WriteLine($"au{student.StudentId}, {student.StudentName}");
                }
            }

        }

        public static void ClearDatabase()
        {
            using (var c = new HrsContext())
            {
                c.Database.EnsureDeleted();
                c.Database.Migrate();
            }
        }

        public static void SeedDatabase()
        {
            using (var c = new HrsContext())
            {
                c.Database.EnsureCreated();
                c.Students.Add(CreateStudent(123456,"Anders"));
                c.Students.Add(CreateStudent(123455,"Peter"));
                c.Students.Add(CreateStudent(132213,"Julie"));
                c.Students.Add(CreateStudent(634643,"Valdemar"));
                c.Students.Add(CreateStudent(233253,"Jonas"));
                c.SaveChanges();
            }
        }

        private static Student CreateStudent(int id, string name)
        {
            return new Student(){StudentId = id, StudentName = name};
        }
    }
}
