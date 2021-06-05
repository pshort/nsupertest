using System.Collections.Generic;

namespace SampleEfCoreApp.Data.Models
{
    public class Course
    {
        public int CourseId { get; set; }
        public string Title { get; set; }
        public int Credits { get; set; }
        
        public ICollection<Enrollment> Enrollments { get; set; }
    }
}