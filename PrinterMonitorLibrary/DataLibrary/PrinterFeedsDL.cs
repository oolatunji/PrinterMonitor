using PrinterMonitorLibrary.ModelLibrary.EntityFrameworkLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrinterMonitorLibrary
{
    public class PrinterFeedsDL
    {
        public static bool Save(PrinterFeed printerFeed)
        {
            try
            {
                using (var context = new PrinterMonitorDBEntities())
                {
                    context.sp_insert_printer_feeds(printerFeed.PrinterUID, printerFeed.PrinterSerialNumber, printerFeed.RibbonCount, printerFeed.CardPrinted, printerFeed.Status, printerFeed.DateofReport);

                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<PrinterFeed> RetrievePrinterFeeds()
        {
            try
            {
                using (var context = new PrinterMonitorDBEntities())
                {
                    var printerFeeds = context.PrinterFeeds.Where(p => p.LatestFeed == true).ToList();

                    return printerFeeds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
