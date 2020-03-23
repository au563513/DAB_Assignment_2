using System;
using System.Collections.Generic;
using System.Text;

namespace HelpRequestSystem.Models
{
    public class Exercise
    {
        public int Number { get; set; }
        public string Lecture { get; set; }
        public string HelpWhere { get; set; }

        //Navigational props
        public Student Student { get; set; }
        public int StudentId { get; set; }

        public Teacher Teacher { get; set; }
        public int TeacherId { get; set; }

        public Course Course { get; set; }
        public int CourseId { get; set; }   
    }
}
