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

        static List<string> Drivers()
        {
            var drivers = new List<string>();
            
            drivers.Add("Evolis Primacy");
            drivers.Add("Evolis Elypso");
            drivers.Add("Evolis Zenius");
            drivers.Add("Evolis KC200");

            return drivers;
        }

        static void Main(string[] args)
        {
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Green;
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


                    int cursor = GenerateRandom(0, 3);
                    string printerType = Drivers().ElementAtOrDefault(cursor);

                    flsservice.SendLatestPrinterFeeds(printerUID, printerSerialNumber, ribbonCount, noOfCardsPrinted, printerOnline, printerType);

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Status sent from printer 1");
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

                    int cursor = GenerateRandom(0, 3);
                    string printerType = Drivers().ElementAtOrDefault(cursor);

                    flsservice.SendLatestPrinterFeeds(printerUID, printerSerialNumber, ribbonCount, noOfCardsPrinted, printerOnline, printerType);

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Status sent from printer 2");
                });

                Thread printer3 = new Thread(delegate()
                {
                    FLSService.FLSSolution flsservice = new FLSService.FLSSolution();

                    string printerUID = "60a0818354e001e0efce9d01000000ff";
                    string printerSerialNumber = "ABCSISOI1HY120";
                    int ribbonCount = GenerateRandom(0, 1000);
                    int noOfCardsPrinted = GenerateRandom(0, 500);
                    bool printerOnline = true;

                    int cursor = GenerateRandom(0, 3);
                    string printerType = Drivers().ElementAtOrDefault(cursor);

                    flsservice.SendLatestPrinterFeeds(printerUID, printerSerialNumber, ribbonCount, noOfCardsPrinted, printerOnline, printerType);

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Status sent from printer 3");
                });


                Thread printer4 = new Thread(delegate()
                {
                    FLSService.FLSSolution flsservice = new FLSService.FLSSolution();

                    string printerUID = "78a1902854e039e0efce9d00398000ff";
                    string printerSerialNumber = "TATSISO9087120";
                    int noOfCardsPrinted = GenerateRandom(0, 500);
                    int online = GenerateRandom(100, 200);
                    bool printerOnline = false;
                    if (online > 150)
                        printerOnline = true;

                    string printerType = "Tattoo2";

                    flsservice.SendLatestPrinterFeeds(printerUID, printerSerialNumber, 0, noOfCardsPrinted, printerOnline, printerType);

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Status sent from printer 4");
                });

                printer1.IsBackground = true;
                printer1.Start();

                printer2.IsBackground = true;
                printer2.Start();

                printer3.IsBackground = true;
                printer3.Start();

                printer4.IsBackground = true;
                printer4.Start();

                //Console.Write(String.Format("{0:dMyyyyHHmmss}", System.DateTime.Now));

                Thread.Sleep(60000);
            }
            
        }
    }
}
