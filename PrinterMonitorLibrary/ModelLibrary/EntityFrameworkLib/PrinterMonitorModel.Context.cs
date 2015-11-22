﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PrinterMonitorLibrary.ModelLibrary.EntityFrameworkLib
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class PrinterMonitorDBEntities : DbContext
    {
        public PrinterMonitorDBEntities()
            : base("name=PrinterMonitorDBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Branch> Branches { get; set; }
        public virtual DbSet<CHECK_WEBTOKEN> CHECK_WEBTOKEN { get; set; }
        public virtual DbSet<Function> Functions { get; set; }
        public virtual DbSet<Printer> Printers { get; set; }
        public virtual DbSet<PrinterFeed> PrinterFeeds { get; set; }
        public virtual DbSet<RoleFunction> RoleFunctions { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<SmartCard> SmartCards { get; set; }
        public virtual DbSet<User> Users { get; set; }
    
        public virtual ObjectResult<Nullable<decimal>> sp_insert_printer_feeds(string printerUID, string printerSerialNumber, Nullable<int> ribbonCount, Nullable<int> cardPrinted, Nullable<bool> status, Nullable<System.DateTime> dateofReport)
        {
            var printerUIDParameter = printerUID != null ?
                new ObjectParameter("PrinterUID", printerUID) :
                new ObjectParameter("PrinterUID", typeof(string));
    
            var printerSerialNumberParameter = printerSerialNumber != null ?
                new ObjectParameter("PrinterSerialNumber", printerSerialNumber) :
                new ObjectParameter("PrinterSerialNumber", typeof(string));
    
            var ribbonCountParameter = ribbonCount.HasValue ?
                new ObjectParameter("RibbonCount", ribbonCount) :
                new ObjectParameter("RibbonCount", typeof(int));
    
            var cardPrintedParameter = cardPrinted.HasValue ?
                new ObjectParameter("CardPrinted", cardPrinted) :
                new ObjectParameter("CardPrinted", typeof(int));
    
            var statusParameter = status.HasValue ?
                new ObjectParameter("Status", status) :
                new ObjectParameter("Status", typeof(bool));
    
            var dateofReportParameter = dateofReport.HasValue ?
                new ObjectParameter("DateofReport", dateofReport) :
                new ObjectParameter("DateofReport", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<decimal>>("sp_insert_printer_feeds", printerUIDParameter, printerSerialNumberParameter, ribbonCountParameter, cardPrintedParameter, statusParameter, dateofReportParameter);
        }
    }
}
