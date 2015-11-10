using PrinterMonitorLibrary;
using PrinterMonitorLibrary.ModelLibrary.EntityFrameworkLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PrinterMonitoringApp.Controllers
{
    public class RoleAPIController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage SaveRole([FromBody]Role role)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (role.RoleFunctions.Count.Equals(0))
                        throw new Exception("The Functions field is required.");

                    string errMsg = string.Empty;
                    role.Status = StatusUtil.Status.Active.ToString();
                    bool result = RolePL.Save(role, out errMsg);
                    if (string.IsNullOrEmpty(errMsg))
                        return result.Equals(true) ? Request.CreateResponse(HttpStatusCode.OK, "Role added successfully.") : Request.CreateResponse(HttpStatusCode.BadRequest, "Request failed.");
                    else
                    {
                        var response = Request.CreateResponse(HttpStatusCode.BadRequest, errMsg);
                        return response;
                    }
                }
                else
                {
                    string errors = ModelStateValidation.GetErrorListFromModelState(ModelState);
                    if (role.RoleFunctions.Count.Equals(0))
                        errors += ", The Functions field is required.";
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
        public HttpResponseMessage UpdateRole([FromBody]Role role)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (role.RoleFunctions.Count.Equals(0))
                        throw new Exception("The Functions field is required.");
                    
                    bool result = RolePL.Update(role);
                    return result.Equals(true) ? Request.CreateResponse(HttpStatusCode.OK, "Role updated successfully.") : Request.CreateResponse(HttpStatusCode.BadRequest, "Request failed.");
                }
                else
                {
                    string errors = ModelStateValidation.GetErrorListFromModelState(ModelState);
                    if (role.RoleFunctions.Count.Equals(0))
                        errors += ", The Functions field is required.";
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

        [HttpGet]
        public HttpResponseMessage RetrieveRoles()
        {
            try
            {
                IEnumerable<Object> roles = RolePL.RolesObject();
                object returnedRoles = new { data = roles };
                return Request.CreateResponse(HttpStatusCode.OK, returnedRoles);
            }
            catch (Exception ex)
            {
                ErrorHandler.WriteError(ex);
                var response = Request.CreateResponse(HttpStatusCode.BadRequest);
                response.ReasonPhrase = ex.Message;
                return response;
            }
        }
    }
}
