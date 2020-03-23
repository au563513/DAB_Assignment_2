using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelpRequestSystem.Data;
using HelpRequestSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace HelpRequestSystem.Services
{
    public class StudentServices
    {
        async Task<List<Student>> ShowAllStudents(HrsContext context)
        {
            return await context.Students.AsNoTracking().ToListAsync();
        }

        private async Task CreateNewStudent(HrsContext context, int auId, string name)
        {
            if (context.Students.AsNoTracking().Any(student=>student.StudentId == auId))
            {
                throw new ArgumentException($"Student with auId:{auId} already is in database");
            }

            var s = new Student() {
                StudentId = auId,
                StudentName = name
            };

            context.Students.Add(s);
            await context.SaveChangesAsync();
        }

    }
}
