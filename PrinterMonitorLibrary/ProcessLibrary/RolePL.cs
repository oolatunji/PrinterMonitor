using PrinterMonitorLibrary.ModelLibrary.EntityFrameworkLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrinterMonitorLibrary
{
    public class RolePL
    {
        public RolePL()
        {

        }

        public static bool Save(Role role, out string message)
        {
            try
            {
                if (RoleDL.RoleExists(role))
                {
                    message = string.Format("Role with name: {0} exists already", role.Name);
                    return false;
                }
                else
                {
                    message = string.Empty;
                    return RoleDL.Save(role);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool Update(Role role)
        {
            try
            {
                return RoleDL.Update(role);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<Role> RetrieveRoles()
        {
            try
            {
                return RoleDL.RetrieveRoles();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Role RetrieveRoleByID(long? roleID)
        {
            try
            {
                return RoleDL.RetrieveRoleByID(roleID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<Role> RolesObject()
        {
            try
            {                              
                return RolePL.RetrieveRoles();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
