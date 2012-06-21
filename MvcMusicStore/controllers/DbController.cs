using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcMusicStore.Models;

namespace MvcMusicStore.Controllers
{
    public class DbController : Controller
    {
        //
        // GET: /Db/
        public MusicStoreEntities db { get; set; }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            this.db = new MusicStoreEntities();
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            this.db.SaveChanges();
        }
        protected override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            this.db.Dispose();
        }
    }
}
