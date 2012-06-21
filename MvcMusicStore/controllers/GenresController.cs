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
    public class GenresController : DbController
    {

        public ActionResult Index(string genre="")
        {

            return View(db.Genres.ToList());
        }


        public ActionResult Details(int id)
        {
            var genre = db.Genres.Include("Albums").Where(x => x.GenreId == id).FirstOrDefault();
            return View(genre);
        }

        public ActionResult Create()
        {
            return View();
        } 

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult Create(Genre item)
        {
            if (ModelState.IsValid)
            {
                db.Genres.Add(item);
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }       
        }
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(int id)
        {
            return View(db.Genres.Find(id));
        }


        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(Genre item)
        {
            if (ModelState.IsValid)
            {
                db.Entry(item).State = EntityState.Modified;
            }

            return View(item);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(int id)
        {
            var item = db.Genres.Find(id);
            db.Genres.Remove(item);
            return RedirectToAction("Index");
        }
        [ChildActionOnly]
        public ActionResult GenreMenu()
        {
            var genres = db.Genres.ToList();

            return PartialView(genres);
        }
    }
}
