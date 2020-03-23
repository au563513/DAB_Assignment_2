using System.Collections.Generic;

namespace HelpRequestSystem.Models
{
    public class Course
    {
        public int CourseId { get; set; }
        public string Name { get; set; }
        public List<StudentCourse> StudentCourses { get; set; }
        public List<Assignment> Assignments { get; set; }
        public List<Teacher> Teachers { get; set; }
        public List<Exercise> Exercises { get; set; }
    }
}