using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PrinterMonitoringApp
{
    public class SmartCardModel
    {
        public long SmartCardID { get; set; }
        public long UserID { get; set; }
        public bool Status { get; set; }
    }
}