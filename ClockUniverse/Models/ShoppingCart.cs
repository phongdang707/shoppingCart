using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ClockUniverse.Models;
namespace ClockUniverse.Models
{
    public class ShoppingCart
    {
        CsK23T3bEntities db = new CsK23T3bEntities();
        public int iMaSP { get; set; }
        public int type { get; set; }
        public string TenSP { get; set; }
        public string Image { get; set; }
        public double donGia { get; set; }
        public int soLuong { get; set; }
        public int TonKho { get; set; }
        public double thanhTien
        {
            get { return soLuong * donGia; }

        }
       
        // Ham tạo cho giỏ hàng
        public ShoppingCart(int MaSP)
        {
            iMaSP = MaSP;
            ProductTable clock = db.ProductTables.Single(n => n.Watch_ID == iMaSP);
            TenSP = clock.Watch_Name;
            donGia = double.Parse(clock.Selling_Price.ToString());
            soLuong = 1;
        }
    }
}