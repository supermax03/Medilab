using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MediLab.Models;
using MediLab.Controllers.MyClasses;
using System.Web.Routing;
using System.IO;

namespace MediLab.Controllers
{
    public class ImagenController : Controller
    {

        MedicinaEntities db;
        public ImagenController()
        {
            db = new MedicinaEntities();

        }
        public ActionResult Index(ResultSet response=null,int page=1,int pageSize=6)
        {
           
           if (page <= 0)
            {
                page = 1;
            }
            if (pageSize <= 0)
            {
                pageSize = 6;
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
        public ActionResult Create(HttpPostedFileBase file,FormCollection collection)
        { try
            {
                if (file != null && file.ContentLength > 0)
                {
                    string path = Path.Combine(Server.MapPath("~/Imagenes"), Path.GetFileName(file.FileName));
                    if (!System.IO.File.Exists(path))
                    {
                        file.SaveAs(path); //Guardo el archivo en el server
                    }
                    ResultSet response = new ResultSet();
                    Imagen imagen = new Imagen()
                    {
                        Titulo = collection["Titulo"].Trim(),
                        Comentarios = collection["Comentarios"].Trim(),
                        Path = String.Format("~/Imagenes/{0}",file.FileName),
                        IdArticulo = Convert.ToInt32(collection["IdArticulo"])
                    };
                    db.Imagen.Add(imagen);
                    db.SaveChanges();
                    response.Code = 1;
                    response.Msg = String.Format("Se creó la imagen {0}", imagen.Titulo);
                    return RedirectToAction("Index", new RouteValueDictionary(response));
                }
                else return View();
            }
            catch(Exception ex)
            {
                return View();
            }            
        }
        public ActionResult Edit(int id)
        {
          

                Imagen imagen = db.Imagen.Where(s => s.Id.Equals(id)).FirstOrDefault();
                if (imagen != null)
                {
                    return View(imagen);
                }
                else
                {
                    ResultSet response = new ResultSet();
                    response.Code = -1;
                    response.Msg = String.Format("La imagen seleccionada ya no existe");
                    return RedirectToAction("Index", new RouteValueDictionary(response));
                }
            
           
        }
        [HttpPost]
        public ActionResult Edit(HttpPostedFileBase file,int id, FormCollection collection)
        {
            try
            {
                ResultSet response = new ResultSet();
                Imagen imagen = db.Imagen.Where(s => s.Id.Equals(id)).FirstOrDefault();
                if (imagen != null)
                {
                    if (file != null && file.ContentLength > 0)
                    {
                        //Borramos el archivo anterior y damos de alta el nuevo archivo siempre y cuando el archivo no este en uso
                        String newpath = String.Format("~/Imagenes/{0}", file.FileName);
                        if (!this.imagenenuso(imagen.Path, id))
                        {
                            String oldfile = Server.MapPath(imagen.Path);
                            if (System.IO.File.Exists(oldfile))
                            {
                                System.IO.File.Delete(oldfile);
                            }
                        }
                        string absnewpath = Path.Combine(Server.MapPath("~/Imagenes"), Path.GetFileName(file.FileName));
                        if (!System.IO.File.Exists(absnewpath))
                        {
                            file.SaveAs(absnewpath);
                        }
                        imagen.Path = String.Format(newpath);
                    }
                    imagen.Titulo = collection["Titulo"].Trim();
                    imagen.Comentarios = collection["Comentarios"].Trim();
                    imagen.IdArticulo = Convert.ToInt32(collection["IdArticulo"]);
                    db.SaveChanges();
                    response.Code = 1;
                    response.Msg = String.Format("Se editó la imagen {0}", imagen.Titulo);
                }
                else
                {
                    response.Code = -1;
                    response.Msg = String.Format("La imagen seleccionada ya no existe");
                }  
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
            Topico topico = db.Topico.Where(s => s.Id.Equals(id)).FirstOrDefault();
            if (topico != null)
            {
                return View(topico);
            }
            else
            {
                ResultSet response = new ResultSet();
                response.Code = -1;
                response.Msg = String.Format("La imagen seleccionada ya no existe");
                return RedirectToAction("Index", new RouteValueDictionary(response));
            }
        }
        private bool imagenenuso(String path, int id)
        {
            var img = db.Imagen.Where(s => (s.Path.Equals(path))
                                            && !(s.Id.Equals(id)))
                                     .FirstOrDefault();
            return (img!=null);            

        }
        
        public ActionResult Delete(int id)
        {
            ResultSet response = new ResultSet();
            try
            {
                
                Imagen imagen = db.Imagen.Where(s => s.Id.Equals(id)).FirstOrDefault();
                if (imagen != null)
                {
                    String path = Server.MapPath(imagen.Path);
                    if (System.IO.File.Exists(path) && !this.imagenenuso(imagen.Path, id))
                    {
                        System.IO.File.Delete(path);
                    }
                    db.Imagen.Remove(imagen);
                    db.SaveChanges();
                    response.Code = 1;
                    response.Msg = String.Format("Se borró la imagen {0}", imagen.Titulo);
                }
                else
                {
                    response.Code = -1;
                    response.Msg = String.Format("La imagen seleccionada ya no existe");
                }
                
            }
            catch(Exception exc)
            {
                return View();
            }
            return RedirectToAction("Index", new RouteValueDictionary(response));
        }      
    }
}