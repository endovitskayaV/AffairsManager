using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AffairsManager.Models;
using System.Data.Entity.Migrations;
namespace AffairsManager.Controllers
{
    public class HomeController : Controller
    {
        AffairsContext db = new AffairsContext();

        public ActionResult Index()
        {
            return View(db.Affairs);
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
                    return View("Index", db.Affairs);
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