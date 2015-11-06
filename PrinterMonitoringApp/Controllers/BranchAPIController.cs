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
    public class BranchAPIController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage SaveBranch([FromBody]Branch branch)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string errMsg = string.Empty;
                    bool result = BranchPL.Save(branch, out errMsg);
                    if (string.IsNullOrEmpty(errMsg))
                        return result.Equals(true) ? Request.CreateResponse(HttpStatusCode.OK, "Branch was added successfully.") : Request.CreateResponse(HttpStatusCode.BadRequest, "Request failed");
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
        public HttpResponseMessage UpdateBranch([FromBody]Branch branch)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool result = BranchPL.Update(branch);
                    return result.Equals(true) ? Request.CreateResponse(HttpStatusCode.OK, "Branch was updated successfully") : Request.CreateResponse(HttpStatusCode.BadRequest, "Request failed");
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
        public HttpResponseMessage RetrieveBranches()
        {
            try
            {
                IEnumerable<Branch> branches = BranchPL.RetrieveBranches();
                object returnedBranches = new { data = branches };
                return Request.CreateResponse(HttpStatusCode.OK, returnedBranches);
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
