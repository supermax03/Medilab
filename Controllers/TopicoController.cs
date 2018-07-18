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
    public class TopicoController : Controller
    {

        MedicinaEntities db;
        public TopicoController()
        {
            db = new MedicinaEntities();

        }
        public ActionResult Index(ResultSet response=null,int page=1,int pageSize=5)
        {
            if (page <= 0)
            {
                page = 1;
            }
            if (pageSize <= 0)
            {
                pageSize = 5;
            }
            ViewBag.Operacion = response;           
            int totalRecord = db.Topico.Count();
            ViewBag.dbcount = (totalRecord / pageSize) + ((totalRecord % pageSize) > 0 ? 1 : 0);
            var topicos = db.Topico.OrderBy(s => s.Id).Skip((page - 1) * pageSize).Take(pageSize).ToList();        
            return View(topicos.AsEnumerable());
        }
     
        public JsonResult BuscarTopico(string term)
        {
           
            var resultado = db.Topico.Where(s => s.Nombre.Contains(term))
                .Select(s => new { value = s.Nombre, Id = s.Id }).Take(5).ToList();
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Create()
        {
            return View();
        }
        private bool topicoenuso(string topico,int id=0)
        {
            var item = db.Topico.Where(s => s.Nombre.Equals(topico) && (!s.Id.Equals(id))).FirstOrDefault();
            return (item != null);

        }


        [HttpPost]
        public ActionResult Create(FormCollection collection)
        { try
            {
                ResultSet response = new ResultSet();

                if (!topicoenuso(collection["Nombre"].Trim()))
                {
                    Topico topico = new Topico()
                    {
                        Nombre = collection["Nombre"].Trim(),
                        Descripcion = collection["Descripcion"].Trim()


                    };

                    db.Topico.Add(topico);
                    db.SaveChanges();
                    response.Code = 1;
                    response.Msg = String.Format("Se creó el topico {0}", topico.Nombre);
                }        
                else
                {
                    response.Code = -1;
                    response.Msg = String.Format("El tópico {0} ya existe", collection["Nombre"].Trim());

                }
                return RedirectToAction("Index", new RouteValueDictionary(response));

            }
            catch(Exception exc)
            {
                return View();
            }            
        }
        public ActionResult Edit(int id)
        {
            Topico topico = db.Topico.Where(s => s.Id.Equals(id)).FirstOrDefault();
            if (topico != null)
            {
                return View(topico);
            }
            else
            {
                ResultSet response = new ResultSet();
                response.Code = -1;
                response.Msg = String.Format("El tópico seleccionado ya no existe");
                return RedirectToAction("Index", new RouteValueDictionary(response));
            }
        }
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                ResultSet response = new ResultSet();
                Topico topico = db.Topico.Where(s => s.Id.Equals(id)).FirstOrDefault();
                if (topico != null)
                {
                    topico.Nombre = collection["Nombre"].Trim();
                    topico.Descripcion = collection["Descripcion"].Trim();
                    db.SaveChanges();
                    response.Code = 1;
                    response.Msg = String.Format("Se editó el topico {0}", topico.Nombre);
                    return RedirectToAction("Index", new RouteValueDictionary(response));
                }
                else
                {                   
                    response.Code = -1;
                    response.Msg = String.Format("El tópico seleccionado ya no existe");
                    return RedirectToAction("Index", new RouteValueDictionary(response));

                }
            }
            catch
            {
                return View();
            }
        }
        public ActionResult Details(int id)
        {
            Topico topico = db.Topico.Where(s => s.Id.Equals(id)).FirstOrDefault();
            if (topico != null)
            {
                return View(topico);
            }
            else
            {
                ResultSet response = new ResultSet();
                response.Code = -1;
                response.Msg = String.Format("El tópico seleccionado ya no existe");
                return RedirectToAction("Index", new RouteValueDictionary(response));

            }
        }
        public ActionResult Delete(int id)
        {
            try
            {
                ResultSet response = new ResultSet();
                Topico topico = db.Topico.Where(s => s.Id.Equals(id)).FirstOrDefault();
                if (topico != null)
                {
                    db.Topico.Remove(topico);
                    db.SaveChanges();
                    response.Code = 1;
                    response.Msg = String.Format("Se borró el topico {0}", topico.Nombre);
                    return RedirectToAction("Index", new RouteValueDictionary(response));
                }
                else
                {
                    response.Code = -1;
                    response.Msg = String.Format("El tópico seleccionado ya no existe");
                    return RedirectToAction("Index", new RouteValueDictionary(response));
                }
            }
            catch
            {
                return View();
            }
        }     
    }
}