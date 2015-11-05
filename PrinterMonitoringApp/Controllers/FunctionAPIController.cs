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
    public class FunctionAPIController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage SaveFunction([FromBody]Function function)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string errMsg = string.Empty;
                    function.Status = StatusUtil.Status.Active.ToString();
                    bool result = FunctionPL.Save(function, out errMsg);
                    if (string.IsNullOrEmpty(errMsg))
                        return result.Equals(true) ? Request.CreateResponse(HttpStatusCode.OK, "Function was added successfully.") : Request.CreateResponse(HttpStatusCode.BadRequest, "Request failed");
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

        [HttpPut]
        public HttpResponseMessage UpdateFunction([FromBody]Function function)
        {
            try
            {
                bool result = FunctionPL.Update(function);
                return result.Equals(true) ? Request.CreateResponse(HttpStatusCode.OK, "Function was updated successfully") : Request.CreateResponse(HttpStatusCode.BadRequest, "Request failed");
            }
            catch (Exception ex)
            {
                var response = Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
                return response;
            }
        }

        [HttpGet]
        public HttpResponseMessage RetrieveFunctions()
        {
            try
            {
                IEnumerable<Function> functions = FunctionPL.RetrieveFunctions();
                object returnedFunctions = new { data = functions };
                return Request.CreateResponse(HttpStatusCode.OK, returnedFunctions);
            }
            catch (Exception ex)
            {
                var response = Request.CreateResponse(HttpStatusCode.BadRequest);
                response.ReasonPhrase = ex.Message;
                return response;
            }
        }
    }
}
