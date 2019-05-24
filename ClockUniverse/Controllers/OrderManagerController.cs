using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Runtime.Caching;
using ClockUniverse.Controllers;
using ClockUniverse.Models;
using System.Transactions;
namespace ClockUniverse.Controllers
{
    public class OrderManagerController : Controller
    {
        private CsK23T3bEntities db = new CsK23T3bEntities();
        // GET: /OrderManager/
        public List<ShoppingCart> GetShoppingCart()
        {
            List<ShoppingCart> lstcart = Session["GioHang"] as List<ShoppingCart>;
            if (lstcart == null)
            {
                // Neu gio hang chua ton tai thi minh tien hang tao gio hang
                lstcart = new List<ShoppingCart>();
                Session["GioHang"] = lstcart;
            }
            return lstcart;
        }
        public ActionResult Index()
        {
            var model = db.Orders.ToList();
            return View(model);

        }

        // GET: /OrderManager/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var order = db.Orders.Find(id);

            if (order == null)
            {
                return HttpNotFound();

            }

            return View(order);

        }


        // GET: /OrderManager/Create


        // POST: /OrderManager/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

        // GET: /OrderManager/Edit/5
        public ActionResult Edit(int id)
        {


            Order od = db.Orders.Find(id);
            if (od == null)
            {
                return HttpNotFound();
            }
            ViewBag.DS = new SelectList(
            new List<SelectListItem>
             {
                 new SelectListItem { Text = "Đang xử lý", Value = "1"},
                 new SelectListItem { Text = "Đã tiếp nhận", Value = "2"},
                 new SelectListItem { Text = "Đang vận chuyển", Value = "3"},
                 new SelectListItem { Text = "Đã giao hàng", Value = "4"}
            }, "Value", "Text", od.Deliver_Status);

            return View(od);

        }

        // POST: /OrderManager/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Order order, int Deliver_Status)
        {


            if (ModelState.IsValid)
            {
                order = db.Orders.Find(order.Order_ID);
                order.Order_ChangeDate = DateTime.Now;
                order.Deliver_Status = Deliver_Status;
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            ViewBag.DS = new SelectList(
            new List<SelectListItem>
             {
                 new SelectListItem { Text = "Đang xử lý", Value = "1"},
                 new SelectListItem { Text = "Đã tiếp nhận", Value = "2"},
                 new SelectListItem { Text = "Đang vận chuyển", Value = "3"},
                 new SelectListItem { Text = "Đã giao hàng", Value = "4"}
            }, "Value", "Text");

            return View(order);
        }

        // GET: /OrderManager/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order od = db.Orders.Find(id);

            if (od == null)
            {
                return HttpNotFound();
            }
            return View(od);
        }

        // POST: /OrderManager/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Orders.Find(id);
            foreach(var item in order.Order_Detail.ToList())
            {
                var od = db.Order_Detail.Find(item.Order_ID,item.Watch_ID);
                db.Order_Detail.Remove(od);
                var product = db.ProductTables.Find(item.Watch_ID);
                product.InStock = product.InStock + od.Amount;
                db.Entry(product).State = EntityState.Modified;
            }
            db.Orders.Remove(order);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Create()
        {
            return View();
        }

        // POST: /InformationManager/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Order order)
        {
            
            if (ModelState.IsValid)
            {
                List<ShoppingCart> cart = GetShoppingCart();
                order.Order_Date = DateTime.Now;
                order.Delivery_Date = DateTime.Now.AddDays(3);
                order.Deliver_Status = 1;

                db.Orders.Add(order);

                foreach (var item in cart)
                {
                    Order_Detail order_Detail = new Order_Detail();
                    order_Detail.Order_ID = order.Order_ID;
                    order_Detail.Watch_ID = item.iMaSP;
                    order_Detail.Amount = (int)item.soLuong;
                    order_Detail.Price = Convert.ToDecimal(item.thanhTien);
                    db.Order_Detail.Add(order_Detail);
                    order.Total_Price += Convert.ToDecimal(item.thanhTien);
                    db.Orders.Add(order);
                    ProductTable product = db.ProductTables.Find(item.iMaSP);
                    product.InStock = product.InStock - item.soLuong;
                    db.Entry(product).State = EntityState.Modified;
                }
                Session["GioHang"] = null;
                db.SaveChanges();
                return RedirectToAction("Index","Home");
            }


            return View("~/Views/CheckOut/Index.cshtml");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
