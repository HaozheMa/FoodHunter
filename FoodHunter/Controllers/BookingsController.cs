using FluentValidation.Results;
using FoodHunter.Models;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace FoodHunter.Controllers
{
    public class BookingsController : Controller
    {
        private FoodHunters db = new FoodHunters();

        // GET: Bookings
        public ActionResult Index()
        {
            var bookingChartList = db.Booking.Include(b => b.AspNetUsers).Include(b => b.Restaurants).ToList();

            var rname = db.Restaurants.Include(r => r.AspNetUsers).ToList();
            List<DataPoint> dataPoints = new List<DataPoint>();
            foreach (Restaurants r in rname)
            {

                int count = 0;
                List<DataPoint> dataPoints1 = new List<DataPoint>();
                foreach (Booking b in bookingChartList)
                {
                    if (b.RestaurantId.Equals(r.Id))
                    {
                        count++;
                    }
                }
                dataPoints.Add(new DataPoint(r.Name, count));

            }
            ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);


            var userId = User.Identity.GetUserId();
            List<Booking> booking = new List<Booking>();
            if (User.IsInRole("Owner"))
            {
                var restaurants = db.Restaurants.Where(r => r.OwnerId == userId).ToList();
                foreach (Restaurants r in restaurants)
                {
                    var restaurantsId = r.Id;
                    var bookings = db.Booking.Where(b => b.RestaurantId == restaurantsId).ToList();
                    booking.AddRange(bookings);
                }
            }
            else if (User.IsInRole("Customer"))
            {
                booking = db.Booking.Where(b => b.CustomerId == userId).ToList();
            }
            else if (User.IsInRole("Administrator"))
            {
                booking = db.Booking.Include(b => b.AspNetUsers).Include(b => b.Restaurants).ToList();
            }


            return View(booking);
        }

        // GET: Bookings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.Booking.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }

            var reviews = db.REVIEW.Include(r => r.AspNetUsers).Include(r => r.Restaurants).ToList();
            double ratingAmount = 0;
            int count = 0;
            double rating = 0;
            foreach (REVIEW r in reviews)
            {
                if (r.RestaurantId.Equals(booking.RestaurantId))
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
            return View(booking);
        }

        // GET: Bookings/Create
        public ActionResult Create()
        {
            ViewBag.CustomerId = new SelectList(db.AspNetUsers, "Id", "Email");
            ViewBag.RestaurantId = new SelectList(db.Restaurants, "Id", "Name");

            return View();
        }

        // POST: Bookings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,RestaurantId,StartDateTime,EndDateTime")] Booking booking)
        {
            RestaurantsValidator rv = new RestaurantsValidator();

            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();
                booking.CustomerId = userId;
                int id = booking.RestaurantId;
                var restaurants = db.Restaurants.Where(r => r.Id == id).ToList().First();
                ValidationResult results = rv.Validate(restaurants);

                if (!results.IsValid)
                {
                    foreach (var failure in results.Errors)
                    {
                        Console.WriteLine("Property " + failure.PropertyName + " failed validation. Error was: " + failure.ErrorMessage);

                    }
                    ViewBag.ErrorMessage = "the booking has been booked!";
                }
                else
                {
                    db.Booking.Add(booking);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

            }

            ViewBag.CustomerId = new SelectList(db.AspNetUsers, "Id", "Email", booking.CustomerId);
            ViewBag.RestaurantId = new SelectList(db.Restaurants, "Id", "Name", booking.RestaurantId);
            return View(booking);
        }

        // GET: Bookings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.Booking.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerId = new SelectList(db.AspNetUsers, "Id", "Email", booking.CustomerId);
            ViewBag.RestaurantId = new SelectList(db.Restaurants, "Id", "Name", booking.RestaurantId);
            return View(booking);
        }

        // POST: Bookings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,RestaurantId,CustomerId,StartDateTime,EndDateTime")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                db.Entry(booking).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerId = new SelectList(db.AspNetUsers, "Id", "Email", booking.CustomerId);
            ViewBag.RestaurantId = new SelectList(db.Restaurants, "Id", "Name", booking.RestaurantId);
            return View(booking);
        }

        // GET: Bookings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.Booking.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }

        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Booking booking = db.Booking.Find(id);
            db.Booking.Remove(booking);
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
