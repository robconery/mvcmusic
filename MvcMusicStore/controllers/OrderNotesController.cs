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
        public ActionResult Create(FormCollection form)
        {
            var note = new OrderNote();
            TryUpdateModel<OrderNote>(note);
            try
            {
                db.OrderNotes.Add(note);
                var notes = db.OrderNotes.Where(x => x.OrderId == note.OrderId).OrderBy(x => x.CreatedOn);
                return Json(new { success = true, message = "Note added", notes = notes });
            }
            catch
            {

                return Json(new { success = true, message = "There was an error adding this note"});
            }
        }
        
        public ActionResult Edit(int id)
        {
            return View(db.OrderNotes.Find(id));
        }


        [HttpPost]
        public ActionResult Edit(int Id, FormCollection form)
        {
            var item = db.OrderNotes.Find(Id);
            TryUpdateModel<OrderNote>(item);
            var notes = db.OrderNotes.Where(x => x.OrderId == item.OrderId).OrderBy(x => x.CreatedOn);
            return Json(new { success = true, message = "Note updated", notes = notes });
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var item = db.OrderNotes.Find(id);
            db.OrderNotes.Remove(item);
            return Json(new { success = true, message = "Note deleted" });
        }
    }
}
