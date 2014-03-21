using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TimetableSystem.Models;

namespace TimetableSystem.Controllers
{
    [Authorize]
    public class ViewController : Controller
    {
        TimetableSystemEntities systemDB = new TimetableSystemEntities();
        //
        // GET: /View/

        public ActionResult Index()
        {
            List<Request> requests = new List<Request>();
            if (systemDB.Requests != null) {
                requests = systemDB.Requests.ToList();
            }
            return View(requests);
        }

        //
        // GET: /View/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }
        

        //
        // GET: /View/Delete/5
 
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /View/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
 
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
