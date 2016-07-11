using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrinterMonitorLibrary
{
    public class PrinterFeedModel
    {
        public List<OnlinePrinter> onlinePrinters { get; set; }
        public List<OfflinePrinter> offlinePrinters { get; set; }
        public List<LowRibbonPrinter> lowRibbonPrinters { get; set; }
        public List<NoCommunicationPrinter> noCommunicationPrinters { get; set; }
    }
}
