using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace PrinterFeedsTest
{
    class Program
    {
        public static int GenerateRandom(int min, int max)
        {
            var seed = Convert.ToInt32(Regex.Match(Guid.NewGuid().ToString(), @"\d+").Value);
            return new Random(seed).Next(min, max);
        }

        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("=============================================");
                Console.WriteLine("Printer Feeds Test Service Running.");
                Console.WriteLine("=============================================");
                
                Thread printer1 = new Thread(delegate()
                {
                    FLSService.FLSSolution flsservice = new FLSService.FLSSolution();

                    string printerUID = "90a0818354e001e0efce9d01000000gg";
                    string printerSerialNumber = "ISOSISOI1HY920";
                    int ribbonCount = GenerateRandom(0, 1000);
                    int noOfCardsPrinted = GenerateRandom(0, 500);
                    int online = GenerateRandom(100, 200);
                    bool printerOnline = false;
                    if (online > 150)
                        printerOnline = true;

                    flsservice.SendLatestPrinterFeeds(printerUID, printerSerialNumber, ribbonCount, noOfCardsPrinted, printerOnline);
                });

                Thread printer2 = new Thread(delegate()
                {
                    FLSService.FLSSolution flsservice = new FLSService.FLSSolution();

                    string printerUID = "89a0818354e001e0efce9d01000000ff";
                    string printerSerialNumber = "IOEIEOIEIP1011";
                    int ribbonCount = GenerateRandom(0, 1000);
                    int noOfCardsPrinted = GenerateRandom(0, 500);
                    int online = GenerateRandom(100, 200);
                    bool printerOnline = false;
                    if (online > 150)
                        printerOnline = true;

                    flsservice.SendLatestPrinterFeeds(printerUID, printerSerialNumber, ribbonCount, noOfCardsPrinted, printerOnline);
                });

                Thread printer3 = new Thread(delegate()
                {
                    FLSService.FLSSolution flsservice = new FLSService.FLSSolution();

                    string printerUID = "60a0818354e001e0efce9d01000000ff";
                    string printerSerialNumber = "ABCSISOI1HY120";
                    int ribbonCount = GenerateRandom(0, 1000);
                    int noOfCardsPrinted = GenerateRandom(0, 500);
                    int online = GenerateRandom(100, 200);
                    bool printerOnline = false;
                    if (online > 150)
                        printerOnline = true;

                    flsservice.SendLatestPrinterFeeds(printerUID, printerSerialNumber, ribbonCount, noOfCardsPrinted, printerOnline);
                });

                printer1.IsBackground = true;
                printer1.Start();

                printer2.IsBackground = true;
                printer2.Start();

                printer3.IsBackground = true;
                printer3.Start();

                Thread.Sleep(60000);
            }
            
        }
    }
}
