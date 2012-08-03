using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity;
using MvcMusicStore.Models;

namespace MvcMusicStore.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class OrderDetailsController : DbController
    {

        public ActionResult Index()
        {
            return View(db.OrderDetails.ToList());
        }


        public ActionResult Details(int id)
        {
            return View(db.OrderDetails.Find(id));
        }

        [HttpPost]
        public ActionResult Create(OrderDetail item)
        {
            if (ModelState.IsValid)
            {
                db.OrderDetails.Add(item);
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }       
        }
        
        public ActionResult Edit(int id)
        {
            return View(db.OrderDetails.Find(id));
        }


        [HttpPost]
        public ActionResult Edit(OrderDetail item)
        {
            if (ModelState.IsValid)
            {
                db.Entry(item).State = EntityState.Modified;
                var order = db.Orders.Find(item.OrderId);
                order.Notes.Add(new OrderNote { Note = "Order item " + item.Sku + " changed by " + User.Identity.Name });
                return RedirectToAction("edit", "orders", new { id = order.OrderId });
            }

            return View(item);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var item = db.OrderDetails.Find(id);
            db.OrderDetails.Remove(item);
            var order = db.Orders.Find(item.OrderId);
            order.Notes.Add(new OrderNote { Note = "Order item " + item.Sku + " removed by " + User.Identity.Name });
            return RedirectToAction("edit", "orders", new { id = order.OrderId });
        }
    }
}
