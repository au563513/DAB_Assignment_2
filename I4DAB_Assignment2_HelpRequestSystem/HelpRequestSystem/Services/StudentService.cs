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
    public static class StudentService
    {
        public static void Seed()
        {
            using (var c = new HrsContext())
            {
                using (var transaction = c.Database.BeginTransaction())
                {
                    CreateStudent(123456,"Anton Nielsen");
                    CreateStudent(654321,"Benta Raket");
                    CreateStudent(123457, "Damen FraNetto");
                    CreateStudent(654327, "Hilda Bentonio");
                    CreateStudent(123454, "Svend Nielsen");
                    CreateStudent(654324, "Severin Raket");
                    transaction.Commit();
                }
            }
        }

        public static void CreateStudent(int id, string name)
        {
            using (var c = new HrsContext())
            { 
                if (c.Students.Any(s => s.StudentId == id)) return;
            
                var student = new Student() { StudentId = id, StudentName = name };
                c.Add(student);
                c.SaveChanges();
            }
        }
    }
}
