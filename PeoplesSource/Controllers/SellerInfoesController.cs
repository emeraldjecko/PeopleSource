using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PeoplesSource.Data.Models;

namespace PeoplesSource.Controllers
{
    [Authorize]
    public class SellerInfoesController : Controller
    {
        private ProductEntities db = new ProductEntities();

        // GET: SellerInfoes
        public ActionResult Index()
        {
            return View(db.SellerInfoes.ToList());
        }

        // GET: SellerInfoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SellerInfo sellerInfo = db.SellerInfoes.Find(id);
            if (sellerInfo == null)
            {
                return HttpNotFound();
            }
            return View(sellerInfo);
        }

        // GET: SellerInfoes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SellerInfoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Increment,IsPercentage,KZ,OHT")] SellerInfo sellerInfo)
        {
            if (ModelState.IsValid)
            {
                db.SellerInfoes.Add(sellerInfo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(sellerInfo);
        }

        // GET: SellerInfoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SellerInfo sellerInfo = db.SellerInfoes.Find(id);
            if (sellerInfo == null)
            {
                return HttpNotFound();
            }
            return View(sellerInfo);
        }

        // POST: SellerInfoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Increment,IsPercentage,KZ,OHT")] SellerInfo sellerInfo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sellerInfo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sellerInfo);
        }

        // GET: SellerInfoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SellerInfo sellerInfo = db.SellerInfoes.Find(id);
            if (sellerInfo == null)
            {
                return HttpNotFound();
            }
            return View(sellerInfo);
        }

        // POST: SellerInfoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SellerInfo sellerInfo = db.SellerInfoes.Find(id);
            db.SellerInfoes.Remove(sellerInfo);
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
