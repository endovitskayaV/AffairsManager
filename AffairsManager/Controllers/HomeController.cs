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
            DateAsc, // сначала добавленные давно
            NameAsc, //возрастанию
            NameDesc, //убыванию
            CategoryAsc,
            CategoryDesc,
            DateDesc //сначала последние добавленные
        }

        public enum SearchCriteria
        {
            Name,
            Description,
            Category,
            All
        }

        private AffairsContext db;
        private SortCriteria sortCriteria;

        public ActionResult Index()
        {
            return Sort(this.sortCriteria);
           // return View(db.Affairs.ToList());
            /*
             Необходимо представить БД в виде листа, 
             чтобы в представлении можно было проверить условие Model.Count > 0
             Иначе возникает ошибка, потому что тогда у модели не свойста Count
             Count нужен, чтобы проверить условие наличия хоть одного дела
             */
        }

        public  ActionResult Sort(SortCriteria sortCriteria)
        {
            this.sortCriteria = sortCriteria;
            IQueryable<Affairs> query = db.Affairs;
            query = query.OrderBy(x => x.Id);
            
            switch (sortCriteria)
            {
                case SortCriteria.NameAsc:
                    query = query.OrderBy(x => x.Name);
                    break;
                case SortCriteria.NameDesc:
                    query = query.OrderByDescending(x => x.Name);
                    break;
                case SortCriteria.CategoryAsc:
                    query = query.OrderBy(x => x.Category);
                    break;
                case SortCriteria.CategoryDesc:
                    query = query.OrderByDescending(x => x.Category);
                    break;
                case SortCriteria.DateDesc:
                    //affairsList.Reverse();
                    query = query.OrderByDescending(x => x.Id); //НЕКРАСИВО?!
                    break;
            }
            return View("Index", query.ToList());

        }

        public ActionResult Search(string request, SearchCriteria searchCriteria )
        {
            IQueryable<Affairs> query = db.Affairs;
            if (request == null || request == "")
                return Content("<script language='javascript' type='text/javascript'>alert('Empty request!');</script>");
            else
            {
                switch (searchCriteria)
                {
                    case SearchCriteria.Name:
                        query = query.Where(x => x.Name.Contains(request));
                        break;
                    case SearchCriteria.Description:
                        query = query.Where(x => x.Description.Contains(request));
                        break;
                    case SearchCriteria.Category:
                        query = query.Where(x => x.Category.Contains(request));
                        break;
                    default:
                        query = query.Where(x => x.Name.Contains(request));
                        IQueryable<Affairs> queryNewReq = db.Affairs;
                        queryNewReq= queryNewReq.Where(x => x.Description.Contains(request));
                        query = query.Union(queryNewReq);
                        queryNewReq = db.Affairs;
                        queryNewReq = queryNewReq.Where(x => x.Category.Contains(request));
                        query = query.Union(queryNewReq);
                        query = query.Distinct();
                        break;
                }
                return View("Index", query.ToList());
            }
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
            return View("Random", affairsList[index]);
        }

       /* protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
        */


    }
}