using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClockUniverse.Models
{
    public class Orderdetail
    {
        CsK23T3bEntities db = new CsK23T3bEntities();
        public int Watch_ID { get; set; }
        public int Amount { get; set; }
        public double Price { get; set; }
        public Order_Detail Order_Detail { get; set; }
        public Orderdetail(int id1, int id2)
        {
            Order_Detail.Order_ID = id1;
            Watch_ID = id2;
            Order_Detail detail = db.Order_Detail.Single(n => n.Order_ID == id1);
            
            Price = double.Parse(detail.Price.ToString());
            Amount = detail.Amount;
        }
    }
}