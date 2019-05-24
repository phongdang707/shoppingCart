using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ClockUniverse;
using System.Transactions;

namespace ClockUniverse.Controllers
{
    public class InformationManagerController : Controller
    {
        private CsK23T3bEntities db = new CsK23T3bEntities();

        // GET: /InformationManager/
        public ActionResult Index(string id)
        {
            var model = db.Contacts.ToList();
            var cont = from o in db.Contacts select o;
            if (!String.IsNullOrEmpty(id))
            {
                var strI = Convert.ToInt32(id.Trim());
                cont = db.Contacts.Where(o => o.Contact_ID ==strI);      
            }
            ViewBag.SearchTerm = id;
            return View(cont.ToList());
        }

        // GET: /InformationManager/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact contact = db.Contacts.Find(id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            return View(contact);
        }

        // GET: /InformationManager/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /InformationManager/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Contact contact, string Title, string Feedback_Detail)
        {
            if (ModelState.IsValid)
                using (var scope = new TransactionScope())
                {
                    db.Contacts.Add(contact);
                    db.SaveChanges();

                    var detail = new ContactsDetail();
                    detail.Title = Title;
                    detail.Feedback_Detail = Feedback_Detail;
                    detail.Feedback_ID = contact.Contact_ID;
                    detail.Date = DateTime.Now;
                    detail.Contact_ID = contact.Contact_ID;
                    db.ContactsDetails.Add(detail);
                    db.SaveChanges();

                    scope.Complete();
                    return RedirectToAction("Index","Home");
                }

            return View(contact);
        }

        // GET: /InformationManager/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact contact = db.Contacts.Find(id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            return View(contact);
        }

        // POST: /InformationManager/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Contact_ID,Customer_Name,Phone,Email,Address,Status")] Contact contact)
        {
            if (ModelState.IsValid)
            {
                db.Entry(contact).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(contact);
        }

        // GET: /InformationManager/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact contact = db.Contacts.Find(id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            return View(contact);
        }

        // POST: /InformationManager/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Contact contact = db.Contacts.Find(id);
            db.Contacts.Remove(contact);
            db.SaveChanges();
            return RedirectToAction("Index");
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
