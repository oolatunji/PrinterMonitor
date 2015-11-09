using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PrinterMonitoringApp.Controllers
{
    public class SystemController : Controller
    {
        // GET: System
        public ActionResult Configuration()
        {
            return View();
        }

        public ActionResult SystemConfiguration()
        {
            return View();
        }
    }
}