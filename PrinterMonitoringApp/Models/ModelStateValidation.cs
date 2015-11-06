using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.ModelBinding;

namespace PrinterMonitoringApp
{
    public class ModelStateValidation
    {
        public static string GetErrorListFromModelState(ModelStateDictionary modelState)
        {
            string errors = "";

            var query = from state in modelState.Values
                        from error in state.Errors
                        select error.ErrorMessage;

            var errorList = query.ToList();
            foreach (string error in errorList)
            {
                errors += error.TrimEnd('.') + ", ";
            }
            return errors.TrimEnd(',', ' ');
        }
    }
}