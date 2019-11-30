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
    public class PermisosEmpleadosController : Controller
    {
        private RecursosHumanosEntities db = new RecursosHumanosEntities();

        // GET: PermisosEmpleados
        public ActionResult Index()
        {
            var permisos = db.Permisos.Include(p => p.Empleados);
            return View(permisos.ToList());
        }

        // GET: PermisosEmpleados/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Permisos permisos = db.Permisos.Find(id);
            if (permisos == null)
            {
                return HttpNotFound();
            }
            return View(permisos);
        }

        // GET: PermisosEmpleados/Create
        public ActionResult Create()
        {
            var consulta = from x in db.Empleados.AsEnumerable() where x.Estatus == "Activo" select (new { Id_Emp = x.Id_Emp, nombre = string.Format("{0} {1}", x.Nombre, x.Apellido) });
            ViewBag.Empleado = new SelectList(consulta, "Id_Emp", "Nombre");
            return View();
        }

        // POST: PermisosEmpleados/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id_Permisos,Empleado,Desde,Hasta,Comentarios")] Permisos permisos)
        {
            var consulta = from x in db.Empleados.AsEnumerable() where x.Estatus == "Activo" select (new { Id_Emp = x.Id_Emp, nombre = string.Format("{0} {1}", x.Nombre, x.Apellido) });

            if (ModelState.IsValid)
            {
                db.Permisos.Add(permisos);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Empleado = new SelectList(consulta, "Id_Emp", "Nombre", permisos.Empleado);
            return View(permisos);
        }

        // GET: PermisosEmpleados/Edit/5
        public ActionResult Edit(int? id)
        {
            var NombreApellido = db.Empleados.AsEnumerable().Select(a => new { Id_Emp = a.Id_Emp, nombre = string.Format("{0} {1}", a.Nombre, a.Apellido) });

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Permisos permisos = db.Permisos.Find(id);
            if (permisos == null)
            {
                return HttpNotFound();
            }
            ViewBag.Empleado = new SelectList(NombreApellido, "Id_Emp", "Nombre", permisos.Empleado);
            return View(permisos);
        }

        // POST: PermisosEmpleados/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id_Permisos,Empleado,Desde,Hasta,Comentarios")] Permisos permisos)
        {
            var NombreApellido = db.Empleados.AsEnumerable().Select(a => new { Id_Emp = a.Id_Emp, nombre = string.Format("{0} {1}", a.Nombre, a.Apellido) });

            if (ModelState.IsValid)
            {
                db.Entry(permisos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Empleado = new SelectList(NombreApellido, "Id_Emp", "Nombre", permisos.Empleado);
            return View(permisos);
        }

        // GET: PermisosEmpleados/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Permisos permisos = db.Permisos.Find(id);
            if (permisos == null)
            {
                return HttpNotFound();
            }
            return View(permisos);
        }

        // POST: PermisosEmpleados/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Permisos permisos = db.Permisos.Find(id);
            db.Permisos.Remove(permisos);
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
