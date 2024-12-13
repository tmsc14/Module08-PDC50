using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module08.Model
{
    public class Attendance
    {
        public int AttendanceID { get; set; }
        public string StudentID { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
    }
}
