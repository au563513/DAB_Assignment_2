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

            HrsService.Seed();

            foreach (var student in HrsService.GetStudentList())
            {
                Console.WriteLine(student);
            }

            foreach (var course in HrsService.GetCourseList())
            {
                Console.WriteLine(course);
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
