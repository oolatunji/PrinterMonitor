using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PrinterMonitoringApp.RealTimeHubs
{
    [HubName("PrinterMonitoring")]
    public class PrinterTrackingHub : Hub
    {
       

        public void GetLatestPrinterStatus()
        {
            List<PrinterDetails> pds = new List<PrinterDetails>();
            Random rand = new Random();
            Random status = new Random();
            Random maxRand = new Random();
            long max = maxRand.Next(0, 20);
            long max2 = maxRand.Next(20, 50);
            for (int i = 1; i <= 600; i++)
            {
                PrinterDetails pd = new PrinterDetails();
                pd.Name = "Branch " + i.ToString();
                pd.PrintedCards = rand.Next(i);
                if (i <= max)
                    pd.PrinterStatus = 0;
                else
                    pd.PrinterStatus = 1;
                if (i <= max2)
                    pd.RibbonStatus = rand.Next(0, 41);
                else
                    pd.RibbonStatus = rand.Next(41, 100);
                pds.Add(pd);
            }
            //Sending the server time to all the connected clients on the client method SendServerTime()            
            Clients.All.LatestPrinterStatus(pds);
        }

        public void Send()
        {
            Clients.All.hello("Toni olatunji");
        }
    }

    public class PrinterDetails
    {
        [JsonProperty("branchName")]
        public string Name { get; set; }

        [JsonProperty("ribbonStatus")]
        public double RibbonStatus { get; set; }

        [JsonProperty("printedCards")]
        public long PrintedCards { get; set; }

        [JsonProperty("status")]
        public long PrinterStatus { get; set; }
    }
}