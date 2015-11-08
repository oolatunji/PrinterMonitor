using PrinterMonitorLibrary.ModelLibrary.EntityFrameworkLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PrinterMonitoringApp
{
    public class TokenResponse : Response
    {
        public CHECK_WEBTOKEN token { get; set; }
    }
}