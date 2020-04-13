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

            StudentService.Seed();

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
    }
}
