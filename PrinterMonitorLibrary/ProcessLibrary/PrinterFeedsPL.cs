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

        public static PrinterFeedModel RetrievePrinterFeeds()
        {
            try
            {
                var lowRibbonThreshold = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings.Get("LowRibbonThreshold"));

                var printerFeedModel = new PrinterFeedModel();
                printerFeedModel.onlinePrinters = new List<OnlinePrinter>();
                printerFeedModel.offlinePrinters = new List<OfflinePrinter>();
                printerFeedModel.noCommunicationPrinters = new List<NoCommunicationPrinter>();
                printerFeedModel.lowRibbonPrinters = new List<LowRibbonPrinter>();

                List<PrinterFeed> feeds = PrinterFeedsDL.RetrievePrinterFeeds();

                Int32 timeToCheckForNoCommunication = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings.Get("TimeToCheckForNoCommunication"));

                var today = System.DateTime.Now;

                foreach(PrinterFeed feed in feeds)
                {
                    Printer printer = PrinterDL.GetPrinterBySerialNumber(feed.PrinterSerialNumber);

                    if (printer != null  && printer.ID != 0)
                    {
                        var timeSpan = today.Subtract(Convert.ToDateTime(feed.DateofReport));

                        if (timeSpan.Minutes >= timeToCheckForNoCommunication)
                        {
                            var ncp = new NoCommunicationPrinter
                            {
                                branchName = printer.Branch.Name,
                                ribbonCount = "NA",
                                printedCards = "NA",
                                dateofReport = String.Format("{0:G}", feed.DateofReport)
                            };

                            printerFeedModel.noCommunicationPrinters.Add(ncp);
                        }
                        else
                        {
                            var printerOnline = Convert.ToBoolean(feed.Status);
                            if(!printerOnline)
                            {
                                var printerOffline = new OfflinePrinter
                                {
                                    branchName = printer.Branch.Name,
                                    ribbonCount = "NA",
                                    printedCards = "NA",
                                    dateofReport = String.Format("{0:G}", feed.DateofReport)
                                };

                                printerFeedModel.offlinePrinters.Add(printerOffline);
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(feed.PrinterType) && feed.PrinterType.Contains("Tattoo"))
                                {
                                    var onlinePrinters = new OnlinePrinter
                                    {
                                        branchName = printer.Branch.Name,
                                        ribbonCount = "NA",
                                        printedCards = feed.CardPrinted.ToString(),
                                        dateofReport = String.Format("{0:G}", feed.DateofReport)
                                    };

                                    printerFeedModel.onlinePrinters.Add(onlinePrinters);
                                }
                                else
                                {
                                    if (feed.RibbonCount <= lowRibbonThreshold)
                                    {
                                        var lowRibbonPrinters = new LowRibbonPrinter
                                        {
                                            branchName = printer.Branch.Name,
                                            ribbonCount = feed.RibbonCount.ToString(),
                                            printedCards = feed.CardPrinted.ToString(),
                                            dateofReport = String.Format("{0:G}", feed.DateofReport)
                                        };

                                        printerFeedModel.lowRibbonPrinters.Add(lowRibbonPrinters);
                                    }
                                    else
                                    {
                                        var onlinePrinters = new OnlinePrinter
                                        {
                                            branchName = printer.Branch.Name,
                                            ribbonCount = feed.RibbonCount.ToString(),
                                            printedCards = feed.CardPrinted.ToString(),
                                            dateofReport = String.Format("{0:G}", feed.DateofReport)
                                        };

                                        printerFeedModel.onlinePrinters.Add(onlinePrinters);
                                    }
                                }
                            }
                        }
                    }
                }

                return printerFeedModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
