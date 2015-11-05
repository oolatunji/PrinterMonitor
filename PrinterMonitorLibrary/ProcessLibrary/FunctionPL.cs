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

        public static List<Function> RetrieveFunctions()
        {
            try
            {
                return FunctionDL.RetrieveFunctions();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Function RetrieveFunctionByID(long functionID)
        {
            try
            {
                return FunctionDL.RetrieveFunctionByID(functionID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<Function> GetFunctionsByIDs(string[] functionIDs)
        {
            try
            {
                List<Function> functions = new List<Function>();

                foreach (string functionID in functionIDs)
                {
                    if (!string.IsNullOrEmpty(functionID))
                    {
                        Function function = FunctionPL.RetrieveFunctionByID(Convert.ToInt64(functionID));

                        functions.Add(function);
                    }
                }

                return functions;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
