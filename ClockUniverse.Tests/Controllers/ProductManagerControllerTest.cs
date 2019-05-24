using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ClockUniverse.Controllers;
using ClockUniverse.Models;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace ClockUniverse.Tests.Controllers
{
    [TestClass]
    public class ProductManagerControllerTest
    {
        [TestMethod]
        public void TestIndex()
        {
            var db = new CsK23T3bEntities();
            var controller = new ProductManagerController();
            var result = controller.Index();
            var view = result as ViewResult;
            Assert.IsNotNull(view);
            var model = view.Model as List<ProductTable>;
            Assert.AreEqual(db.ProductTables.Count(), model.Count);
            
        }
    }
}
