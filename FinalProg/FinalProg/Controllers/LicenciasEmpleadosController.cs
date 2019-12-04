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
    public class LicenciasEmpleadosController : Controller
    {
        private RecursosHumanosEntities db = new RecursosHumanosEntities();

        // GET: LicenciasEmpleados
        public ActionResult Index()
        {
            var licencias = db.Licencias.Include(l => l.Empleados);
            return View(licencias.ToList());
        }

        // GET: LicenciasEmpleados/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Licencias licencias = db.Licencias.Find(id);
            if (licencias == null)
            {
                return HttpNotFound();
            }
            return View(licencias);
        }

        // GET: LicenciasEmpleados/Create
        public ActionResult Create()
        {
            var consulta = from x in db.Empleados.AsEnumerable() where x.Estatus == "Activo" select (new { Id_Emp = x.Id_Emp, nombre = string.Format("{0} {1}", x.Nombre, x.Apellido) });

            ViewBag.Empleado = new SelectList(consulta, "Id_Emp", "Nombre");
            return View();
        }

        // POST: LicenciasEmpleados/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id_Licencias,Empleado,Desde,Hasta,Motivo,Comentarios")] Licencias licencias)
        {
            var consulta = from x in db.Empleados.AsEnumerable() where x.Estatus == "Activo" select (new { Id_Emp = x.Id_Emp, nombre = string.Format("{0} {1}", x.Nombre, x.Apellido) });

            if (ModelState.IsValid)
            {
                db.Licencias.Add(licencias);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Empleado = new SelectList(consulta, "Id_Emp", "Nombre", licencias.Empleado);
            return View(licencias);
        }

        // GET: LicenciasEmpleados/Edit/5
        public ActionResult Edit(int? id)
        {
            var consulta = from x in db.Empleados.AsEnumerable() select (new { Id_Emp = x.Id_Emp, nombre = string.Format("{0} {1}", x.Nombre, x.Apellido) });
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Licencias licencias = db.Licencias.Find(id);
            if (licencias == null)
            {
                return HttpNotFound();
            }
            ViewBag.Empleado = new SelectList(consulta, "Id_Emp", "Nombre", licencias.Empleado);
            return View(licencias);
        }

        // POST: LicenciasEmpleados/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id_Licencias,Empleado,Desde,Hasta,Motivo,Comentarios")] Licencias licencias)
        {
            var consulta = from x in db.Empleados.AsEnumerable() select (new { Id_Emp = x.Id_Emp, nombre = string.Format("{0} {1}", x.Nombre, x.Apellido) });
            if (ModelState.IsValid)
            {
                db.Entry(licencias).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Empleado = new SelectList(consulta, "Id_Emp", "Nombre", licencias.Empleado);
            return View(licencias);
        }

        // GET: LicenciasEmpleados/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Licencias licencias = db.Licencias.Find(id);
            if (licencias == null)
            {
                return HttpNotFound();
            }
            return View(licencias);
        }

        // POST: LicenciasEmpleados/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Licencias licencias = db.Licencias.Find(id);
            db.Licencias.Remove(licencias);
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
