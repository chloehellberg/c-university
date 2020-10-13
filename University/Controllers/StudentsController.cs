using Microsoft.AspNetCore.Mvc;
using University.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;

namespace University.Controllers
{
  public class StudentsController : Controller
  {
  private readonly UniversityContext _db;

    public StudentsController(UniversityContext db)
    {
      _db = db;
    }

    public ActionResult Index()
    {
      List<Student> model = _db.Students.ToList();
        return View(model);
    }
    public ActionResult Create()
    {
      ViewBag.MajorId = new SelectList(_db.Majors, "MajorId", "MajorName");
      return View();
    }
    [HttpPost]
    public ActionResult Create(Student student)
    {
      Major majorRow = _db.Majors.FirstOrDefault(majors => majors.MajorId == student.MajorId);
      student.DepartmentId = majorRow.DepartmentId;
      _db.Students.Add(student);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
    public ActionResult Details(int id)
    {
      var thisStudent = _db.Students
        .Include(students => students.Courses)
        .ThenInclude(join => join.Course)
        .FirstOrDefault(student => student.StudentId == id);
        return View(thisStudent);
    }
    public ActionResult Edit(int id)
    {
      var thisStudent = _db.Students.FirstOrDefault(students => students.StudentId == id);
      ViewBag.MajorId = new SelectList(_db.Majors, "MajorId", "MajorName");
      return View(thisStudent);
    }
    [HttpPost]
    public ActionResult Edit(Student student)
    {
      Major majorRow = _db.Majors.FirstOrDefault(majors => majors.MajorId == student.MajorId);
      student.DepartmentId = majorRow.DepartmentId;
      _db.Entry(student).State = EntityState.Modified;
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
    public ActionResult Delete(int id)
    {
        var thisStudent = _db.Students.FirstOrDefault(students => students.StudentId == id);
        return View(thisStudent);
    }
    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
        var thisStudent = _db.Students.FirstOrDefault(students => students.StudentId == id);
        _db.Students.Remove(thisStudent);
        _db.SaveChanges();
        return RedirectToAction("Index");
    }
    public ActionResult Completed(int joinId)
    {
        var joinEntry = _db.CourseStudent.FirstOrDefault(entry => entry.CourseStudentId == joinId);
        joinEntry.Completed = true;
        _db.Entry(joinEntry).State = EntityState.Modified;
        _db.SaveChanges();
        return RedirectToAction("Details", new { id = joinEntry.StudentId });
    }
  }
}
