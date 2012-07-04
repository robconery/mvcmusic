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
    public class OrderNotesController : DbController
    {

        public ActionResult Index()
        {
            return View(db.OrderNotes.ToList());
        }


        public ActionResult Details(int id)
        {
            return View(db.OrderNotes.Find(id));
        }

        public ActionResult Create(int id)
        {
            var order = db.Orders.Find(id);
            return View(order);
        } 

        [HttpPost]
        public ActionResult Create(OrderNote item)
        {
            if (ModelState.IsValid)
            {
                db.OrderNotes.Add(item);
                item.CreatedOn = DateTime.Now;
                return Json(new { success = true, message = "Note created", newNote = item});
            }
            else
            {
                return Json(new { success = false, message = "Note invalid" });
            }       
        }
        
        public ActionResult Edit(int id)
        {
            return View(db.OrderNotes.Find(id));
        }


        [HttpPost]
        public ActionResult Edit(OrderNote item)
        {
            if (ModelState.IsValid)
            {
                db.Entry(item).State = EntityState.Modified;
            }

            return RedirectToAction("edit", "orders", new { id = item.OrderId });
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var item = db.OrderNotes.Find(id);
            try
            {
                db.OrderNotes.Remove(item);
                return Json(new { success = true, message = "Note deleted" });
            }
            catch (Exception x)
            {
                return Json(new { success = false, message = "Can't delete order... there was an error" });
            }
        }
    }
}
