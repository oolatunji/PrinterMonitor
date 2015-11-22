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
    }
}
