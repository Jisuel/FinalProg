using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FinalProg.Models;

namespace FinalProg.Controllers
{
    public class VacacionesEmpleadosController : Controller
    {
        private RecursosHumanosEntities db = new RecursosHumanosEntities();

        // GET: VacacionesEmpleados
        public ActionResult Index()
        {
            var vacaciones = db.Vacaciones.Include(v => v.Empleados);
            return View(vacaciones.ToList());
        }

        // GET: VacacionesEmpleados/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vacaciones vacaciones = db.Vacaciones.Find(id);
            if (vacaciones == null)
            {
                return HttpNotFound();
            }
            return View(vacaciones);
        }

        // GET: VacacionesEmpleados/Create
        public ActionResult Create()
        {
            var NombreApellido = db.Empleados.AsEnumerable().Select(a => new { Id_Emp = a.Id_Emp, nombre = string.Format("{0} {1}", a.Nombre, a.Apellido) });
            ViewBag.Empleado = new SelectList(NombreApellido, "Id_Emp", "Nombre");
            return View();
        }

        // POST: VacacionesEmpleados/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id_Vacaciones,Empleado,Desde,Hasta,Comentario")] Vacaciones vacaciones)
        {
            var NombreApellido = db.Empleados.AsEnumerable().Select(a => new { Id_Emp = a.Id_Emp, nombre = string.Format("{0} {1}", a.Nombre, a.Apellido) });

            if (ModelState.IsValid)
            {
                db.Vacaciones.Add(vacaciones);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Empleado = new SelectList(NombreApellido, "Id_Emp", "Nombre", vacaciones.Empleado);
            return View(vacaciones);
        }

        // GET: VacacionesEmpleados/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vacaciones vacaciones = db.Vacaciones.Find(id);
            if (vacaciones == null)
            {
                return HttpNotFound();
            }
            ViewBag.Empleado = new SelectList(db.Empleados, "Id_Emp", "Nombre", vacaciones.Empleado);
            return View(vacaciones);
        }

        // POST: VacacionesEmpleados/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id_Vacaciones,Empleado,Desde,Hasta,Comentario")] Vacaciones vacaciones)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vacaciones).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Empleado = new SelectList(db.Empleados, "Id_Emp", "Nombre", vacaciones.Empleado);
            return View(vacaciones);
        }

        // GET: VacacionesEmpleados/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vacaciones vacaciones = db.Vacaciones.Find(id);
            if (vacaciones == null)
            {
                return HttpNotFound();
            }
            return View(vacaciones);
        }

        // POST: VacacionesEmpleados/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Vacaciones vacaciones = db.Vacaciones.Find(id);
            db.Vacaciones.Remove(vacaciones);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
