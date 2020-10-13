using System.Collections.Generic;

namespace University.Models
{
  public class Major
    {
public Major()
    {
        this.Students = new HashSet<Student>();
        this.Departments = new HashSet<Department>();
    }

    public int MajorId { get; set; }
    public string MajorName { get; set; }
    public int DepartmentId { get; set; }
    public virtual ICollection<Student> Students { get; set; }
    public virtual ICollection<Department> Departments { get; set; }
  }
}