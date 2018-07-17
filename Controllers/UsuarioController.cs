using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MediLab.Models;
using MediLab.Controllers.MyClasses;
using System.Web.Routing;
using MediLab.Servicios;
using Libreria;


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
        public JsonResult GetEstadoMailer()
        {

            var resultado = Servicios.Servicios.getStatusMailerService();
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult StopUnlockUsers()
        {

            Servicios.Servicios.turnOffServiceUsuarios();
            return Json(Servicios.Servicios.getStatusServiceUsuarios(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult StopMailer()
        {

            Servicios.Servicios.turnOffMailer();
            return Json(Servicios.Servicios.getStatusMailerService(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult StartUnlockUsers()
        {

            Servicios.Servicios.turnOnServiceUsuarios();
            return Json(Servicios.Servicios.getStatusServiceUsuarios(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult StartMailer()
        {

            Servicios.Servicios.turnOnMailer();
            return Json(Servicios.Servicios.getStatusMailerService(), JsonRequestBehavior.AllowGet);
        }
        private bool mailenuso(string email,int idUser=0)
        {
            var resultado = db.Usuario.Where(s => s.Email.Equals(email) && (!s.Id.Equals(idUser))).FirstOrDefault();
            return (resultado!=null);

        }
        private bool usernameenuso(string username, int idUser=0)
        {
            var resultado = db.Usuario.Where(s => s.Username.Equals(username) && (!s.Id.Equals(idUser))).FirstOrDefault();
            return (resultado != null);
        }


        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {                
                ResultSet response = new ResultSet();                
                
                if (usernameenuso(collection["Username"].Trim()))              
                {
                    response.Code = -1;
                    response.Msg = String.Format("El Username {0} ya esta siendo utilizado", collection["Username"].Trim());
                }
                else
                {
                    if (mailenuso(collection["Email"].Trim()))
                    {
                        response.Code = -1;
                        response.Msg = String.Format("El mail {0} ya esta siendo utilizado", collection["Email"].Trim());
                    }
                    else
                    {
                        Usuario usuario = new Usuario()
                        {

                            Username = collection["Username"].Trim(),
                            Password = collection["Password"].Trim(),
                            Rol = Convert.ToInt32(collection["Rol"].Trim()),
                            RegDate = DateTime.Now,
                            Estado = Convert.ToInt32(collection["Estado"].Trim()),
                            Email = collection["Email"].Trim()
                        };

                        Novedad novedad = new Novedad()
                        {
                            IdUser = usuario.Id,
                            FechaPublicacion = DateTime.Now,
                            IdTemplate = (int)MyTemplate.TypeOp.CreateUser

                        };

                        db.Novedad.Add(novedad);
                        db.Usuario.Add(usuario);
                        db.SaveChanges();
                        response.Code = 1;
                        response.Msg = String.Format("Se creó el usuario {0}", usuario.Username);
                    }
                }       
             
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
                if (mailenuso(collection["Email"].Trim(), id))
                {
                        response.Code = -1;
                        response.Msg = String.Format("El mail {0} ya esta siendo utilizado", collection["Email"].Trim());
                }
                else
                {
                    Usuario usuario = db.Usuario.Where(s => s.Id.Equals(id)).First();
                    if (!usuario.Password.Equals(collection["Password"].Trim()))//Aplicar modulo de encriptacion y desencriptacion
                        {
                            Novedad novedad = new Novedad()
                            {
                                IdUser = usuario.Id,
                                FechaPublicacion = DateTime.Now,
                                IdTemplate = (int)MyTemplate.TypeOp.ChangePassword

                            };
                            db.Novedad.Add(novedad);
                        }
                        usuario.Password = collection["Password"].Trim();
                        usuario.Rol = Convert.ToInt32(collection["Rol"].Trim());
                        usuario.Estado = Convert.ToInt32(collection["Estado"].Trim());
                        usuario.Email = collection["Email"].Trim();
                        db.SaveChanges();
                        response.Code = 1;
                        response.Msg = String.Format("Se editó el usuario {0}", usuario.Username);                       
                 }
                            
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