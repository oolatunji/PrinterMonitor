﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PrinterMonitoringApp.Controllers
{
    public class PrinterController : Controller
    {
        // GET: Printer
        public ActionResult Monitor()
        {
            return View();
        }
    }
}