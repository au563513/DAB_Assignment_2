using System;
using System.Collections.Generic;
using System.Text;

namespace HelpRequestSystem.Models
{
    public class Assignment
    {
        public int AssignmentId { get; set; }
        public string AssignmentName { get; set; }

        public Teacher Teacher { get; set; }
        public Course Course { get; set; }

        public List<StudentAssignment> StudentAssignments { get; set; }
    }
}
