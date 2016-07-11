using PrinterMonitorLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Activation;
using System.Web;
using System.Web.Security;
using System.Web.Services;

namespace PrinterMonitoringApp
{
    /// <summary>
    /// Summary description for PostRecord
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class PostRecord : System.Web.Services.WebService
    {

        [WebMethod]
        public EnrolmentSystemUser ValidateUser(string key, string key_pd)
        {
            var user = new EnrolmentSystemUser();
            try
            {
                if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(key_pd))
                {
                    user.Response = String.Format("{0}|{1}", "Failed", "All the parameters compulsory.");
                }
                else
                {
                    string password = FormsAuthentication.HashPasswordForStoringInConfigFile(key_pd, "MD5");
                    string username = key;

                    var result = UserDL.AuthenticateUser(username, password);

                    if (result == null)
                    {
                        user.Response = String.Format("{0}|{1}", "Failed", "Validation of user failed");
                    }
                    else if (result.ID == 0)
                    {
                        user.Response = String.Format("{0}|{1}", "Failed", "Validation of user failed");
                    }
                    else
                    {
                        user.ID = Convert.ToInt32(result.ID);
                        user.OfficialEmail = result.Email;
                        user.UserName = result.Username;
                        user.UserType = "1";
                        user.Response = String.Format("{0}|{1}", "Success", "Validation of user is successful");
                    }
                }
            }
            catch (Exception ex)
            {
                user.Response = String.Format("{0}|{1}", "Failed", ex.Message);
            }

            return user;
        }

        [WebMethod]
        public bool CheckUser(string userid, string userpassword)
        {
            bool result = false;
            try
            {
                string hashedpassword = FormsAuthentication.HashPasswordForStoringInConfigFile(userpassword, "MD5");

                var user = UserDL.AuthenticateUser(userid, hashedpassword);

                if (user == null)
                {
                    result = false;
                }
                else if (user.ID == 0)
                {
                    result = false;
                }
                else
                {
                    result = true;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;//new ViaCard.Base.PRINTING.PrintDataService().CheckUser(userid, userpassword);
        }
    }
}
