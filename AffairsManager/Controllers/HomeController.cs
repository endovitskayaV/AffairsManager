using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AffairsManager.Models;

namespace AffairsManager.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
        {
            affairsContext = new AffairsContext();
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

        private AffairsContext affairsContext;

        public ActionResult Index()
        {
             return View(affairsContext.Affairs.ToList());
            /*
             Необходимо представить таблтцу в виде листа, 
             чтобы в представлении можно было проверить условие Model.Count > 0
             Иначе возникает ошибка, потому что тогда у модели не свойста Count
             Count нужен, чтобы проверить условие наличия хоть одного дела
             */
        }

        public  ActionResult Sort(SortCriteria? sortCriteria)
        {
            List<Affairs> list= affairsContext.Affairs.ToList();
            
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

            /*IQueryable (а не IEnumerable) 
            позволяет избежать иключения при выполнении Where(), если Description==null*/
            IQueryable<Affairs> affairsQueryable = affairsContext.Affairs;

            if (request == null || request == "")
            {
                ViewBag.Warning = "Пустой запрос";
            }
            else
            {
                switch (searchCriteria)
                {
                    case SearchCriteria.Name:
                        affairsQueryable = affairsQueryable.Where(x => x.Name.Contains(request));
                        break;
                    case SearchCriteria.Description:
                        affairsQueryable = affairsQueryable.Where(x => x.Description.Contains(request));
                        break;
                    default:
                        affairsQueryable = affairsQueryable.Where(x => x.Name.Contains(request));

                        IQueryable<Affairs> queryNewReq = affairsContext.Affairs;
                        queryNewReq = queryNewReq.Where(x => x.Description.Contains(request));

                        affairsQueryable = affairsQueryable.Union(queryNewReq);
                        affairsQueryable = affairsQueryable.Distinct();
                        break;
                }
               
                if (affairsQueryable.ToList().Count<1)
                {
                    ViewBag.Warning = "Nothing is found";
                }
            }
            return View("Index", affairsQueryable.ToList());
        }

        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(Affairs affair)
        {
            affair.Id =UnixTimeSeconds();
            affair.Date = DateTime.Now;
            affairsContext.Add(affair);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id != null)
            {
                Affairs affair = affairsContext.GetAffair(x => x.Id==id);
                if (affair != null)
                    return View("EditableAffairContent", affair);
            }
            return HttpNotFound();
        }

        [HttpPost]
        public ActionResult Edit(Affairs affair)
        {
            affair.Date = DateTime.Now;
            affairsContext.Edit(affair);
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int? id)
        {
            if (id != null)
            {
                Affairs affair = affairsContext.GetAffair(x => x.Id == id);
                if (affair != null)
                {

                    affairsContext.Delete(affair);
                    return RedirectToAction("Index");
                }
            }
            return HttpNotFound();
        }

        public ActionResult ChooseRandomAffair()
        {
            Random rnd = new Random();
            List<Affairs> affairsList = affairsContext.Affairs.ToList();
            int index = rnd.Next(0, affairsList.Count);
            return View("EditableAffairContent", affairsList[index]);
        }
    

        private int UnixTimeSeconds()
        {
            return (int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }

        protected override void Dispose(bool disposing)
        {
            affairsContext.Dispose();
            base.Dispose(disposing);
        }


    }
}