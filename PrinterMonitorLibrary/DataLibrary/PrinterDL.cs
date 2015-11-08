using PrinterMonitorLibrary.ModelLibrary.EntityFrameworkLib;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrinterMonitorLibrary
{
    public class PrinterDL
    {
        public PrinterDL()
        {

        }

        public static bool Save(Printer printer)
        {
            try
            {                                
                using (var context = new PrinterMonitorDBEntities())
                {
                    context.Printers.Add(printer);
                    context.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool PrinterExists(Printer printer)
        {
            try
            {
                var existingPrinter = new Printer();
                using (var context = new PrinterMonitorDBEntities())
                {
                    existingPrinter = context.Printers
                                    .Include("Branch")
                                    .Where(t => t.PrinterUID.Equals(printer.PrinterUID))
                                    .FirstOrDefault();
                }

                if (existingPrinter == null)
                    return false;
                else
                    return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<Printer> RetrievePrinters()
        {
            try
            {
                using (var context = new PrinterMonitorDBEntities())
                {
                    var printers = context.Printers
                                    .Include("Branch")
                                    .ToList();

                    return printers;
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
                Printer existingPrinter = new Printer();
                using (var context = new PrinterMonitorDBEntities())
                {
                    existingPrinter = context.Printers
                                    .Include("Branch")
                                    .Where(t => t.ID == printer.ID)
                                    .FirstOrDefault();
                }

                if (existingPrinter != null)
                {
                    existingPrinter.PrinterUID = printer.PrinterUID;
                    existingPrinter.PrinterSrNo = printer.PrinterSrNo;
                    existingPrinter.PrinterName = printer.PrinterName;
                    existingPrinter.PrinterBrand = printer.PrinterBrand;
                    existingPrinter.BranchID = printer.BranchID;

                    using (var context = new PrinterMonitorDBEntities())
                    {
                        context.Entry(existingPrinter).State = EntityState.Modified;

                        context.SaveChanges();
                    }

                    return true;
                }
                else
                {
                    return false;
                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
