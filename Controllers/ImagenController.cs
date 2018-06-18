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
    public class ImagenController : Controller
    {

        MedicinaEntities db;
        public ImagenController()
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
            int totalRecord = db.Imagen.Count();
            ViewBag.dbcount= (totalRecord / pageSize) + ((totalRecord % pageSize) > 0 ? 1 : 0);
            var imagenes = db.Imagen.OrderBy(s => s.Id).Skip((page - 1) * pageSize).Take(pageSize).ToList();
            return View(imagenes.AsEnumerable());        
        }
     
      

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        { try
            {
                ResultSet response = new ResultSet();
                Imagen imagen = new Imagen()
                {
                    Titulo = collection["Titulo"].Trim(),
                    Comentarios = collection["Comentarios"].Trim(),
                    Path = collection["Path"].Trim(),
                    IdArticulo=Convert.ToInt32(collection["IdArticulo"])
                };
                db.Imagen.Add(imagen);
                db.SaveChanges();
                response.Code = 1;
                response.Msg = String.Format("Se creó la imagen {0}", imagen.Titulo);             
                return RedirectToAction("Index", new RouteValueDictionary(response));

            }
            catch(Exception ex)
            {
                return View();
            }            
        }
        public ActionResult Edit(int id)
        {
            
            Imagen imagen = db.Imagen.Where(s => s.Id.Equals(id)).First();            
            return View(imagen);
        }
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {                
                ResultSet response = new ResultSet();
                Imagen imagen = db.Imagen.Where(s => s.Id.Equals(id)).First();
                imagen.Titulo = collection["Titulo"].Trim();
                imagen.Comentarios = collection["Comentarios"].Trim();
                imagen.Path = collection["Path"].Trim();
                imagen.IdArticulo = Convert.ToInt32(collection["IdArticulo"]);
                db.SaveChanges();
                response.Code = 1;
                response.Msg = String.Format("Se editó la imagen {0}", imagen.Titulo);           
                return RedirectToAction("Index", new RouteValueDictionary(response));
                
            }
            catch(Exception ex)
            {
                return View();
            }
        }
        public ActionResult Details(int id)
        {
            MedicinaEntities db = new MedicinaEntities();
            Topico topico = db.Topico.Where(s => s.Id.Equals(id)).First();
            return View(topico);
        }
        public ActionResult Delete(int id)
        {
            try
            {
                ResultSet response = new ResultSet();
                Imagen imagen = db.Imagen.Where(s => s.Id.Equals(id)).First();
                db.Imagen.Remove(imagen);
                db.SaveChanges();
                response.Code = 1;
                response.Msg = String.Format("Se borró la imagen {0}", imagen.Titulo);
                return RedirectToAction("Index", new RouteValueDictionary(response));
            }
            catch
            {
                return View();
            }
        }      
    }
}