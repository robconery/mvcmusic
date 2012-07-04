using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcMusicStore.Models;
using MvcMusicStore.Controllers;
using System.Web.Script.Serialization;

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

        public ActionResult Refund(int id){
            var order = db.Orders.Find(id);

            try
            {
                order.Status = "refunded";
                order.Notes.Add(new OrderNote { Note = "Order Refunded: Authorization XYZ by " + User.Identity.Name, CreatedOn = DateTime.Now });
                order.Transactions.Add(new Transaction { Amount = -order.Total, Authorization = "XYZ by " + User.Identity.Name, CreatedOn = DateTime.Now, Discount = 0, Processor = "coupon" });
                //push the changes, we need the changed object
                db.SaveChanges();
                //reload it
                order = db.Orders
                    .Include("Transactions")
                    .Include("OrderDetails")
                    .Include("Notes")
                    .Where(x => x.OrderId == id).FirstOrDefault(); 
                return Json(new { success = true, message = "Order Refunded", order = order });
            }
            catch
            {
                return Json(new { success = false, message = "Can't refund this order, there was an error", order = order });
            }
            
            //TempData["message"] = "Order Refunded";
            //return RedirectToAction("edit", new {id = id});
        }
        public ActionResult Void(int id)
        {
            var order = db.Orders.Find(id);
            order.Status = "voided";
            order.Notes = new List<OrderNote>();
            order.Notes.Add(new OrderNote { Note = "Order Voidedby " + User.Identity.Name, CreatedOn = DateTime.Now });
            TempData["message"] = "Order Voided by " + User.Identity.Name;
            return RedirectToAction("edit", new { id = id });
        }
        public ActionResult Ship(int id)
        {
            var order = db.Orders.Find(id);
            order.Status = "shipped";
            order.Notes = new List<OrderNote>();
            order.Notes.Add(new OrderNote { Note = "Order Shipped by " + User.Identity.Name, CreatedOn = DateTime.Now });
            TempData["message"] = "Order Shipped by " + User.Identity.Name;
            return RedirectToAction("edit", new { id = id });
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
            
            return View(ShoppingCart.GetCart(db,this.HttpContext));
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
                    
                    //set the status as paid. Simplistic, but will work for now
                    order.Status = "paid";
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

            var serializer = new JavaScriptSerializer();
            ViewBag.OrderJson = serializer.Serialize(order);
            
            return View(order);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(int id, FormCollection form)
        {
            var order = db.Orders.Find(id);
            TryUpdateModel<Order>(order);
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                return Json(new { success = true, message = "Order Saved!" });
            }
            else
            {
                return Json(new { success = false, message = "The information you provided is invalid" });
            }
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