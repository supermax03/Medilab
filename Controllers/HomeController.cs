using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MediLab.Models;

namespace MediLab.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
     
        public JsonResult BuscarTopico(string term)
        {
            MedicinaEntities db = new MedicinaEntities();
            var resultado = db.Topico.Where(s => s.Nombre.Contains(term))
                .Select(s => new {value=s.Nombre,Id=s.Id}).Take(5).ToList();
            return Json(resultado, JsonRequestBehavior.AllowGet);        
    }



        public ActionResult About()
        {
            ViewBag.Message = "Medilab - Facultad de Medicina UBA";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Facultad de Medicina";

            return View();
        }
    }
}