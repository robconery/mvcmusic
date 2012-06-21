using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcMusicStore.Models;
using MvcMusicStore.Controllers;

namespace MvcMusicStore.Controllers
{ 
    public class OrdersController : DbController
    {
        const string PromoCode = "FREE";
        //
        // GET: /Default1/

        [Authorize(Roles = "Administrator")]
        public ViewResult Index()
        {
            return View(db.Orders.ToList());
        }

        //
        // GET: /Default1/Details/5

        public ViewResult Details(int id)
        {
            // Validate customer owns this order
            Order order = db.Orders
                .Include("Transactions")
                .Include("OrderDetails")
                .Include("Notes")
                .Where(x => x.OrderId == id).FirstOrDefault();

            return View(order);
        }

        //The Checkout Page
        public ActionResult Create()
        {
            return View();
        }

        //
        // GET: /Default1/Create

        [HttpPost]
        public ActionResult Create(FormCollection values)
        {
            var order = new Order();
            TryUpdateModel(order);


                if (string.Equals(values["PromoCode"], PromoCode,
                    StringComparison.OrdinalIgnoreCase) == false)
                {
                    return View(order);
                }
                else
                {
                    order.Username = User.Identity.Name;
                    order.OrderDate = DateTime.Now;

                    //Save Order
                    db.Orders.Add(order);

                    //Process the order
                    var cart = ShoppingCart.GetCart(db, this.HttpContext);
                    cart.CreateOrder(order);
                    
                    //add a note
                    order.Notes = new List<OrderNote>();
                    order.Transactions = new List<Transaction>();
                    order.Notes.Add(new OrderNote { Note = "Preparing Order", CreatedOn = DateTime.Now});
                    
                    //auth the charge...
                    order.Transactions.Add(new Transaction { Processor = "coupon", Authorization = PromoCode, Amount = cart.GetTotal(), CreatedOn = DateTime.Now, Discount = 0, OrderId = order.OrderId });
                    order.Notes.Add(new OrderNote { Note = "Transaction Authorized by Coupon: " + PromoCode, CreatedOn = DateTime.Now});

                    //send a thank you note via email
                    order.Notes.Add(new OrderNote { Note = "Thank You Invoice Email Sent", CreatedOn = DateTime.Now});

                    //flush it since we need the new id
                    db.SaveChanges();
                    //save it down
                    return RedirectToAction("Details",
                        new { id = order.OrderId });
                }

            

        } 

        //
        // GET: /Default1/Edit/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(int id)
        {
            //grab it all at once to avoid late-bound, extra lazy queries
            Order order = db.Orders
                .Include("Transactions")
                .Include("OrderDetails")
                .Include("Notes")
                .Where(x => x.OrderId == id).FirstOrDefault();

            return View(order);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                return RedirectToAction("Index");
            }
            return View(order);
        }

        //
        // GET: /Default1/Delete/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(int id)
        {
            Order order = db.Orders.Find(id);
            return View(order);
        }

        //
        // POST: /Default1/Delete/5

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Administrator")]
        public ActionResult DeleteConfirmed(int id)
        {            
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
            return RedirectToAction("Index");
        }

    }
}