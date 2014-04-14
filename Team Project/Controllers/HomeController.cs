using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using TimetableSystem.Models;
using System.Text;
using System.Reflection;

namespace TimetableSystem.Controllers
{
    [HandleError]
    [Authorize]
    public class HomeController : Controller
    {
        private TimetableSystemEntities db = new TimetableSystemEntities();

        public ActionResult Index()
        {
            return CreateForm();
        }

        [HttpPost]
        public ActionResult Index(Request request)
        {
            List<RequestWeek> weeks = new List<RequestWeek>();
            if (request.Weeks != null)
            {
                foreach (var week in request.Weeks)
                {
                    RequestWeek rw = new RequestWeek(Convert.ToInt16(week));
                    weeks.Add(rw);
                }
            }
            request.RequestWeeks = weeks;

            if (ModelState.IsValid)
            {
                db.Requests.Add(request);
                db.SaveChanges();
                return View();
            }
            else
            {
                Response.Write(DisplayObjectInfo(request));
                Response.End();
            }

            return CreateForm();
        }

        public static string DisplayObjectInfo(Object o)
        {
           StringBuilder sb = new StringBuilder();

           // Include the type of the object
           System.Type type = o.GetType();
           sb.Append("Type: " + type.Name);

           // Include information for each Field
           sb.Append("\r\n\r\nFields:");
           System.Reflection.FieldInfo[] fi = type.GetFields();
           if (fi.Length > 0)
            {
              foreach (FieldInfo f in fi)
              {
                 sb.Append("\r\n " + f.ToString() + " = " +
                           f.GetValue(o));
              }
           }
           else
              sb.Append("\r\n None");

           // Include information for each Property
           sb.Append("\r\n\r\nProperties:");
           System.Reflection.PropertyInfo[] pi = type.GetProperties();
           if (pi.Length > 0)
           {
              foreach (PropertyInfo p in pi)
              {
                 sb.Append("\r\n " + p.ToString() + " = " +
                           p.GetValue(o, null));
              }
           }
           else
              sb.Append("\r\n None");

           return sb.ToString();
        }


        private ActionResult CreateForm()
        {
            ViewBag.Title = "Create new Request";

            ViewBag.Modules = new SelectList(db.Modules, "ModuleCode", "ModuleTitle");


            ViewBag.WeekCheckboxes = "";
            for (int x = 1; x <= 13; x++)
            {
                ViewBag.WeekCheckboxes += "<label>" + x + " <input type='checkbox' name='Weeks' value='" + x + "' /></label>";
            }

            return View();
        }

    }
}
