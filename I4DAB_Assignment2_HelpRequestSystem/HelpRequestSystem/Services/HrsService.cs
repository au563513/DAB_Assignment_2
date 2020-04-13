using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
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

                        CreateTeacher("DAB manden", 1);
                        CreateTeacher("Hans Kristian", 1);
                        CreateTeacher("Bente 'UML' Hansen",2);

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
                        throw;
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
                if (c.StudentCourses.Find(studentId, courseId) != null) return;

                var student = c.Students.Find(studentId);
                var course = c.Courses.Find(courseId);
                if (student == null || course == null) return;

                var studentCourse = new StudentCourse()
                {
                    Active = active,
                    Semester = semester,
                    StudentId = student.StudentId,
                    CourseId = course.CourseId
                };
                c.Add(studentCourse);
                c.SaveChanges();
            }
        }

        public static void CreateExerciseHelpRequest(int studentId, int courseId, string lecture, string helpWhere)
        {
            using (var c = new HrsContext())
            {
                var student = c.Students.Find(studentId);
                if (student == null) return;

                var course = c.Courses.Find(courseId);
                if (course == null) return;

                var exercise = new Exercise()
                {
                    Lecture = lecture,
                    HelpWhere = helpWhere,
                    StudentId = studentId,
                    Student = student,
                    CourseId = courseId,
                    Course = course
                };
                
                course.Exercises.Add(exercise);
                student.Exercises.Add(exercise);
            }
        }

        public static void CreateAssignmentHelpRequest(int studentId, int courseId, string assignmentName)
        {
            using (var c = new HrsContext())
            {
                var student = c.Students.Find(studentId);
                if (student == null) return;

                var course = c.Courses.Find(courseId);
                if (course == null) return;

                var assignment = new Assignment()
                {
                    AssignmentName = assignmentName,
                    CourseId = courseId,
                    Course = course
                };

                var helpRequest = new StudentAssignment()
                {
                    StudentId = studentId,
                    Student = student,
                    AssignmentId = assignment.AssignmentId,
                    Assignment = assignment
                };

                assignment.StudentAssignments.Add(helpRequest);
                student.StudentAssignments.Add(helpRequest);
                course.Assignments.Add(assignment);
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
                var List = c.Courses.AsNoTracking()
                    .Include(c => c.Teachers)
                    .Include(c=> c.StudentCourses)
                        .ThenInclude(sc=>sc.Student)
                    .ToList();

                return List;
            }
        }

        public static void CreateTeacher(string name, int courseId)
        {
            using (var c = new HrsContext())
            {
                if (c.Teachers.Any(t=>t.TeacherName == name)) return;
                if (!c.Courses.Any(c=>c.CourseId == courseId)) return;

                c.Add(new Teacher() { TeacherName = name, CourseId = courseId});
                c.SaveChanges();
            }
        }

    }
}