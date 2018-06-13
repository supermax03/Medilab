using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MediLab.Models;
using MediLab.Controllers.MyClasses;
using System.Web.Routing;

namespace MediLab.Controllers
{
    public class ArticuloController : Controller
    {
        public ActionResult Index(int topico = -1, int page = 1, int pageSize = 5)
        {
            ViewBag.topico = topico;
            var articulos = GetArticulosPaginados(topico, page, pageSize);
            return View(articulos.AsEnumerable());
        }

        public List<Articulo> GetArticulosPaginados(int topico, int page = 1, int pageSize = 5)
        {
            System.Diagnostics.Debug.WriteLine(String.Format("Ingresando a obtiene articulos {0}", System.DateTime.Now));
            if (page <= 0)
            {
                page = 1;
            }
            if (pageSize <= 0)
            {
                pageSize = 5;
            }

            MedicinaEntities db = new MedicinaEntities();
            int totalRecord = db.Articulo.Where(s => (s.IdTopico.Equals(topico) && !topico.Equals(-1)) || (s.IdTopico.Equals(s.IdTopico) && topico.Equals(-1))).Count();
            ViewBag.dbcount = (totalRecord / pageSize) + ((totalRecord % pageSize) > 0 ? 1 : 0);
            var articulos = db.Articulo.Where(s => (s.IdTopico.Equals(topico) && !topico.Equals(-1)) || (s.IdTopico.Equals(s.IdTopico) && topico.Equals(-1))).OrderBy(s => s.Id).Skip((page - 1) * pageSize).Take(pageSize).ToList();
            return articulos;
        }

        public ActionResult GetArticulo(int codigo)
        {
            MedicinaEntities db = new MedicinaEntities();
            var articulo = db.Articulo.Where(s => s.Id.Equals(codigo)).First();
            return View(articulo);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                /* ResultSet response = new ResultSet();
                 MedicinaEntities db = new MedicinaEntities();
                 Topico topico = new Topico()
                 {
                     Nombre = collection["Nombre"],
                     Descripcion = collection["Descripcion"]


                 };
                 db.Topico.Add(topico);
                 db.SaveChanges();
                 response.Code = 1;
                 response.Msg = String.Format("Se creó el topico {0}", topico.Nombre);
                 return RedirectToAction("Index", new RouteValueDictionary(response));*/
                return RedirectToAction("Index");

            }
            catch
            {
                return View();
            }
        }
        public ActionResult Edit(int id)
        {          
            MedicinaEntities db = new MedicinaEntities();
            Articulo articulo= db.Articulo.Where(s => s.Id.Equals(id)).First();
            return View(articulo);
        }
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                /*  ResultSet response = new ResultSet();
                  MedicinaEntities db = new MedicinaEntities();
                  Topico topico = db.Topico.Where(s => s.Id.Equals(id)).First();
                  topico.Nombre = collection["Nombre"];
                  topico.Descripcion = collection["Descripcion"];
                  db.SaveChanges();
                  response.Code = 1;
                  response.Msg = String.Format("Se editó el topico {0}", topico.Nombre);
                  return RedirectToAction("Index", new RouteValueDictionary(response)); */
                return RedirectToAction("Index");

            }
            catch
            {
                return View();
            }
        }
        public ActionResult Delete(int id)
        {
            return View();
        }
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}