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
                c.Add(new Course() {Name = courseName});
                c.SaveChanges();
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