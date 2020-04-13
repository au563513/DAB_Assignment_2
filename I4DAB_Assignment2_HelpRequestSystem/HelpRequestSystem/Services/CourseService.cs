using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HelpRequestSystem.Data;
using HelpRequestSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace HelpRequestSystem.Services
{
    class CourseService
    {
        public static void CreateCourse(string courseName)
        {
            using (var c = new HrsContext())
            {
                if (c.Courses.Any(c=>c.Name == courseName)) return;

                c.Add(new Course() {Name = courseName});
                c.SaveChanges();
            }
        }

        public static Course FindCourse(int id)
        {
            using (var c = new HrsContext())
            {
                if (c.Courses.Any(c=>c.CourseId == id))
                {
                    return c.Courses.Find(id);
                }
                return null;
            }
        }

        public static List<Course> GetCourseList()
        {
            using (var c = new HrsContext())
            {
                return c.Courses.AsNoTracking().ToList();
            }
        }
    }
}