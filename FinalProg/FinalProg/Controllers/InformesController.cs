using FinalProg.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FinalProg.Controllers
{
    public class InformesController : Controller
    {
        // GET: Informes
        public ActionResult Index()
        {
            return View();
        }

        private RecursosHumanosEntities db = new RecursosHumanosEntities();

        public ActionResult Nomina()
        {
            var consulta = from x in db.Nomina select x;

            return View(consulta.ToList());
        }

        [HttpPost]
        public ActionResult Nomina(int buscar)
        {

            var lista = from x in db.Nomina select x;

            if (buscar == 0)
            {
                return View(lista.ToList());
            }
            else
            {
                lista = lista.Where(a => a.Fecha.Year.Equals(buscar) || a.Fecha.Month.Equals(buscar));
                return View(lista);
            }
        }

        // GET: Activos
        public ActionResult Activos()
        {
            var consulta = from x in db.Empleados where x.Estatus == "Activo" select x;

            return View(consulta.ToList());
        }

        [HttpPost]
        public ActionResult Activos(string buscar)
        {
            var lista = from x in db.Empleados
                        join z in db.Departamentos
                        on x.Departamento_Id equals z.Id_Dep
                        where x.Estatus == "Activo"
                        select x;

            if (string.IsNullOrEmpty(buscar))
            {
                return View(lista.ToList());
            }
            else
            {
                lista = lista.Where(a => a.Nombre.Contains(buscar) || a.Departamento_Id.ToString().Equals(buscar));
                return View(lista);
            }
        }

        public ActionResult Inactivos()
        {
            var consulta = from x in db.Empleados where x.Estatus == "Inactivo" select x;

            return View(consulta.ToList());
        }

        public ActionResult Departamentos()
        {
            var consulta = from x in db.Departamentos select x;

            return View(consulta.ToList());

        }

        public ActionResult Cargos()
        {
            var consulta = from x in db.Cargos select x;

            return View(consulta.ToList());
        }

        public ActionResult Entradas()
        {
            var consulta = from x in db.Empleados select x;

            return View(consulta.ToList());
        }

        [HttpPost]
        public ActionResult Entradas(int buscar)
        {

            var lista = from x in db.Empleados select x;

            if (buscar == 0)
            {
                return View(lista.ToList());
            }
            else
            {
                lista = lista.Where(a => a.Fecha_Ingreso.Month.Equals(buscar));
                return View(lista);
            }
        }

        public ActionResult Salidas()
        {
            var consulta = from x in db.Salidas select x;

            return View(consulta.ToList());
        }

        [HttpPost]
        public ActionResult Salidas(int buscar)
        {

            var lista = from x in db.Salidas select x;

            if (buscar == 0)
            {
                return View(lista.ToList());
            }
            else
            {
                lista = lista.Where(a => a.Fecha_Salida.Month.Equals(buscar));
                return View(lista);
            }
        }

        public ActionResult Permisos()
        {
            var consulta = from x in db.Empleados
                           join z in db.Permisos
                           on x.Id_Emp equals z.Empleado
                           where x.Id_Emp == z.Empleado
                           select z;

            return View(consulta.ToList());
        }

        [HttpPost]
        public ActionResult Permisos(string buscar)
        {
            var lista = from x in db.Permisos select x;

            if (string.IsNullOrEmpty(buscar))
            {
                return View(lista.ToList());
            }
            else
            {
                lista = lista.Where(a => a.Empleados.Nombre.Contains(buscar) || a.Empleados.Codigo_Emp.Equals(buscar));
                return View(lista);
            }
        }
    }
}