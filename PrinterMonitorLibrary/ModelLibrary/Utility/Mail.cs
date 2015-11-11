using PrinterMonitorLibrary.ModelLibrary.EntityFrameworkLib;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PrinterMonitorLibrary
{
    public class Mail
    {
        public static void SendNewUserMail(User user)
        {
            try
            {
                Role role = RolePL.RetrieveRoleByID(user.UserRole);
                
                string userFullName = user.Lastname + " " + user.Othernames;
                string userUsername = user.Username;
                string userPassword = user.HashedPassword;
                string userRole = role.Name;
                string organization = System.Configuration.ConfigurationManager.AppSettings.Get("Organization");
                string applicationName = System.Configuration.ConfigurationManager.AppSettings.Get("ApplicationName");
                string websiteUrl = System.Configuration.ConfigurationManager.AppSettings.Get("WebsiteUrl");
                string subject = "Welcome to " + applicationName;
                string userFunction = "";
                
                foreach(RoleFunction roleFunction in role.RoleFunctions)
                {
                    userFunction += roleFunction.Function.Name + "<br/>";
                }

                string fromAddress = "";
                string smtpUsername = "";
                string smtpPassword = "";
                string smtpHost = "";
                Int32 smtpPort = 587;
                bool smtpUseDefaultCredentials = false;
                bool smtpEnableSsl = true;

                MailHelper mailConfig = ConfigurationManager.GetSection("mailHelperSection") as MailHelper;
                if (mailConfig != null && mailConfig.Mail != null)
                {
                    fromAddress = mailConfig.Mail.FromEmailAddress;
                    smtpUsername = mailConfig.Mail.Username;
                    smtpPassword = mailConfig.Mail.Password;
                }

                if (mailConfig != null && mailConfig.Smtp != null)
                {
                    smtpHost = mailConfig.Smtp.Host;
                    smtpPort = Convert.ToInt32(mailConfig.Smtp.Port);
                    smtpUseDefaultCredentials = Convert.ToBoolean(mailConfig.Smtp.UseDefaultCredentials);
                    smtpEnableSsl = Convert.ToBoolean(mailConfig.Smtp.EnableSsl);
                }


                string body = "";

                body = System.IO.File.ReadAllText(System.Web.Hosting.HostingEnvironment.MapPath(@"~/App_Data/MailTemplates/NewUser.txt"));
                body = body.Replace("#Organization", organization);
                body = body.Replace("#ApplicationName", applicationName);
                body = body.Replace("#UserFullName", userFullName);
                body = body.Replace("#Username", userUsername);
                body = body.Replace("#Password", userPassword);
                body = body.Replace("#Role", userRole);
                body = body.Replace("#UserFunctions", userFunction);
                body = body.Replace("#WebsiteUrl", websiteUrl);

                Thread email = new Thread(delegate()
                {
                    Mail.SendMail(user.Email, fromAddress, subject, body, smtpHost, smtpPort, smtpUseDefaultCredentials, smtpUsername, smtpPassword, smtpEnableSsl);
                    
                });

                email.IsBackground = true;
                email.Start();

            }
            catch (Exception ex)
            {
                ErrorHandler.WriteError(ex);
                throw ex;
            }

        }

        public static void SendForgotPasswordMail(User user)
        {
            try
            {
                string key = System.Configuration.ConfigurationManager.AppSettings.Get("ekey");
                string encrypted_username = Crypter.Encrypt(key, user.Username);

                string userFullName = user.Lastname + " " + user.Othernames;
                
                string organization = System.Configuration.ConfigurationManager.AppSettings.Get("Organization");
                string applicationName = System.Configuration.ConfigurationManager.AppSettings.Get("ApplicationName");
                string websiteUrl = System.Configuration.ConfigurationManager.AppSettings.Get("WebsiteUrl");
                string passwordResetUrl = websiteUrl + "User/ResetPassword?rq=" + encrypted_username; ;
                string subject = "Password Reset Request on " + applicationName;
               
                string fromAddress = "";
                string smtpUsername = "";
                string smtpPassword = "";
                string smtpHost = "";
                Int32 smtpPort = 587;
                bool smtpUseDefaultCredentials = false;
                bool smtpEnableSsl = true;

                MailHelper mailConfig = ConfigurationManager.GetSection("mailHelperSection") as MailHelper;
                if (mailConfig != null && mailConfig.Mail != null)
                {
                    fromAddress = mailConfig.Mail.FromEmailAddress;
                    smtpUsername = mailConfig.Mail.Username;
                    smtpPassword = mailConfig.Mail.Password;
                }

                if (mailConfig != null && mailConfig.Smtp != null)
                {
                    smtpHost = mailConfig.Smtp.Host;
                    smtpPort = Convert.ToInt32(mailConfig.Smtp.Port);
                    smtpUseDefaultCredentials = Convert.ToBoolean(mailConfig.Smtp.UseDefaultCredentials);
                    smtpEnableSsl = Convert.ToBoolean(mailConfig.Smtp.EnableSsl);
                }


                string body = "";

                body = System.IO.File.ReadAllText(System.Web.Hosting.HostingEnvironment.MapPath(@"~/App_Data/MailTemplates/ForgotPassword.txt"));
                body = body.Replace("#Organization", organization);
                body = body.Replace("#ApplicationName", applicationName);
                body = body.Replace("#UserFullName", userFullName);
                body = body.Replace("#WebsiteUrl", websiteUrl);
                body = body.Replace("#PasswordResetUrl", passwordResetUrl);

                Thread email = new Thread(delegate()
                {
                    Mail.SendMail(user.Email, fromAddress, subject, body, smtpHost, smtpPort, smtpUseDefaultCredentials, smtpUsername, smtpPassword, smtpEnableSsl);

                });

                email.IsBackground = true;
                email.Start();

            }
            catch (Exception ex)
            {
                ErrorHandler.WriteError(ex);
                throw ex;
            }

        }

        public static void SendMail(string toAddress, string fromAddress, string subject, string body, string smtpHost, Int32 smtpPort, bool smtpUseDefaultCredentials, string smtpUsername, string smtpPassword, bool smtpEnableSsl)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.To.Add(toAddress);
                mail.From = new MailAddress(fromAddress);
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;

                SmtpClient smtp = new SmtpClient();
                smtp.Host = smtpHost;
                smtp.Port = smtpPort;
                smtp.UseDefaultCredentials = smtpUseDefaultCredentials;
                smtp.Credentials = new System.Net.NetworkCredential(smtpUsername, smtpPassword);// Senders User name and password
                smtp.EnableSsl = smtpEnableSsl;

                smtp.Send(mail);
            }
            catch (Exception ex)
            {
                ErrorHandler.WriteError(ex);
            }
        }


        public static bool networkIsAvailable()
        {
            try
            {
                using(var client = new WebClient())
                {
                    using(var stream = client.OpenRead("http://www.google.com"))
                    {
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }           
        }
    }
}
