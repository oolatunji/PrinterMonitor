using PrinterMonitorLibrary.ModelLibrary.EntityFrameworkLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrinterMonitorLibrary
{
    public class BranchPL
    {
        public BranchPL()
        {

        }

        public static bool Save(Branch branch, out string message)
        {
            try
            {
                if (BranchDL.BranchExists(branch))
                {
                    message = string.Format("Branch with name: {0} or code: {1} exists already", branch.Name, branch.Code);
                    return false;
                }
                else
                {
                    message = string.Empty;
                    return BranchDL.Save(branch);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool Update(Branch branch)
        {
            try
            {
                return BranchDL.Update(branch);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<Branch> RetrieveBranches()
        {
            try
            {
                return BranchDL.RetrieveBranches();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Branch RetrieveBranchByID(long? branchID)
        {
            try
            {
                return BranchDL.RetrieveBranchByID(branchID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }        
    }
}
