﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MediLab.Models;
using MediLab.Controllers.MyClasses;
using System.Web.Routing;
using System.Globalization;

namespace MediLab.Controllers
{
    public class ArticuloController : Controller
    {
        MedicinaEntities db;
        public ArticuloController()
        {
            db = new MedicinaEntities();

        }

        public ActionResult Index(ResultSet response = null,int topico = -1, int page = 1, int pageSize = 5)
        {
            ViewBag.topico = topico;
            ViewBag.Operacion = response;
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
          
            int totalRecord = db.Articulo.Where(s => (s.IdTopico.Equals(topico) && !topico.Equals(-1) && s.visible.Equals(true)) || (s.IdTopico.Equals(s.IdTopico) && topico.Equals(-1))).Count();
            ViewBag.dbcount = (totalRecord / pageSize) + ((totalRecord % pageSize) > 0 ? 1 : 0);
            var articulos = db.Articulo.Where(s => (s.IdTopico.Equals(topico) && !topico.Equals(-1) && s.visible.Equals(true)) || (s.IdTopico.Equals(s.IdTopico) && topico.Equals(-1))).OrderBy(s => s.Id).Skip((page - 1) * pageSize).Take(pageSize).ToList();
            return articulos;
        }
        public JsonResult BuscarArticulo(string term)
        {
            var resultado = db.Articulo.Where(s => s.Titulo.Contains(term))
                .Select(s => new { value = s.Titulo, Id = s.Id }).Take(5).ToList();
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetArticulo(int codigo)
        {

            var articulo = db.Articulo.Where(s => s.Id.Equals(codigo)).FirstOrDefault();
            if (articulo != null)
            {
                return View(articulo);
            }
            else
            {
                ResultSet response = new ResultSet();
                response.Code = -1;
                response.Msg = String.Format("El artículo seleccionado ya no existe");
                return RedirectToAction("Index", new RouteValueDictionary(response));

            }
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost,ValidateInput(false)]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                ResultSet response = new ResultSet();
                Articulo articulo = new Articulo()
                {
                    Titulo = collection["Titulo"].Trim(),
                    Comentarios = collection["Comentarios"].Trim(),
                    FechaPublicacion = DateTime.Now,
                    IdTopico = Convert.ToInt32(collection["IdTopico"]),
                    visible = Convert.ToBoolean(collection["visible"].Split(',')[0])
                };
                db.Articulo.Add(articulo);
                db.SaveChanges();
                response.Code = 1;
                response.Msg = String.Format("Se creó el artículo {0}", articulo.Titulo);               
                return RedirectToAction("Index", new RouteValueDictionary(response));
              

            }
            catch(Exception ex)
            {
                return View();
            }
        }
        public ActionResult Edit(int id)
        {
            Articulo articulo = db.Articulo.Where(s => s.Id.Equals(id)).FirstOrDefault();
            if (articulo != null)
            {
                return View(articulo);
            }
            else
            {
                ResultSet response = new ResultSet();
                response.Code = -1;
                response.Msg = String.Format("El artículo seleccionado ya no existe");
                return RedirectToAction("Index", new RouteValueDictionary(response));

            }
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {

                ResultSet response = new ResultSet();
                Articulo articulo = db.Articulo.Where(s => s.Id.Equals(id)).FirstOrDefault();
                if (articulo != null)
                {
                    articulo.Titulo = collection["Titulo"].Trim();
                    articulo.Comentarios = collection["Comentarios"].Trim();
                    articulo.IdTopico = Convert.ToInt32(collection["IdTopico"]);
                    articulo.visible = Convert.ToBoolean(collection["visible"].Split(',')[0]);
                    db.SaveChanges();
                    response.Code = 1;
                    response.Msg = String.Format("Se editó el articulo {0}", articulo.Titulo);
                }
                else
                {
                    response.Code = -1;
                    response.Msg = String.Format("El artículo seleccionado ya no existe");
                }             
                return RedirectToAction("Index", new RouteValueDictionary(response)); 
               

            }
            catch(Exception ex)
            {
                return View();
            }
        }
        public ActionResult Delete(int id)
        {
            try
            {
                ResultSet response = new ResultSet();
                Articulo articulo = db.Articulo.Where(s => s.Id.Equals(id)).FirstOrDefault();
                if (articulo != null)
                {
                    db.Articulo.Remove(articulo);
                    db.SaveChanges();
                    response.Code = 1;
                    response.Msg = String.Format("Se borró el articulo {0}", articulo.Titulo);
                    
                }
                else
                {
                    response.Code = -1;
                    response.Msg = String.Format("El artículo seleccionado ya no existe");
                }
                return RedirectToAction("Index", new RouteValueDictionary(response));

            }
            catch
            {
                return View();
            }
        }     
    }
}