using System;
using System.Collections.Generic;
using System.Text;

namespace HelpRequestSystem.Models
{
    public class Teacher
    {
        public int TeacherId { get; set; }
        public string TeacherName { get; set; }

        // Navigational props
        public Course Course { get; set; }
        public int CourseId { get; set; }

        public List<Exercise> Exercises { get; set; }

        public List<Assignment> Assignments { get; set; }

        public override string ToString()
        {
            return $"{TeacherName}";
        }
    }
}
