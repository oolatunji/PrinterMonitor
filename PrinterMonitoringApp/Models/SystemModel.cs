using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PrinterMonitoringApp
{
    public class SystemModel
    {
        [Required]
        public string WebsiteUrl { get; set; }
        [Required]
        public string Organization { get; set; }
        [Required]
        public string ApplicationName { get; set; }
        [Required]
        public Int32 LowRibbonThreshold { get; set; }
        [Required]
        public string DatabaseServer { get; set; }
        [Required]
        public string DatabaseName { get; set; }
        [Required]
        public string DatabaseUser { get; set; }
        [Required]
        public string DatabasePassword { get; set; }
        [Required]
        public string FromEmailAddress { get; set; }
        [Required]
        public string SmtpUsername { get; set; }
        [Required]
        public string SmtpPassword { get; set; }
        [Required]
        public string SmtpHost { get; set; }
        [Required]
        public string SmtpPort { get; set; }
        [Required]
        public bool UseSmartCardAuthentication { get; set; }
        [Required]
        public Int32 PrinterFeedsPollingTime { get; set; }
        [Required]
        public Int32 TimeToCheckForNoCommunication { get; set; }
    }
}