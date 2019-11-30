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
    public class SalidaEmpleadosController : Controller
    {
        private RecursosHumanosEntities db = new RecursosHumanosEntities();

        // GET: SalidaEmpleados
        public ActionResult Index()
        {
            var salidas = db.Salidas.Include(s => s.Empleados);
            return View(salidas.ToList());
        }

        // GET: SalidaEmpleados/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Salidas salidas = db.Salidas.Find(id);
            if (salidas == null)
            {
                return HttpNotFound();
            }
            return View(salidas);
        }

        // GET: SalidaEmpleados/Create
        public ActionResult Create()
        {
            var consulta = from x in db.Empleados.AsEnumerable() where x.Estatus == "Activo" select ( new { Id_Emp = x.Id_Emp, nombre = string.Format("{0} {1}", x.Nombre, x.Apellido) });

            ViewBag.Empleado = new SelectList(consulta, "Id_Emp", "Nombre");
            return View();
        }

        // POST: SalidaEmpleados/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id_Salida,Empleado,Tipo_Salida,Motivo,Fecha_Salida")] Salidas salidas)
        {
            var consulta = from x in db.Empleados.AsEnumerable() where x.Estatus == "Activo" select (new { Id_Emp = x.Id_Emp, nombre = string.Format("{0} {1}", x.Nombre, x.Apellido) });

            if (ModelState.IsValid)
            {
                db.Salidas.Add(salidas);
                db.SaveChanges();

                var query = (from a in db.Empleados
                             join b in db.Salidas on a.Id_Emp equals b.Empleado
                             where a.Id_Emp == b.Empleado
                             select a).ToList();

                foreach (var item in query)
                {
                    item.Estatus = "Inactivo";
                }

                db.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.Empleado = new SelectList(consulta, "Id_Emp", "Nombre", salidas.Empleado);
            return View(salidas);
        }

        // GET: SalidaEmpleados/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Salidas salidas = db.Salidas.Find(id);
            if (salidas == null)
            {
                return HttpNotFound();
            }
            ViewBag.Empleado = new SelectList(db.Empleados, "Id_Emp", "Nombre", salidas.Empleado);
            return View(salidas);
        }

        // POST: SalidaEmpleados/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id_Salida,Empleado,Tipo_Salida,Motivo,Fecha_Salida")] Salidas salidas)
        {
            if (ModelState.IsValid)
            {
                db.Entry(salidas).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Empleado = new SelectList(db.Empleados, "Id_Emp", "Nombre", salidas.Empleado);
            return View(salidas);
        }

        // GET: SalidaEmpleados/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Salidas salidas = db.Salidas.Find(id);
            if (salidas == null)
            {
                return HttpNotFound();
            }
            return View(salidas);
        }

        // POST: SalidaEmpleados/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Salidas salidas = db.Salidas.Find(id);
            db.Salidas.Remove(salidas);
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
