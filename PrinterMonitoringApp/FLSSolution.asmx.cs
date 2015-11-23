using PrinterMonitorLibrary.ModelLibrary.EntityFrameworkLib;
using PrinterMonitorLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Activation;
using System.Web;
using System.Web.Services;

namespace PrinterMonitoringApp
{
    /// <summary>
    /// Summary description for FLSSolution
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class FLSSolution : System.Web.Services.WebService
    {
        [WebMethod]
        public TokenResponse CheckNewToken(string username, string userSmartCardID)
        {
            TokenResponse response = new TokenResponse();
            try
            {
                CHECK_WEBTOKEN webToken = SmartCardPL.CheckNewToken(username, userSmartCardID);
                if (webToken != null)
                {
                    response.ErrMessage = "";
                    response.Successful = true;
                    response.token = webToken;
                }
                else
                {
                    response.ErrMessage = string.Format("No Token found for user, {0} with the supplied smartcardID, {1}.", username, userSmartCardID);
                    response.Successful = false;
                    response.token = null;
                }

                return response;
            }
            catch (Exception ex)
            {
                response.ErrMessage = ex.Message;
                response.Successful = false;
                response.token = null;

                ErrorHandler.WriteError(ex);

                return response;
            }
        }

        [WebMethod]
        public Response InsertEncryptedToken(CHECK_WEBTOKEN webToken)
        {
            Response response = new Response();
            try
            {
                if (SmartCardPL.InsertEncrptedToken(webToken))
                {
                    response.ErrMessage = string.Empty;
                    response.Successful = true;
                }
                else
                {
                    response.ErrMessage = "InsertEncryptedToken operation was not successful.";
                    response.Successful = false;
                }

                return response;
            }
            catch (Exception ex)
            {
                response.ErrMessage = ex.Message;
                response.Successful = false;

                ErrorHandler.WriteError(ex);

                return response;
            }
        }

        [WebMethod]
        public Response InsertSmartCardID(string smartCardID)
        {
            Response response = new Response();
            try
            {
                string errMsg = string.Empty;
                if (SmartCardPL.InsertSmartCardID(smartCardID, out errMsg))
                {
                    response.ErrMessage = string.Empty;
                    response.Successful = true;
                }
                else
                {
                    response.ErrMessage = errMsg;
                    response.Successful = false;
                }

                return response;
            }
            catch (Exception ex)
            {
                response.ErrMessage = ex.Message;
                response.Successful = false;

                ErrorHandler.WriteError(ex);

                return response;
            }
        }

        [WebMethod]
        public Response CheckInsertedTokenStatus(string token)
        {
            Response response = new Response();
            try
            {
                if (SmartCardPL.CheckInsertedTokenStatus(token))
                {
                    response.ErrMessage = string.Empty;
                    response.Successful = true;
                }
                else
                {
                    response.ErrMessage = "No inserted token found";
                    response.Successful = false;
                }

                return response;
            }
            catch (Exception ex)
            {
                response.ErrMessage = ex.Message;
                response.Successful = false;

                ErrorHandler.WriteError(ex);

                return response;
            }
        }

        [WebMethod]
        public string UserExists(string username, string password)
        {
            try
            {
                if (UserPL.UserExists(username, password))
                {
                    return "True";
                }
                else
                    return "False";
            }
            catch(Exception ex)
            {
                ErrorHandler.WriteError(ex);
                return "False";
            }
        }

        [WebMethod]
        public Response SendLatestPrinterFeeds(string printerUID, string printerSerialNumber, int ribbonCount, int noOfCardsPrinted, bool printerOnline)
        {
            Response response = new Response();
            try
            {
                if (string.IsNullOrEmpty(printerSerialNumber))
                {
                    response.ErrMessage = "Operation failed: Printer Serial Number Cannot be Empty or Null";
                    response.Successful = false;
                }
                else
                {
                    PrinterFeed printerFeed = new PrinterFeed();
                    printerFeed.PrinterUID = printerUID;
                    printerFeed.PrinterSerialNumber = printerSerialNumber;
                    printerFeed.RibbonCount = ribbonCount;
                    printerFeed.CardPrinted = noOfCardsPrinted;
                    printerFeed.Status = printerOnline;
                    printerFeed.DateofReport = System.DateTime.Now;

                    if (PrinterFeedsPL.Save(printerFeed))
                    {
                        response.ErrMessage = string.Empty;
                        response.Successful = true;
                    }
                    else
                    {
                        response.ErrMessage = "Operation failed: " + printerSerialNumber;
                        response.Successful = false;
                    }
                }

                return response;
            }
            catch (Exception ex)
            {
                response.ErrMessage = ex.Message;
                response.Successful = false;

                ErrorHandler.WriteError(ex);

                return response;
            }
        }
    }
}
