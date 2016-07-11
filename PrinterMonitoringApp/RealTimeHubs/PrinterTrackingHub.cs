using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PrinterMonitorLibrary;

namespace PrinterMonitoringApp.RealTimeHubs
{
    [HubName("PrinterMonitoring")]
    public class PrinterTrackingHub : Hub
    {
        public void GetLatestPrinterStatus()
        {
            var pds = PrinterFeedsPL.RetrievePrinterFeeds();

            Clients.All.LatestPrinterStatus(pds);
        }

        public void Send()
        {
            Clients.All.hello("Toni olatunji");
        }
    }
}