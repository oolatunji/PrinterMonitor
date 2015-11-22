using PrinterMonitorLibrary.ModelLibrary.EntityFrameworkLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrinterMonitorLibrary
{
    public class PrinterPL
    {
        public PrinterPL()
        {

        }

        public static bool Save(Printer printer, out string message)
        {
            try
            {
                if (PrinterDL.PrinterExists(printer))
                {
                    message = string.Format("Printer with serial number: {0} exists already", printer.PrinterSrNo);
                    return false;
                }
                else
                {
                    message = string.Empty;
                    return PrinterDL.Save(printer);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool Update(Printer printer)
        {
            try
            {
                return PrinterDL.Update(printer);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<Object> RetrievePrinters()
        {
            try
            {
                List<Object> returnedPrinters = new List<object>();

                List<Printer> printers = PrinterDL.RetrievePrinters();

                foreach (Printer printer in printers)
                {
                    Object printerObj = new
                    {
                        ID = printer.ID,
                        PrinterSrNo = printer.PrinterSrNo,
                        PrinterName = printer.PrinterName,
                        PrinterBrand = printer.PrinterBrand,
                        Branch = new{ ID = printer.Branch.ID, Name= printer.Branch.Name},
                        DateofEnrollment = String.Format("{0:ddd, MMM d, yyyy}", printer.DateofEnrollment)
                    };

                    returnedPrinters.Add(printerObj);
                }

                return returnedPrinters;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
