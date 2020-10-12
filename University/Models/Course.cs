using System.Collections.Generic;

namespace University.Models
{
  public class Course
    {
        public Course()
        {
            this.Students = new HashSet<CourseStudent>();
        }

        public int CourseId { get; set; }
        public string Name { get; set; }
        public string Number { get; set; }
        public int DepartmentId { get; set; }

        public virtual Department Department { get; set; }
        public virtual ICollection<CourseStudent> Students { get; set; }
    }
}