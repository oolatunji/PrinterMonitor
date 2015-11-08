using PrinterMonitorLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PrinterMonitoringApp.Controllers
{
    public class SmartCardAPIController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage RetrieveSmartCards()
        {
            try
            {
                IEnumerable<Object> smartCards = SmartCardPL.GetSmartCards();
                object returnedSmartCards = new { data = smartCards };
                return Request.CreateResponse(HttpStatusCode.OK, returnedSmartCards);
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
