using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Text.Json;

namespace Module08.Model
{
    public class Student
    {
        public string StudentID { get; set; }
        public string FullName { get; set; }
        public string GradeClass { get; set; }
        public string ContactNo { get; set; }
        public string DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string EmergencyContact { get; set; }
        public string Status { get; set; }
    }
}
