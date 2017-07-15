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

        public ActionResult Index()
        {
           
            return View(db.Affairs);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        public ActionResult Edit()
        {
            
            return View();
        }

    }
}