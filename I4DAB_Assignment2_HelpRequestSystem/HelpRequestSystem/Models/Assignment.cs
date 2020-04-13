using System;
using System.Collections.Generic;
using System.Text;

namespace HelpRequestSystem.Models
{
    public class Assignment
    {
        public int AssignmentId { get; set; }
        public string AssignmentName { get; set; }

        // Navigational props
        public Teacher Teacher { get; set; }
        public int? TeacherId { get; set; }

        public Course Course { get; set; }
        public int CourseId { get; set; }

        public List<StudentAssignment> StudentAssignments { get; set; }
    }
}
