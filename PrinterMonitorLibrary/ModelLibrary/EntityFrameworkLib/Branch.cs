//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    
    public partial class Branch
    {
        public Branch()
        {
            this.Printers = new HashSet<Printer>();
            this.Users = new HashSet<User>();
        }
    
        public long ID { get; set; }
        [Required]
        public string Name { get; set; }
        public string Code { get; set; }
        [Required]
        public string Address { get; set; }
    
        public virtual ICollection<Printer> Printers { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
