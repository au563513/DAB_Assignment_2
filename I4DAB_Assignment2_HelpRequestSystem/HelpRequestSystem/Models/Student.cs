using System;
using System.Collections.Generic;
using System.Text;

namespace HelpRequestSystem.Models
{
    public class Student
    {
        public int StudentId { get; set; }
        public string StudentName  { get; set; }

        //Navigational props
        public List<Exercise> Exercises { get; set; }
        public List<StudentAssignment> StudentAssignments { get; set; }
        public List<StudentCourse> StudentCourses { get; set; }
    }
}
