using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MediLab.Models;
using MediLab.Controllers.MyClasses;
using System.Web.Routing;
using MediLab.Servicios;

namespace MediLab.Controllers
{
    public class UsuarioController : Controller
    {

        MedicinaEntities db;
        public UsuarioController()
        {
            db = new MedicinaEntities();
        
        }     
        public ActionResult Index(ResultSet response = null, int page = 1, int pageSize = 5)
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
            int totalRecord = db.Usuario.Count();
            ViewBag.dbcount = (totalRecord / pageSize) + ((totalRecord % pageSize) > 0 ? 1 : 0);
            var usuarios = db.Usuario.OrderBy(s => s.Id).Skip((page - 1) * pageSize).Take(pageSize).ToList();
            return View(usuarios.AsEnumerable());
        }
        public ActionResult Details(int id)
        {
            Usuario usuario = db.Usuario.Where(s => s.Id.Equals(id)).First();
            return View(usuario);
        }

        private IQueryable<SelectListItem> getEstados(int estado=0)
        {
            var estados = db.EstadoUsuario.Select(s =>
                                                                           new SelectListItem
                                                                           {
                                                                               Value = s.Id.ToString(),
                                                                               Text = s.Nombre,
                                                                               Selected = (s.Id.Equals(estado)) ? true : false
                                                                           }

                                                                          );
            return estados;
        }
        private IQueryable<SelectListItem> getRoles(int rol=0)
        {
            var roles = db.Rol.Select(s =>
                                                                     new SelectListItem
                                                                     {
                                                                         Value = s.Id.ToString(),
                                                                         Text = s.Nombre,
                                                                         Selected = (s.Id.Equals(rol)) ? true : false
                                                                     }

                                                               );
            return roles;
        }



        public ActionResult Edit(int id)
        {
            Usuario usuario = db.Usuario.Where(s => s.Id.Equals(id)).First();  
            ViewBag.Rol = getRoles(usuario.Rol);
            ViewBag.Estado = getEstados(usuario.Estado);
            return View(usuario);
        }
        
        public ActionResult Create()
        {
            
            ViewBag.Rol = getRoles();
            ViewBag.Estado = getEstados();           
            return View();
        }

        public JsonResult GetEstadoUnlockUsers()
        {

            var resultado = Servicios.Servicios.getStatusServiceUsuarios();
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        public JsonResult StopUnlockUsers()
        {

            Servicios.Servicios.turnOffServiceUsuarios();
            return Json(Servicios.Servicios.getStatusServiceUsuarios(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult StartUnlockUsers()
        {

            Servicios.Servicios.turnOnServiceUsuarios();
            return Json(Servicios.Servicios.getStatusServiceUsuarios(), JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                
                ResultSet response = new ResultSet();
                Usuario usuario = new Usuario()
                {                   

                    Username = collection["Username"].Trim(),
                    Password = collection["Password"].Trim(),
                    Rol = Convert.ToInt32(collection["Rol"].Trim()),
                    RegDate= DateTime.Now,
                    Estado =Convert.ToInt32(collection["Estado"].Trim()),
                    Email=collection["Email"].Trim()

                };
                db.Usuario.Add(usuario);
                db.SaveChanges();
                response.Code = 1;
                response.Msg = String.Format("Se creó el usuario {0}", usuario.Username);
                return RedirectToAction("Index", new RouteValueDictionary(response));

            }
            catch(Exception e)
            {
                return View();
            }
        }


        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {

                ResultSet response = new ResultSet();
                Usuario usuario = db.Usuario.Where(s => s.Id.Equals(id)).First();
                usuario.Username = collection["Username"].Trim();
                usuario.Password = collection["Password"].Trim();
                usuario.Rol = Convert.ToInt32(collection["Rol"].Trim());
                usuario.Estado = Convert.ToInt32(collection["Estado"].Trim());
                usuario.Email = collection["Email"].Trim();              
                db.SaveChanges();
                response.Code = 1;
                response.Msg = String.Format("Se editó el usuario {0}", usuario.Username);
                return RedirectToAction("Index", new RouteValueDictionary(response));

            }
            catch(Exception e)
            {
                return View();
            }
        }


        public ActionResult Delete(int id)
        {
            try
            {
                ResultSet response = new ResultSet();
                Usuario usuario = db.Usuario.Where(s => s.Id.Equals(id)).First();                
                db.Usuario.Remove(usuario);
                db.SaveChanges();
                response.Code = 1;
                response.Msg = String.Format("Se borró el usuario {0}", usuario.Username);
                return RedirectToAction("Index", new RouteValueDictionary(response));

            }
            catch
            {
                return View();
            }
        }

    }

}