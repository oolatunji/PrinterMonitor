using PrinterMonitorLibrary.ModelLibrary.EntityFrameworkLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrinterMonitorLibrary
{
    public class PrinterFeedsPL
    {
        public static bool Save(PrinterFeed printerFeed)
        {
            try
            {
                return PrinterFeedsDL.Save(printerFeed);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<dynamic> RetrievePrinterFeeds()
        {
            try
            {
                List<dynamic> printerFeeds = new List<dynamic>();

                List<PrinterFeed> feeds = PrinterFeedsDL.RetrievePrinterFeeds();

                Int32 timeToCheckForNoCommunication = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings.Get("TimeToCheckForNoCommunication"));

                var today = System.DateTime.Now;

                foreach(PrinterFeed feed in feeds)
                {
                    Printer printer = PrinterDL.GetPrinterBySerialNumber(feed.PrinterSerialNumber);

                    if (printer != null)
                    {
                        var timeSpan = today.Subtract(Convert.ToDateTime(feed.DateofReport));

                        dynamic printerFeed = new System.Dynamic.ExpandoObject();
                        printerFeed.branchName = printer.Branch.Name;
                        printerFeed.ribbonStatus = feed.RibbonCount;
                        printerFeed.printedCards = feed.CardPrinted;
                        printerFeed.status = feed.Status.Equals(true) ? 1 : 0;
                        printerFeed.dateofReport = String.Format("{0:G}", feed.DateofReport);
                        printerFeed.overDue = timeSpan.Minutes >=timeToCheckForNoCommunication ? 1 : 0; 

                        printerFeeds.Add(printerFeed);
                    }
                }

                return printerFeeds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
