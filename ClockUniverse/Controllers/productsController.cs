using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClockUniverse.Models;

namespace ClockUniverse.Controllers
{
    public class productsController : Controller
    {
        //
        CsK23T3bEntities db = new CsK23T3bEntities();
        // GET: /products/
        public ActionResult Index()
        {
            return View(db.ProductTables.ToList());
        }
        public ViewResult product_detail(int watch_ID = 0)
        {
            // Tra về đôi tượng với điều kiện
            ProductTable product = db.ProductTables.SingleOrDefault(n => n.Watch_ID == watch_ID);
            if (product == null)
            {
                // Trả về trang báo lỗi
                Response.StatusCode = 404;
                return null;
            }
            return View(product);
        }

    }
}