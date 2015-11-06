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

        public static List<Object> RetrieveBranches()
        {
            try
            {
                List<Object> returnedBranches = new List<object>();

                List<Branch> branches = BranchDL.RetrieveBranches();

                foreach(Branch branch in branches)
                {
                    Object branchObj = new
                    {
                        ID = branch.ID,
                        Name = branch.Name,
                        Code = branch.Code,
                        Address = branch.Address
                    };

                    returnedBranches.Add(branchObj);
                }

                return returnedBranches;
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
