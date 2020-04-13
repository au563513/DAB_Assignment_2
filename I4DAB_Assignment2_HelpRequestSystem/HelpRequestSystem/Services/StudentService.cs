using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
                    try
                    {
                        CreateStudent(new Student(){ StudentId = 123456, StudentName = "Anton Nielsen" });
                        CreateStudent(new Student(){ StudentId = 123457, StudentName = "Bente Pedersen" });
                        CreateStudent(new Student(){ StudentId = 123458, StudentName = "Søstjerne Fiskesen" });
                        CreateStudent(new Student(){ StudentId = 123459, StudentName = "Hilda Nielsen" });
                        CreateStudent(new Student(){ StudentId = 123460, StudentName = "Bent Sørensen" });
                        CreateStudent(new Student(){ StudentId = 123411, StudentName = "Hans Hansen" });
                        CreateStudent(new Student(){ StudentId = 123412, StudentName = "Emil Dollas" });

                        CourseService.CreateCourse("I4Databaser");

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                    }
                }
            }
        }

        public static void CreateStudent(Student student)
        {
            using (var c = new HrsContext())
            { 
                if (c.Students.Any(s => s.StudentId == student.StudentId)) return;
                
                c.Add(student);
                c.SaveChanges();
            }
        }

        public static void EnrollStudent(Student student, Course course)
        {
            using (var c = new HrsContext())
            {
                using (var transaction = c.Database.BeginTransaction())
                {
                    


                }
            }
        }

        public static Student FindStudent(int auId)
        {
            using (var c = new HrsContext())
            {
                if (c.Students.Any(s => s.StudentId == auId)) return null;

                return c.Students.Find(auId);
            }
        }

        public static List<Student> GetStudentList()
        {
            using (var c = new HrsContext())
            {
                return c.Students.AsNoTracking().ToList();
            }
        }
    }
}
