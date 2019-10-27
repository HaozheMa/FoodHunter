 using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FoodHunter.Models;
using Microsoft.AspNet.Identity;

namespace FoodHunter.Controllers
{
    public class RestaurantsController : Controller
    {
        private FoodHunters db = new FoodHunters();

        // GET: Restaurants
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            List<Restaurants> restaurants = new List<Restaurants>();
            if (User.IsInRole("Owner"))
            {
                restaurants = db.Restaurants.Where(r => r.OwnerId == userId).ToList();
            }
            else
            {
                restaurants = db.Restaurants.Include(r => r.AspNetUsers).ToList();
            }
            return View(restaurants);
        }

        // GET: Restaurants/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Restaurants restaurants = db.Restaurants.Find(id);
            if (restaurants == null)
            {
                return HttpNotFound();
            }

            var reviews = db.REVIEW.Include(r => r.AspNetUsers).Include(r => r.Restaurants).ToList();
            double ratingAmount = 0;
            int count = 0;
            double rating = 0;
            foreach (REVIEW r in reviews)
            {
                if (r.RestaurantId.Equals(id))
                {
                    ratingAmount = (double)r.Rating + ratingAmount;
                    count++;
                }
            }
            if (count != 0)
            {
                rating = ratingAmount / count;
            }
            ViewBag.rating = rating;
            return View(restaurants);
        }

        // GET: Restaurants/Create
        public ActionResult Create()
        {
            ViewBag.OwnerId = new SelectList(db.AspNetUsers, "Id", "Email");
            return View();
        }

        // POST: Restaurants/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Address,PhoneNumber,BookStatus,CreateDate,Latitude,Longitude")] Restaurants restaurants)
        {
            var userId = User.Identity.GetUserId();
            restaurants.OwnerId = userId;
            if (ModelState.IsValid)
            {
                db.Restaurants.Add(restaurants);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.OwnerId = new SelectList(db.AspNetUsers, "Id", "Email", restaurants.OwnerId);
            return View(restaurants);
        }

        // GET: Restaurants/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Restaurants restaurants = db.Restaurants.Find(id);
            if (restaurants == null)
            {
                return HttpNotFound();
            }
            ViewBag.OwnerId = new SelectList(db.AspNetUsers, "Id", "Email", restaurants.OwnerId);
            return View(restaurants);
        }

        // POST: Restaurants/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Address,PhoneNumber,BookStatus,OwnerId,CreateDate,Latitude,Longitude")] Restaurants restaurants)
        {
            if (ModelState.IsValid)
            {
                db.Entry(restaurants).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OwnerId = new SelectList(db.AspNetUsers, "Id", "Email", restaurants.OwnerId);
            return View(restaurants);
        }

        // GET: Restaurants/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Restaurants restaurants = db.Restaurants.Find(id);
            if (restaurants == null)
            {
                return HttpNotFound();
            }
            return View(restaurants);
        }

        // POST: Restaurants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Restaurants restaurants = db.Restaurants.Find(id);
            db.Restaurants.Remove(restaurants);
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
