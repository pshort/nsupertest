using System;
using System.Collections.Generic;

namespace SampleEfCoreApp.Data.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string LastName { get; set; }
        public string FirstMidName { get; set; }
        public DateTime EnrollmentDate { get; set; }
        
        public ICollection<Enrollment> Entrollments { get; set; }
    }
}