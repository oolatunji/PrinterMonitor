using PrinterMonitorLibrary;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Configuration;
using System.Web.Http;

namespace PrinterMonitoringApp.Controllers
{
    public class SystemAPIController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage ConfigureSystem([FromBody]SystemModel systemModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string errMsg = "";

                    //Configure JS File used by all APIs (configFile.js)
                    string configFilepath = System.Web.Hosting.HostingEnvironment.MapPath(@"~/Scripts/Utility/configFile.js");
                    string jsSettings = "var settingsManager = {\"websiteURL\": \"" + systemModel.WebsiteUrl + "\"};";

                    var lines = File.ReadAllLines(configFilepath);
                    lines[0] = jsSettings;
                    File.WriteAllLines(configFilepath, lines);


                    var configuration = WebConfigurationManager.OpenWebConfiguration("~");

                    var appSettingsSection = (AppSettingsSection)configuration.GetSection("appSettings");
                    appSettingsSection.Settings["Organization"].Value = systemModel.Organization;
                    appSettingsSection.Settings["ApplicationName"].Value = systemModel.ApplicationName;
                    appSettingsSection.Settings["WebsiteUrl"].Value = systemModel.WebsiteUrl;

                    
                    var section = (ConnectionStringsSection)configuration.GetSection("connectionStrings");
                    section.ConnectionStrings["PrinterMonitorDBEntities"].ConnectionString = GetConnectionString(systemModel.DatabaseServer, systemModel.DatabaseName, systemModel.DatabaseUser, systemModel.DatabasePassword);

                    var mailHelperSection = (MailHelper)configuration.GetSection("mailHelperSection");
                    mailHelperSection.Mail.FromEmailAddress = systemModel.FromEmailAddress;
                    mailHelperSection.Mail.Username = systemModel.SmtpUsername;
                    mailHelperSection.Mail.Password = systemModel.SmtpPassword;

                    mailHelperSection.Smtp.Host = systemModel.SmtpHost;
                    mailHelperSection.Smtp.Port = systemModel.SmtpPort;

                    configuration.Save();


                    bool result = true;

                    if (string.IsNullOrEmpty(errMsg))
                        return result.Equals(true) ? Request.CreateResponse(HttpStatusCode.OK, "Successful") : Request.CreateResponse(HttpStatusCode.BadRequest, "Request failed");
                    else
                    {
                        var response = Request.CreateResponse(HttpStatusCode.BadRequest, errMsg);
                        return response;
                    }
                }
                else
                {
                    string errors = ModelStateValidation.GetErrorListFromModelState(ModelState);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, errors);
                }
            }
            catch (Exception ex)
            {
                var response = Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
                return response;
            }
        }

        [HttpGet]
        public HttpResponseMessage GetSystemSettings()
        {
            try
            {
                object systemSettings = SystemSettings();
                return Request.CreateResponse(HttpStatusCode.OK, systemSettings);  
            }
            catch (Exception ex)
            {
                var response = Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
                return response;
            }
        }

        Object SystemSettings()
        {
            Object systemSettings = new object();

            var configuration = WebConfigurationManager.OpenWebConfiguration("~");

            var appSettingsSection = (AppSettingsSection)configuration.GetSection("appSettings");
            string organization = appSettingsSection.Settings["Organization"].Value;
            string applicationName = appSettingsSection.Settings["ApplicationName"].Value;
            string websiteUrl = appSettingsSection.Settings["WebsiteUrl"].Value;

            Object generalSettings = new
            {
                Organization = organization,
                ApplicationName = applicationName,
                ApplicationUrl = websiteUrl
            };

            var connectionStringsSection = (ConnectionStringsSection)configuration.GetSection("connectionStrings");

            Object databaseSettings = ConnectionStringParser(connectionStringsSection.ConnectionStrings["PrinterMonitorDBEntities"].ConnectionString);

            var mailHelperSection = (MailHelper)configuration.GetSection("mailHelperSection");
            string fromEmailAddress = mailHelperSection.Mail.FromEmailAddress;
            string smtpUsername = mailHelperSection.Mail.Username;
            string smtpPassword = mailHelperSection.Mail.Password;
            string smtpHost = mailHelperSection.Smtp.Host;
            string smtpPort = mailHelperSection.Smtp.Port;

            Object mailSettings = new
            {
                SmtpHost = smtpHost,
                SmtpPort = smtpPort,
                SmtpUsername = smtpUsername,
                SmtpPassword = smtpPassword,
                FromEmailAddress = fromEmailAddress,
            };

            systemSettings = new
            {
                GeneralSettings = generalSettings,
                DatabaseSettings = databaseSettings,
                MailSettings = mailSettings
            };

            return systemSettings;
        }
        
        string GetConnectionString(string databaseServerInstance, string databaseName, string databaseUser, string databasePassword)
        {
            string connectionString = string.Format("metadata=res://*/ModelLibrary.EntityFrameworkLib.PrinterMonitorModel.csdl|res://*/ModelLibrary.EntityFrameworkLib.PrinterMonitorModel.ssdl|res://*/ModelLibrary.EntityFrameworkLib.PrinterMonitorModel.msl;provider=System.Data.SqlClient;provider connection string='data source={0};initial catalog={1};user id={2};password={3};MultipleActiveResultSets=True;App=EntityFramework'", databaseServerInstance, databaseName, databaseUser, databasePassword);

            return connectionString;
        }

        Object ConnectionStringParser(string connectionString)
        {
            Object connection = new object();

            string[] providerConnectionString = connectionString.Split('=');

            connection = new
            {
                DatabaseServer = providerConnectionString[4].Split(';')[0],
                DatabaseName = providerConnectionString[5].Split(';')[0],
                DatabaseUser = providerConnectionString[6].Split(';')[0],
                DatabasePassword = providerConnectionString[7].Split(';')[0]
            };
            
            return connection;
        }
    }
}
