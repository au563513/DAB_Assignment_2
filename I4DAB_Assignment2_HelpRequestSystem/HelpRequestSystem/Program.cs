using System;

using HelpRequestSystem.Services;
using HelpRequestSystem.Data;
using HelpRequestSystem.Models;

namespace HelpRequestSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var c = new HrsContext())
            {
                Student student = new Student();
                student.StudentId = 563513;
                student.StudentName = "Anders Tøgersen";

                c.Students.Add(student);

                c.SaveChanges();
            }
            
        }
    }
}
