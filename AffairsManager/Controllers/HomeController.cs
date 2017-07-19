using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AffairsManager.Models;
using System.Data.Entity.Migrations;
using System.ComponentModel.DataAnnotations;

namespace AffairsManager.Controllers
{
    public class HomeController : Controller
    {
        public SortCriteria sortCriteria;
        public enum SortCriteria
        {
            //[Display (Name= "Имя А-Я")]      
            NameAsc, //возрастанию
            NameDesc, //убыванию
            CategoryAsc,
            CategoryDesc,
            DateAsc, // сначала добавленные давно
            DateDesc //сначала последние добавленные
        }
        private AffairsContext db = new AffairsContext();

        public ActionResult Index(SortCriteria sortCriteria = SortCriteria.DateAsc)
        {
            List<Affairs> affairsList = db.Affairs.ToList();

            IOrderedEnumerable<Affairs> affairs = affairsList.OrderBy(x => x.Id);
            switch (sortCriteria)
            {
                case SortCriteria.NameAsc:
                    affairs = affairsList.OrderBy(x => x.Name);
                    break;
                case SortCriteria.NameDesc:
                    affairs = affairsList.OrderByDescending(x => x.Name);
                    break;
                case SortCriteria.CategoryAsc:
                    affairs = affairsList.OrderBy(x => x.Category);
                    break;
                case SortCriteria.CategoryDesc:
                    affairs = affairsList.OrderByDescending(x => x.Category);
                    break;
                case SortCriteria.DateDesc:
                    //affairsList.Reverse();
                    affairs = affairsList.OrderByDescending(x => x.Id); //НЕКРАСИВО!!!
                    break;
            }

            return View("Index", affairs.ToList());

            /* ViewBag.Criteria=new SelectList(
                new List<SortCriteria>
                {SortCriteria.CategoryAsc,SortCriteria.CategoryDesc,
                 SortCriteria.DateAsc,SortCriteria.DateDesc,
                 SortCriteria.NameAsc, SortCriteria.NameDesc});*/
            // return View(db.Affairs);
        }

        [HttpPost]
        public ActionResult Sort(SortCriteria sortCriteria = SortCriteria.DateAsc)
        {
            List<Affairs> affairsList = db.Affairs.ToList();

            IOrderedEnumerable<Affairs> affairs = affairsList.OrderBy(x => x.Id);
            switch (sortCriteria)
            {
                case SortCriteria.NameAsc:
                    affairs = affairsList.OrderBy(x => x.Name);
                    break;
                case SortCriteria.NameDesc:
                    affairs = affairsList.OrderByDescending(x => x.Name);
                    break;
                case SortCriteria.CategoryAsc:
                    affairs = affairsList.OrderBy(x => x.Category);
                    break;
                case SortCriteria.CategoryDesc:
                    affairs = affairsList.OrderByDescending(x => x.Category);
                    break;
                case SortCriteria.DateDesc:
                    //affairsList.Reverse();
                    affairs = affairsList.OrderByDescending(x => x.Id); //НЕКРАСИВО!!!
                    break;
            }

            return View("Index", affairs.ToList());
        }

        [HttpGet]
        public ActionResult AddAffair()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddAffair(Affairs affair)
        {
            db.Affairs.Add(affair);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id != null)
            {
                Affairs affair = db.Affairs.FirstOrDefault(x => x.Id == id);
                if (affair != null)
                    return View(affair);
            }
            return HttpNotFound();
        }

        [HttpPost]
        public ActionResult Edit(Affairs affair)
        {
            db.Affairs.AddOrUpdate(affair);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        /* [HttpGet]
         [ActionName("Delete")]
         public ActionResult ConfirmDelete(int? id)
         {
             if (id != null)
             {
                 Affairs affair = db.Affairs.FirstOrDefault(x => x.Id == id);
                 if (affair != null)
                     return View();
             }
             return HttpNotFound();
         }
         
         [HttpPost]*/
        public ActionResult Delete(int? id)
        {
            if (id != null)
            {
                Affairs affair = db.Affairs.FirstOrDefault();
                if (affair != null)
                {
                    db.Affairs.Remove(affair);
                    db.SaveChanges();

                    if (db.Affairs.ToList().Count < 1)
                         return RedirectToAction("Index");
                    else return RedirectToAction("Index", db.Affairs);
                }
            }
            return HttpNotFound();
        }

        public ActionResult ChooseRandomAffair()
        {
            Random rnd = new Random();
            List<Affairs> affairsList = db.Affairs.ToList();
            int index = rnd.Next(0, affairsList.Count);
            return View("Random", affairsList[index]);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }


    }
}