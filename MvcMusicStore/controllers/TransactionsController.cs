using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcMusicStore.Models;
using System.Data;

namespace MvcMusicStore.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class TransactionsController : DbController
    {
        //
        // GET: /Transactions/

        public ActionResult Index()
        {
            
            return View(db.Transactions.ToList());
        }

        //
        // GET: /Transactions/Details/5

        public ActionResult Details(int id)
        {
            return View(db.Transactions.Find(id));
        }

        //
        // GET: /Transactions/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Transactions/Create

        [HttpPost]
        public ActionResult Create(Transaction item)
        {

            if (ModelState.IsValid)
            {
                db.Transactions.Add(item);
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }             

        }
        
        //
        // GET: /Transactions/Edit/5
 
        public ActionResult Edit(int id)
        {
            return View(db.Transactions.Find(id));
        }

        //
        // POST: /Transactions/Edit/5

        [HttpPost]
        public ActionResult Edit(Transaction item)
        {

            if (ModelState.IsValid)
            {
                db.Entry(item).State = EntityState.Modified;
            }

            return View(item);
        }

        //
        // POST: /Transactions/Delete/5

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var item = db.Transactions.Find(id);
            db.Transactions.Remove(item);
            return RedirectToAction("Index");
        }
    }
}
