using Microsoft.AspNetCore.Mvc;
using University.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;

namespace University.Controllers
{
  public class MajorsController : Controller
  {
    private readonly UniversityContext _db;

    public MajorsController(UniversityContext db)
    {
      _db = db;
    }

    public ActionResult Index()
    {
      return View(_db.Majors.ToList());
    }
    public ActionResult Create()
    {
      ViewBag.DepartmentId = new SelectList(_db.Departments, "DepartmentId", "DepartmentName");
      return View();
    }

    [HttpPost]
    public ActionResult Create(Major major)
    {
      _db.Majors.Add(major);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
    public ActionResult Edit(int id)
    {
      ViewBag.DepartmentId = new SelectList(_db.Departments, "DepartmentId", "DepartmentName");
      var thisMajor = _db.Majors.FirstOrDefault(major => major.MajorId == id);
      return View(thisMajor);
    }

    [HttpPost]
    public ActionResult Edit(Major major)
    {
      _db.Entry(major).State = EntityState.Modified;
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Details(int id)
    {
      Major thisMajor = _db.Majors
      .Include(major => major.Students)
      .FirstOrDefault(major => major.MajorId == id);
      return View(thisMajor);
    }
    public ActionResult Delete(int id)
    {
      var thisMajor = _db.Majors.FirstOrDefault(major => major.MajorId == id);
      return View(thisMajor);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      var thisMajor = _db.Majors.FirstOrDefault(major => major.MajorId == id);
      _db.Majors.Remove(thisMajor);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
  }
}