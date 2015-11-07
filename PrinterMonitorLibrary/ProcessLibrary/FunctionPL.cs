using PrinterMonitorLibrary.ModelLibrary.EntityFrameworkLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrinterMonitorLibrary
{
    public class FunctionPL
    {
        public FunctionPL()
        {

        }

        public static bool Save(Function function, out string message)
        {
            try
            {
                if (FunctionDL.FunctionExists(function))
                {
                    message = string.Format("Function with name: {0} exists already", function.Name);
                    return false;
                }
                else
                {
                    message = string.Empty;
                    return FunctionDL.Save(function);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool Update(Function function)
        {
            try
            {
                return FunctionDL.Update(function);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<Object> RetrieveFunctions()
        {
            try
            {
                List<Object> returnedFunctions = new List<object>();

                List<Function> functions = FunctionDL.RetrieveFunctions();

                foreach(Function function in functions)
                {
                    Object functionObj = new
                    {
                        ID = function.ID,
                        Name = function.Name,
                        PageLink = function.PageLink
                    };

                    returnedFunctions.Add(functionObj);
                }

                return returnedFunctions; 
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
