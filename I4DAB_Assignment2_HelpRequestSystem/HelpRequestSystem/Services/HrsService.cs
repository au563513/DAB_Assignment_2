using System;
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
                        EnrollStudent(123456, 1, true, 3);
                        EnrollStudent(123457, 1, true, 3);
                        EnrollStudent(123458,1,true,3);
                        EnrollStudent(123459,1,true,3);
                        EnrollStudent(123460,1,true,3);
                        EnrollStudent(123411,1,true,4);
                        EnrollStudent(123412,1,true,4);

                        CreateAssignmentHelpRequest(123456, 2, "State-machines 101");
                        CreateAssignmentHelpRequest(123457, 2, "State-machines 101");
                        CreateAssignmentHelpRequest(123412, 1, "DAB Assigment 2");
                        CreateAssignmentHelpRequest(123412, 1, "DAB Assigment 2");

                        CreateExerciseHelpRequest(123459, 1, "7.1 EF Core", "Discord lokale 3", 1);
                        CreateExerciseHelpRequest(123412, 1, "7.2 EF Core - Query + Manipulation", "Discord lokale 4", 1);
                        CreateExerciseHelpRequest(123457, 1, "7.2 EF Core - Query + Manipulation", "Discord lokale 2", 1);
                        CreateExerciseHelpRequest(123458, 1, "7.2 EF Core - Query + Manipulation", "I baghaven", 1);

                        AddTeacherToAssignment(1, 3); // State-machines 101 + Bente 'UML' Hansen
                        AddTeacherToAssignment(2, 1); // DAB Assigment 2 + DAB manden
                        
                        AddTeacherToExercise("7.1 EF Core", 1, 2); // 7.1 EF Core + Hans Kristian
                        AddTeacherToExercise("7.2 EF Core - Query + Manipulation", 1, 1); // 7.2 EF Core - Query + Manipulation + DAB manden

                        CloseAssignmentHelpRequest(1);
                        CloseExerciseHelpRequest("7.1 EF Core", 1);

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

        public static void CloseAssignmentHelpRequest(int assignmentId)
        {
            using (var c = new HrsContext())
            {
                var assignment = c.Assignments.Find(assignmentId);
                if (assignment == null) return; // Hvis assignment er lige med null - altså ingen assignment eksisterer med det id - så hopper vi ud af funktionen

                assignment.IsOpen = false;
                c.SaveChanges();
            }
        }

        public static void CloseExerciseHelpRequest(string lecture, int number)
        {
            using (var c = new HrsContext())
            {
                var exercise = c.Exercises.Find(lecture, number);
                if (exercise == null) return; // Hvis exercise er lige med null - altså ingen exercise eksisterer med den primary key - så hopper vi ud af funktionen

                exercise.IsOpen = false;
                c.SaveChanges();
            }
        }

        public static void AddTeacherToAssignment(int assignmentId, int teacherId)
        {
            using (var c = new HrsContext())
            {
                var assignment = c.Assignments.Find(assignmentId);
                var teacher = c.Teachers.Find(teacherId);

                if (assignment == null) return;
                if (teacher == null) return;

                assignment.TeacherId = teacher.TeacherId;

                c.SaveChanges();
            }
        }

        public static void AddTeacherToExercise(string lecture, int number, int teacherId)
        {
            using (var c = new HrsContext())
            {
                var exercises = c.Exercises.Find(lecture, number);
                if (exercises == null) return;

                if (c.Teachers.Any(t => t.TeacherId == teacherId))
                {
                    exercises.TeacherId = teacherId;

                    c.SaveChanges();
                }

            }
        }

        public static void CreateExerciseHelpRequest(int studentId, int courseId, string lecture, string helpWhere, int number)
        {
            using (var c = new HrsContext())
            {
                if (c.Exercises.Find(lecture, number) != null) return;

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

        public static List<Exercise> GetExerciseHelpRequest(int teacherId, int courseId)
        {
            using (var c = new HrsContext())
            {
                var list = c.Exercises.AsNoTracking()
                    .Where(E => E.CourseId == courseId && E.TeacherId == teacherId)
                    .Include(c=>c.Student)
                    .ToList();

                return list;
            }
        }

        public static List<Assignment> GetAssignmentHelpRequest(int teacherId, int courseId)
        {
            using (var c = new HrsContext())
            {
                var list = c.Assignments.AsNoTracking()
                    .Where(A => A.CourseId == courseId && A.TeacherId == teacherId)
                    .Include(a=>a.StudentAssignments)
                    .ThenInclude(sa=>sa.Student)
                    .ToList();

                return list;
            }
        }

        public static bool GetHelpRequestsForStudent(int studentId, out List<Exercise> exercises,
            out List<Assignment> assignments)
        {
            using (var c = new HrsContext())
            {
                if (!c.Students.Any(s => s.StudentId == studentId))
                {
                    exercises = null;
                    assignments = null;
                    return false;
                }

                exercises = c.Exercises.AsNoTracking()
                    .Where(e => e.StudentId == studentId)
                    .Include(e=>e.Teacher)
                    .ToList();

                assignments = c.StudentAssignments.AsNoTracking()
                    .Where(sa=>sa.StudentId == studentId)
                    .Select(sa=>sa.Assignment)
                    .ToList();

                if (!exercises.Any() && !assignments.Any())
                {
                    return false;
                }

                return true;
            }
            
        }

        public static int ShowStatisticsExercises(bool isOpen, int courseId)
        {
            using (var c = new HrsContext())
            {
                var count = c.Exercises.Where(e=>e.CourseId == courseId).Count(e => e.IsOpen == isOpen);

                return count;
            }
        }

        public static int ShowStatisticsAssignment(bool isOpen, int courseId)
        {
            using (var c = new HrsContext())
            {
                var count = c.Assignments.Where(a => a.CourseId == courseId).Count(e => e.IsOpen == isOpen);

                return count;
            }
        }



    }
}