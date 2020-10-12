using Microsoft.AspNetCore.Mvc;
using University.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;

namespace University.Controllers
{
  public class CoursesController : Controller
  {
    private readonly UniversityContext _db;

    public CoursesController(UniversityContext db)
    {
      _db = db;
    }

    public ActionResult Index()
    {
      return View(_db.Courses.ToList());
    }

public ActionResult Create()
{
    ViewBag.CourseId = new SelectList(_db.Courses, "CourseId", "Name");
    ViewBag.DepartmentId = new SelectList(_db.Departments, "DepartmentId", "DepartmentName");
    return View();
}

[HttpPost]
public ActionResult Create(Course course, int StudentId)
{
    _db.Courses.Add(course);
    if (StudentId != 0)
    {
        _db.CourseStudent.Add(new CourseStudent() { StudentId = StudentId, CourseId = course.CourseId });
    }
    _db.SaveChanges();
    return RedirectToAction("Index");
}

    public ActionResult Details(int id)
    {
        ViewBag.Completed = _db.CourseStudent.FirstOrDefault(courseStudent => courseStudent.CourseStudentId == id);;
        var thisCourse = _db.Courses
        .Include(course => course.Students)
        .ThenInclude(join => join.Student)
        .FirstOrDefault(course => course.CourseId == id);
        return View(thisCourse);
    }

    public ActionResult Edit(int id)
    {
        var thisCourse = _db.Courses.FirstOrDefault(courses => courses.CourseId == id);
        // ViewBag.StudentId = new SelectList(_db.Students, "StudentId", "Name");
        ViewBag.DepartmentId = new SelectList(_db.Departments, "DepartmentId", "DepartmentName");
        return View(thisCourse);
    }

    [HttpPost]
    public ActionResult Edit(Course course, int StudentId)
    {
      if (StudentId != 0)
      {
        _db.CourseStudent.Add(new CourseStudent() { StudentId = StudentId, CourseId = course.CourseId });
      }
      _db.Entry(course).State = EntityState.Modified;
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult AddStudent(int id)
    {
        var thisCourse = _db.Courses.FirstOrDefault(courses => courses.CourseId == id);
        ViewBag.StudentId = new SelectList(_db.Students, "StudentId", "Name");
        return View(thisCourse);
    }

    [HttpPost]  
    public ActionResult AddStudent(Course course, int StudentId)
    {
        if (StudentId != 0)
        {
        _db.CourseStudent.Add(new CourseStudent() { StudentId = StudentId, CourseId = course.CourseId, Completed = false});
        }
        _db.SaveChanges();
        return RedirectToAction("Index");
    }

    public ActionResult Delete(int id)
    {
        var thisCourse = _db.Courses.FirstOrDefault(courses => courses.CourseId == id);
        return View(thisCourse);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
        var thisCourse = _db.Courses.FirstOrDefault(courses => courses.CourseId == id);
        _db.Courses.Remove(thisCourse);
        _db.SaveChanges();
        return RedirectToAction("Index");
    }

    [HttpPost]
    public ActionResult DeleteStudent(int joinId)
    {
        var joinEntry = _db.CourseStudent.FirstOrDefault(entry => entry.CourseStudentId == joinId);
        _db.CourseStudent.Remove(joinEntry);
        _db.SaveChanges();
        return RedirectToAction("Index");
    }
    public ActionResult Completed(int joinId)
    {
        var joinEntry = _db.CourseStudent.FirstOrDefault(entry => entry.CourseStudentId == joinId);
        joinEntry.Completed = true;
        _db.Entry(joinEntry).State = EntityState.Modified;
        _db.SaveChanges();
        return RedirectToAction("Details", new { id = joinEntry.CourseId });
    }
  }
}