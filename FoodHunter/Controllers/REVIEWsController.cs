using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FoodHunter.Models;

namespace FoodHunter.Controllers
{
    public class REVIEWsController : Controller
    {
        private FoodHunters db = new FoodHunters();

        // GET: REVIEWs
        public ActionResult Index()
        {
            var rEVIEW = db.REVIEW.Include(r => r.AspNetUsers).Include(r => r.Restaurants);
            return View(rEVIEW.ToList());
        }

        // GET: REVIEWs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            REVIEW rEVIEW = db.REVIEW.Find(id);
            if (rEVIEW == null)
            {
                return HttpNotFound();
            }
            return View(rEVIEW);
        }

        // GET: REVIEWs/Create
        public ActionResult Create()
        {
            ViewBag.CustomerId = new SelectList(db.AspNetUsers, "Id", "Email");
            ViewBag.RestaurantId = new SelectList(db.Restaurants, "Id", "Name");
            return View();
        }

        // POST: REVIEWs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,RestaurantId,Review1,Rating,CustomerId")] REVIEW rEVIEW)
        {
            if (ModelState.IsValid)
            {
                db.REVIEW.Add(rEVIEW);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CustomerId = new SelectList(db.AspNetUsers, "Id", "Email", rEVIEW.CustomerId);
            ViewBag.RestaurantId = new SelectList(db.Restaurants, "Id", "Name", rEVIEW.RestaurantId);
            return View(rEVIEW);
        }

        // GET: REVIEWs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            REVIEW rEVIEW = db.REVIEW.Find(id);
            if (rEVIEW == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerId = new SelectList(db.AspNetUsers, "Id", "Email", rEVIEW.CustomerId);
            ViewBag.RestaurantId = new SelectList(db.Restaurants, "Id", "Name", rEVIEW.RestaurantId);
            return View(rEVIEW);
        }

        // POST: REVIEWs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,RestaurantId,Review1,Rating,CustomerId")] REVIEW rEVIEW)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rEVIEW).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerId = new SelectList(db.AspNetUsers, "Id", "Email", rEVIEW.CustomerId);
            ViewBag.RestaurantId = new SelectList(db.Restaurants, "Id", "Name", rEVIEW.RestaurantId);
            return View(rEVIEW);
        }

        // GET: REVIEWs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            REVIEW rEVIEW = db.REVIEW.Find(id);
            if (rEVIEW == null)
            {
                return HttpNotFound();
            }
            return View(rEVIEW);
        }

        // POST: REVIEWs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            REVIEW rEVIEW = db.REVIEW.Find(id);
            db.REVIEW.Remove(rEVIEW);
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
