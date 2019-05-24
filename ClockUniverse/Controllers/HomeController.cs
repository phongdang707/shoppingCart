using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClockUniverse.Controllers
{
    public class HomeController : Controller
    {
        private CsK23T3bEntities db = new CsK23T3bEntities();
        public ActionResult Index()
        {
            return View(db.ProductTables.ToList());
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Search(string text)
        {
            var itemsz = db.ProductTables.Where(x => x.Watch_Name.ToLower().Contains(text.ToLower())).ToList();

            if (itemsz.Count() > 0)
            {
                //ViewBag.Message = "";
            }
            else
            {
                ViewBag.Message = "No Item found";

            }
            ViewData["Item"] = itemsz;
            return View(itemsz);
        }
    }
}