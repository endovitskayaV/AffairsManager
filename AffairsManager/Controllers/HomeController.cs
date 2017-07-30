using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AffairsManager.Models;
using System.Data.Entity.Migrations;
using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
//using System.Data.Entity;

namespace AffairsManager.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
        {
            db = new AffairsContext();
        }

        public enum SortCriteria
        {
            Default,
            New,
            Important
        }

        public enum SearchCriteria
        {
            Name,
            Description,
            All
        }

        private AffairsContext db;

        public ActionResult Index()
        {
             return View(db.Affairs.ToList());
            /*
             Необходимо представить БД в виде листа, 
             чтобы в представлении можно было проверить условие Model.Count > 0
             Иначе возникает ошибка, потому что тогда у модели не свойста Count
             Count нужен, чтобы проверить условие наличия хоть одного дела
             */
        }

        public  ActionResult Sort(SortCriteria? sortCriteria)
        {
            List<Affairs> list= db.Affairs.ToList();
            
            switch (sortCriteria)
            {
                case SortCriteria.Important:
                    list.Sort((a, b) => b.Importance.CompareTo(a.Importance));//НАПИСАТЬ СВОЙ КОМПАРАТОР!
                    break;
                case SortCriteria.New:
                    list.Reverse();
                    break;
            }
            return View("Index", list);
        }

        public ActionResult Search(string request, SearchCriteria searchCriteria)
        {
            ViewBag.Message = "Найдено по запросу " + "'" + request + "'";
            ViewBag.SelectedCriteria = searchCriteria;
            ViewBag.Request = request;

            IEnumerable<Affairs> affairsEnumerable = db.Affairs;

            if (request == null || request == "")
            {
                ViewBag.Warning = "Пустой запрос";
            }
            else
            {
                switch (searchCriteria)
                {
                    case SearchCriteria.Name:
                        affairsEnumerable = affairsEnumerable.Where(x => x.Name.Contains(request));
                        break;
                    case SearchCriteria.Description:
                        affairsEnumerable = affairsEnumerable.Where(x => x.Description.Contains(request));
                        break;
                    default:
                        affairsEnumerable = affairsEnumerable.Where(x => x.Name.Contains(request));
                        IQueryable<Affairs> queryNewReq = db.Affairs;
                        queryNewReq = queryNewReq.Where(x => x.Description.Contains(request));
                        affairsEnumerable = affairsEnumerable.Union(queryNewReq);
                        affairsEnumerable = affairsEnumerable.Distinct();
                        break;
                }
               
                if (affairsEnumerable.ToList().Count<1)
                {
                    ViewBag.Warning = "Nothing is found";
                    affairsEnumerable = db.Affairs;
                }
            }
            return View("Index", affairsEnumerable.ToList());
        }

        [HttpGet]
        public ActionResult AddAffair()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddAffair(Affairs affair)
        {
            affair.Id =UnixTimeSeconds();
            affair.Date = DateTime.Now;
            db.Affairs.Add(affair);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id != null)
            {
                Affairs affair = db.Affairs.FirstOrDefault(x => x.Id==id);
                if (affair != null)
                    return View("EditableAffairContent", affair);
            }
            return HttpNotFound();
        }

        [HttpPost]
        public ActionResult Edit(Affairs affair)
        {
            affair.Date = DateTime.Now;
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
                Affairs affair = db.Affairs.FirstOrDefault(x => x.Id == id);
                if (affair != null)
                {
                    
                    db.Affairs.Remove(affair);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return HttpNotFound();
        }

        public ActionResult ChooseRandomAffair()
        {
            Random rnd = new Random();
            List<Affairs> affairsList = db.Affairs.ToList();
            int index = rnd.Next(0, affairsList.Count);
            return View("EditableAffairContent", affairsList[index]);
        }
    

        private int UnixTimeSeconds()
        {
            return (int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }


    }
}