using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClockUniverse.Models
{
    public class Cart
    {
        public int id { get; set; }
        public int idProduct { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
    }
}