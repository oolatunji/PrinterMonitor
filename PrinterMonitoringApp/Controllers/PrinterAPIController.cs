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
    public class PrinterAPIController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage SavePrinter([FromBody]Printer printer)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string errMsg = string.Empty;
                    printer.DateofEnrollment = System.DateTime.Now;
                    bool result = PrinterPL.Save(printer, out errMsg);
                    if (string.IsNullOrEmpty(errMsg))
                        return result.Equals(true) ? Request.CreateResponse(HttpStatusCode.OK, "Printer added successfully.") : Request.CreateResponse(HttpStatusCode.BadRequest, "Request failed");
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
        public HttpResponseMessage UpdatePrinter([FromBody]Printer printer)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool result = PrinterPL.Update(printer);
                    return result.Equals(true) ? Request.CreateResponse(HttpStatusCode.OK, "Printer updated successfully") : Request.CreateResponse(HttpStatusCode.BadRequest, "Request failed");
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
        public HttpResponseMessage RetrievePrinters()
        {
            try
            {
                IEnumerable<Object> printers = PrinterPL.RetrievePrinters();
                object returnedPrinters = new { data = printers };
                return Request.CreateResponse(HttpStatusCode.OK, returnedPrinters);
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
