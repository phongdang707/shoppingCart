using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ClockUniverse.Controllers;
using System.Web.Mvc;
using ClockUniverse.Models;


namespace ClockUniverse.Controllers
{
    public class ShoppingCartController : Controller
    {
        CsK23T3bEntities db = new CsK23T3bEntities();
        // get Shoppping Cart

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
        // Them gio hang
        public ActionResult addShoppingCart(int iMaSP, string strUrl, int txtSoLuong)
        {

            ProductTable product = db.ProductTables.SingleOrDefault(n => n.Watch_ID == iMaSP);

            if (txtSoLuong <= 0 || txtSoLuong.ToString().Trim().Equals(null))
            {
                txtSoLuong = 1;
            }

            if (product == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            // Lấy ra session giỏ hàng
            List<ShoppingCart> lstCart = GetShoppingCart();
            // Kiểm tra sản phẩm này đã tồn tại trong session[giohang] chưa
            ShoppingCart sp = lstCart.Find(n => n.iMaSP == iMaSP);

            if (sp == null)
            {

                sp = new ShoppingCart(iMaSP);
                sp.soLuong = txtSoLuong;
                lstCart.Add(sp);
                return Redirect(strUrl);
            }
            else
            {

                sp.soLuong = sp.soLuong + txtSoLuong;

                return Redirect(strUrl);
            }


        }

        
        // Cap nhat gio hang
        public ActionResult UpdateShoppingCart(int iMaSP, FormCollection f)
        {
            // Kiem tra ma san pham
            ProductTable product = db.ProductTables.SingleOrDefault(n => n.Watch_ID == iMaSP);
            // Neu lay sai ma san pham thi tra ve loi
            if (product == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            // lay gio hang ra tu session
            List<ShoppingCart> lstCart = GetShoppingCart();
            // Kiem tra san pham co ton tai tron session
            ShoppingCart sp = lstCart.SingleOrDefault(n => n.iMaSP == iMaSP);
            // Neu ton tai thi cho sua so luong
            if (sp != null)
            {
                sp.soLuong = int.Parse(f["txtSoLuong"].ToString());

            }
            return RedirectToAction("ShoppingCart");

        }
        // Xoa gio hang
        public ActionResult DeleteShoppingCart(int iMaSP)
        {
            // Kiem tra ma san pham
            ProductTable product = db.ProductTables.SingleOrDefault(n => n.Watch_ID == iMaSP);
            // Neu lay sai ma san pham thi tra ve loi
            if (product == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            // lay gio hang ra tu session
            List<ShoppingCart> lstCart = GetShoppingCart();
            // Kiem tra san pham co ton tai tron session
            ShoppingCart sp = lstCart.SingleOrDefault(n => n.iMaSP == iMaSP);
            // Neu ton tai thi cho sua so luong
            if (sp != null)
            {
                lstCart.RemoveAll(n => n.iMaSP == iMaSP);
            }
            if (lstCart.Count == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("ShoppingCart");
        }
        //Xay dung trang gio hang
        public ActionResult ShoppingCart()
        {


            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            List<ShoppingCart> lstCart = GetShoppingCart();
            return View(lstCart);
        }
        // Tinh tong so luong va tong tien
        private int SumAmount()
        {
            int iSumAmount = 0;
            List<ShoppingCart> lstCart = Session["GioHang"] as List<ShoppingCart>;
            if (lstCart != null)
            {
                iSumAmount = lstCart.Sum(n => n.soLuong);

            }
            return iSumAmount;
        }
        // Tinh tong thanh tien
        private double SumPrice()
        {
            double dSumPrice = 0;
            List<ShoppingCart> lstCart = Session["GioHang"] as List<ShoppingCart>;
            if (lstCart != null)
            {
                dSumPrice += lstCart.Sum(n => n.thanhTien);
            }
            return dSumPrice;
        }


        public ActionResult Index()
        {

            ViewBag.Message = "Your application description page.";




            return View();
        }
        // Tao partial gio hang
        public ActionResult CartPartial()
        {
            if (SumAmount() == 0)
            {
                ViewBag.SumAmount = 0;
                ViewBag.TongSoLuong =0;
                ViewBag.SumPrice = 0;
                return PartialView();
            }
            ViewBag.TongSoLuong = SumAmount();
            ViewBag.TongTien = SumPrice();
            return PartialView();
        }
        // Xay dunng view cho nguoi dung chinh sua gio hang
        public ActionResult EditShoppingCart()
        {
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            List<ShoppingCart> lstCart = GetShoppingCart();
            return View(lstCart);
        }
        [HttpPost]
        public ActionResult DatHang()
        {
            // Kiem tra don dat hang
            if (Session["GioHang"] == null)
            {
                RedirectToAction("index", "Home");
            }

            // Them don hang
            Order order = new Order();
            List<ShoppingCart> cart = GetShoppingCart();
            order.Delivery_Date = DateTime.Now;
            db.Orders.Add(order);
            db.SaveChanges();
            // Them chi tiet don hang
            foreach (var item in cart)
            {
                Order_Detail order_Detail = new Order_Detail();
                order_Detail.Order_ID = order.Order_ID;
                order_Detail.Watch_ID = item.iMaSP;
                order_Detail.Amount = (int)item.soLuong;
                db.Order_Detail.Add(order_Detail);
            }
            db.SaveChanges();
            return RedirectToAction("index", "Home");
        }
        public ActionResult _TotalPartial()
        {
            if (SumAmount() == 0)
            {
                ViewBag.SumAmount = 0;
                ViewBag.SumPrice = 0;
                return PartialView();
            }
            ViewBag.TongSoLuong = SumAmount();
            ViewBag.TongTien = SumPrice();
            return PartialView();
        }
    }
}