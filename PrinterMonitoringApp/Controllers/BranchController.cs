using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PrinterMonitoringApp.Controllers
{
    public class BranchController : Controller
    {
        // GET: Branch
        public ActionResult AddBranch()
        {
            return View();
        }

        public ActionResult ViewBranch()
        {
            return View();
        }
    }
}