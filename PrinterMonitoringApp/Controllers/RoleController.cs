using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PrinterMonitoringApp.Controllers
{
    public class RoleController : Controller
    {
        // GET: Role
        public ActionResult AddRole()
        {
            return View();
        }

        public ActionResult ViewRole()
        {
            return View();
        }
    }
}