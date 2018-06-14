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