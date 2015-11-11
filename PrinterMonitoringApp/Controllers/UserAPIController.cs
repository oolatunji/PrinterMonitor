using PrinterMonitorLibrary;
using PrinterMonitorLibrary.ModelLibrary.EntityFrameworkLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;

namespace PrinterMonitoringApp.Controllers
{
    public class UserAPIController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage SaveUser([FromBody]User user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //if (Mail.networkIsAvailable())
                    //{
                    string errMsg = string.Empty;
                    user.CreatedOn = System.DateTime.Now;
                    user.FirstTime = true;
                    string password = System.Web.Security.Membership.GeneratePassword(8, 0);
                    user.HashedPassword = PasswordHash.MD5Hash(password);

                    bool result = UserPL.Save(user, out errMsg);
                    if (string.IsNullOrEmpty(errMsg))
                    {
                        if (result)
                        {
                            user.HashedPassword = password;
                            Mail.SendNewUserMail(user);
                            return Request.CreateResponse(HttpStatusCode.OK, "User added successfully.");
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.BadRequest, "Request failed");
                        }
                    }
                    else
                    {
                        var response = Request.CreateResponse(HttpStatusCode.BadRequest, errMsg);
                        return response;
                    }
                    //}
                    //else
                    //{
                    //return Request.CreateResponse(HttpStatusCode.BadRequest, "Kindly ensure that internet connection is available before creating a user");
                    //}
                }
                else
                {
                    string errors = ModelStateValidation.GetErrorListFromModelState(ModelState);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, errors);
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.WriteError(ex);
                var response = Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
                return response;
            }
        }

        [HttpPut]
        public HttpResponseMessage UpdateUser([FromBody]User user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                bool result = UserPL.Update(user);
                return result.Equals(true) ? Request.CreateResponse(HttpStatusCode.OK, "User Updated Successfully.") : Request.CreateResponse(HttpStatusCode.BadRequest, "Failed");
                }
                else
                {
                    string errors = ModelStateValidation.GetErrorListFromModelState(ModelState);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, errors);
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.WriteError(ex);
                var response = Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
                return response;
            }
        }

        [HttpPut]
        public HttpResponseMessage ChangePassword([FromBody]PasswordModel changePassword)
        {
            try
            {
                string password = PasswordHash.MD5Hash(changePassword.Password);
                string username = changePassword.Username;
                bool result = UserPL.ChangePassword(username, password);
                return result.Equals(true) ? Request.CreateResponse(HttpStatusCode.OK, "Successful") : Request.CreateResponse(HttpStatusCode.BadRequest, "Failed");
            }
            catch (Exception ex)
            {
                ErrorHandler.WriteError(ex);
                var response = Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
                return response;
            }
        }

        [HttpPut]
        public HttpResponseMessage ForgotPassword([FromBody]PasswordModel changePassword)
        {
            try
            {
                //if (Mail.networkIsAvailable())
                //{
                    string username = changePassword.Username;
                    User user = UserPL.RetrieveUserByUsername(username);
                    if (user != null)
                    {

                        Mail.SendForgotPasswordMail(user);
                        return Request.CreateResponse(HttpStatusCode.OK, user.Email);
                    }
                    else
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid username");
                //}
                //else
                //{
                    //return Request.CreateResponse(HttpStatusCode.BadRequest, "Kindly ensure that internet connection is available in order to reset your password.");
                //}
            }
            catch (Exception ex)
            {
                ErrorHandler.WriteError(ex);
                var response = Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
                return response;
            }
        }

        [HttpPost]
        public HttpResponseMessage ConfirmUsername([FromBody]PasswordModel changePassword)
        {
            try
            {
                string key = System.Configuration.ConfigurationManager.AppSettings.Get("ekey");
                string username = Crypter.Decrypt(key, changePassword.Username);
                User user = UserPL.RetrieveUserByUsername(username);
                if (user != null)
                {
                    //Add a mail for password reset
                    object response = new
                    {
                        Status = "Successful",
                        Username = user.Username
                    };
                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }
                else
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid username");
            }
            catch (Exception ex)
            {
                ErrorHandler.WriteError(ex);
                var response = Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
                return response;
            }
        }

        [HttpGet]
        public HttpResponseMessage RetrieveUsers()
        {
            try
            {
                IEnumerable<Object> users = UserPL.RetrieveUsers();
                object returnedUsers = new { data = users };
                return Request.CreateResponse(HttpStatusCode.OK, returnedUsers);
            }
            catch (Exception ex)
            {
                ErrorHandler.WriteError(ex);
                var response = Request.CreateResponse(HttpStatusCode.BadRequest);
                response.ReasonPhrase = ex.Message;
                return response;
            }
        }

        [HttpGet]
        public HttpResponseMessage RetrieveUsersWithoutSmartCard()
        {
            try
            {
                IEnumerable<Object> users = UserPL.RetrieveUsersWithoutSmartCard();
                object returnedUsers = new { data = users };
                return Request.CreateResponse(HttpStatusCode.OK, returnedUsers);
            }
            catch (Exception ex)
            {
                ErrorHandler.WriteError(ex);
                var response = Request.CreateResponse(HttpStatusCode.BadRequest);
                response.ReasonPhrase = ex.Message;
                return response;
            }
        }

        [HttpPost]
        public HttpResponseMessage AuthenticateUser([FromBody]PasswordModel passwordModel)
        {
            try
            {
                string password = PasswordHash.MD5Hash(passwordModel.Password);
                dynamic userObj = UserPL.AuthenticateUser(passwordModel.Username, password);
                if (userObj != null)
                {
                    bool useSmartCardAuthentication = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings.Get("UseSmartCardAuthentication"));
                    if (!useSmartCardAuthentication)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, (object)userObj);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(userObj.SmartCard))
                        {
                            string smartCard = userObj.SmartCard;

                            if (!smartCard.Equals("None"))
                            {
                                string token = System.Guid.NewGuid().ToString();

                                bool insertToken = SmartCardPL.InsertToken(token, userObj.Username, smartCard);

                                if (insertToken)
                                {
                                    bool successful = false;
                                    for (int i = 0; i < 6; i++)
                                    {
                                        Thread.Sleep(1000);
                                        bool checkTokenStatus = SmartCardPL.CheckInsertedTokenStatus(token);
                                        if (checkTokenStatus)
                                        {
                                            successful = true;
                                            break;
                                        }
                                    }

                                    if (successful)
                                        return Request.CreateResponse(HttpStatusCode.OK, (object)userObj); 
                                    else
                                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Login failed. Smart Card validation process ended but failed. Kindly check that the smart card has been placed in the smart card machine and try again.");
                                }
                                else
                                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Login failed. Smart Card validation process started but failed.");
                            }
                            else                            
                                return Request.CreateResponse(HttpStatusCode.BadRequest, "Login failed. No assigned smart card ID. Kindly contact your administrator.");
                        }
                        else
                            return Request.CreateResponse(HttpStatusCode.BadRequest, "Login failed. No assigned smart card ID. Kindly contact your administrator");
                    }
                }
                else
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid Username/Password");
            }
            catch (Exception ex)
            {
                ErrorHandler.WriteError(ex);
                var response = Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
                return response;
            }
        }
    }
}
