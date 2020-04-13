﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
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

                        CreateAssignmentHelpRequest(123456, 2, "State-machines 101");
                        CreateAssignmentHelpRequest(123412, 1, "DAB Assigment 2");
                        CreateExerciseHelpRequest(123459, 1, "7.1 EF Core", "Discord lokale 3", 1);
                        CreateExerciseHelpRequest(123412, 1, "7.2 EF Core - Query + Manipulation", "Discord lokale 4", 1);

                        AddTeacherToAssignment(1, 3); // State-machines 101 + Bente 'UML' Hansen
                        AddTeacherToAssignment(2, 1); // DAB Assigment 2 + DAB manden
                        AddTeacherToExercise("7.1 EF Core", 1, 2); // 7.1 EF Core + Hans Kristian
                        AddTeacherToExercise("7.2 EF Core - Query + Manipulation", 1, 1); // 7.2 EF Core - Query + Manipulation + DAB manden

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

        public static void AddTeacherToAssignment(int assignmentId, int teacherId)
        {
            using (var c = new HrsContext())
            {
                if (c.Assignments.Find(assignmentId) != null) return;
                if (c.Teachers.Find(teacherId) != null) return;

                var assignment = c.Assignments.Find(assignmentId);
                assignment.TeacherId = assignmentId;

                c.SaveChanges();
            }
        }

        public static void AddTeacherToExercise(string lecture, int number, int teacherId)
        {
            using (var c = new HrsContext())
            {
                var exercises = c.Exercises.Find(lecture, number);

                if (exercises == null) return;
                if (c.Teachers.Find(teacherId) == null) return;

                exercises.TeacherId = teacherId;

                c.SaveChanges();
            }
        }


        public static void CreateExerciseHelpRequest(int studentId, int courseId, string lecture, string helpWhere, int number)
        {
            using (var c = new HrsContext())
            {
                if (c.Exercises.Find(lecture, number) == null) return;

                var student = c.Students.Find(studentId);
                if (student == null) return;

                var course = c.Courses.Find(courseId);
                if (course == null) return;

                var exercise = new Exercise()
                {
                    TeacherId = null,
                    Lecture = lecture,
                    HelpWhere = helpWhere,
                    StudentId = studentId,
                    CourseId = courseId,
                    Number = number
                };

                c.Add(exercise);
                c.SaveChanges();
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
                    Teacher = null,
                    AssignmentName = assignmentName,
                    CourseId = courseId
                };
                c.Add(assignment);
                c.SaveChanges();

                var helpRequest = new StudentAssignment()
                {
                    StudentId = studentId,
                    AssignmentId = assignment.AssignmentId,
                };

                c.Add(helpRequest);
                c.SaveChanges();
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