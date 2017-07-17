using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AffairsManager.Models;

namespace AffairsManager.Controllers
{
    public class HomeController : Controller
    {
        AffairsContext db = new AffairsContext();

        [HttpGet]
        public ActionResult AddAffair()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddAffair(Affairs affairs)
        {
            return View("Index", affairs);
        }

        public ActionResult Index()
        {
            return View(db.Affairs);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            //НЕБЕЗОПАСНО!!!!!!!!
            /*ViewBag.Name = affair.Name;
            ViewBag.Description = affair.Description;
            ViewBag.Category = affair.Category;*/


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
            db.Affairs.Add(affair);
            db.SaveChanges();
            return View("Index");
        }

        public ActionResult ChooseRandomAffair()
        {
            return View();
        }



    }
}