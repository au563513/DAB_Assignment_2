using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using HelpRequestSystem.Data;
using HelpRequestSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace HelpRequestSystem.Services
{
    public static class HrsService
    {
        public static void Seed()
        {
            using (var c = new HrsContext())
            {
                using (var transaction = c.Database.BeginTransaction())
                {
                    try
                    {
                        CreateStudent(new Student() {StudentId = 123456, StudentName = "Anton Nielsen"});
                        CreateStudent(new Student() {StudentId = 123457, StudentName = "Bente Pedersen"});
                        CreateStudent(new Student() {StudentId = 123458, StudentName = "Søstjerne Fiskesen"});
                        CreateStudent(new Student() {StudentId = 123459, StudentName = "Hilda Nielsen"});
                        CreateStudent(new Student() {StudentId = 123460, StudentName = "Bent Sørensen"});
                        CreateStudent(new Student() {StudentId = 123411, StudentName = "Hans Hansen"});
                        CreateStudent(new Student() {StudentId = 123412, StudentName = "Emil Dollas"});

                        CreateCourse("I4Databaser");
                        CreateCourse("I4Software Design");

                        EnrollStudent(123456,2,true,3);
                        EnrollStudent(123457,2,true,3);
                        EnrollStudent(123458,1,true,3);
                        EnrollStudent(123459,1,true,3);
                        EnrollStudent(123460,1,true,3);
                        EnrollStudent(123411,1,true,4);
                        EnrollStudent(123412,1,true,4);

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

        public static void EnrollStudent(int studentId, int courseId, bool active, int semester)
        {
            using (var c = new HrsContext())
            {
                if (c.StudentCourses.Find(studentId, courseId) == null) return;

                var student = c.Students.Find(studentId);
                var course = c.Courses.Find(courseId);
                if (student == null || course == null) return;

                var SA = new StudentCourse()
                {
                    Active = active,
                    Semester = semester,
                    StudentId = student.StudentId,
                    CourseId = course.CourseId
                };
                c.StudentCourses.Add(SA);
                c.SaveChanges();
            }
        }

        public static void CreateHelpRequest(int studentId, bool isAssignment)
        {
            using (var c = new HrsContext())
            {
                var student = c.Students.Find(studentId);
                if (student == null) return;
                


            }
        }

        public static List<Student> GetStudentList()
        {
            using (var c = new HrsContext())
            {
                return c.Students.AsNoTracking().ToList();
            }
        }

        public static void CreateCourse(string courseName)
        {
            using (var c = new HrsContext())
            {
                if (c.Courses.Any(c => c.Name == courseName)) return;

                c.Add(new Course() { Name = courseName });
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