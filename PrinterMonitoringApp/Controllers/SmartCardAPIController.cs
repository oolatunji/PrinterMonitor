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
                ErrorHandler.WriteError(ex);
                var response = Request.CreateResponse(HttpStatusCode.BadRequest);
                response.ReasonPhrase = ex.Message;
                return response;
            }
        }

        [HttpPut]
        public HttpResponseMessage UpdateSmartCard([FromBody]SmartCardModel smartCard)
        {
            try
            {

                bool result = SmartCardPL.UpdateSmartCardID(smartCard.SmartCardID, smartCard.UserID, smartCard.Status);

                string message = "";
                if (smartCard.Status)
                    message = "Smart Card allocated successfully";
                else
                    message = "Smart Card de-allocated successfully";

                return result.Equals(true) ? Request.CreateResponse(HttpStatusCode.OK, message) : Request.CreateResponse(HttpStatusCode.BadRequest, "Request failed");

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
