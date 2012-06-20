using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcMusicStore.Models;

namespace MvcMusicStore.controllers
{ 
    public class OrdersController : Controller
    {
        private MusicStoreEntities db = new MusicStoreEntities();
        const string PromoCode = "FREE";
        //
        // GET: /Default1/

        public ViewResult Index()
        {
            return View(db.Orders.ToList());
        }

        //
        // GET: /Default1/Details/5

        public ViewResult Details(int id)
        {
            // Validate customer owns this order
            bool isValid = db.Orders.Any(
                o => o.OrderId == id &&
                o.Username == User.Identity.Name);

            if (isValid)
            {
                return View(id);
            }
            else
            {
                return View("Error");
            }
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
                    //flush the changes - we need the order id
                    //which is silly
                    db.SaveChanges();

                    //Process the order
                    var cart = ShoppingCart.GetCart(this.HttpContext);
                    cart.CreateOrder(order);
                    

                    
                    //add a note
                    order.Notes = new List<OrderNote>();
                    order.Transactions = new List<Transaction>();
                    order.Notes.Add(new OrderNote { Note = "Preparing Order", CreatedOn = DateTime.Now, OrderId = order.OrderId });
                    
                    //auth the charge...
                    order.Transactions.Add(new Transaction { Processor = "coupon", Authorization = PromoCode, Amount = cart.GetTotal(), CreatedOn = DateTime.Now, Discount = 0, OrderId = order.OrderId });
                    order.Notes.Add(new OrderNote { Note = "Transaction Authorized by Coupon: " + PromoCode, CreatedOn = DateTime.Now, OrderId = order.OrderId });

                    //send a thank you note via email
                    order.Notes.Add(new OrderNote { Note = "Thank You Invoice Email Sent", CreatedOn = DateTime.Now, OrderId = order.OrderId });

                    //save it down
                    db.SaveChanges();
                    return RedirectToAction("Details",
                        new { id = order.OrderId });
                }

            

        } 

        //
        // POST: /Default1/Create

        public ActionResult Create()
        {
            return View();
        }
        
        //
        // GET: /Default1/Edit/5
 
        public ActionResult Edit(int id)
        {
            Order order = db.Orders.Find(id);
            return View(order);
        }

        //
        // POST: /Default1/Edit/5

        [HttpPost]
        public ActionResult Edit(Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(order);
        }

        //
        // GET: /Default1/Delete/5
 
        public ActionResult Delete(int id)
        {
            Order order = db.Orders.Find(id);
            return View(order);
        }

        //
        // POST: /Default1/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}