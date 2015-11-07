using PrinterMonitorLibrary.ModelLibrary.EntityFrameworkLib;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrinterMonitorLibrary
{
    public class RoleDL
    {

        public static bool Save(Role role)
        {
            try
            {
                using (var context = new PrinterMonitorDBEntities())
                {
                    context.Roles.Add(role);
                    context.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool RoleExists(Role role)
        {
            try
            {
                var existingRole = new Role();
                using (var context = new PrinterMonitorDBEntities())
                {
                    existingRole = context.Roles
                                    .Where(t => t.Name.Equals(role.Name))
                                    .FirstOrDefault();
                }

                if (existingRole == null)
                    return false;
                else
                    return true;
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
                using (var context = new PrinterMonitorDBEntities())
                {
                    var roles = context.Roles
                                .Include(r => r.RoleFunctions.Select(rf => rf.Function))
                                .ToList();

                    return roles;
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
                Role existingRole = new Role();
                using (var context = new PrinterMonitorDBEntities())
                {
                    existingRole = context.Roles
                                    .Where(t => t.ID == role.ID)
                                    .FirstOrDefault();
                }

                if (existingRole != null)
                {
                    existingRole.Name = role.Name;

                    using (var context = new PrinterMonitorDBEntities())
                    {
                        //Transaction block
                        using (var transaction = context.Database.BeginTransaction())
                        {
                            try
                            {
                                //Modifying just the property details
                                context.Entry(existingRole).State = EntityState.Modified;
                                context.SaveChanges();

                                //Delete existing role function of the role
                                IEnumerable<RoleFunction> existingRoleFunctions = context.RoleFunctions.Include("Role")
                                                                .Where(t => existingRole.ID.Equals(t.RoleID))
                                                                .ToList();

                                if (existingRoleFunctions != null && existingRoleFunctions.ToList().Count != 0)
                                {
                                    context.RoleFunctions.RemoveRange(existingRoleFunctions);
                                    context.SaveChanges();
                                }

                                //Adding new Role Functions
                                List<RoleFunction> newRoleFunctions = new List<RoleFunction>();
                                foreach (RoleFunction function in role.RoleFunctions)
                                {
                                    RoleFunction roleFunction = new RoleFunction();
                                    roleFunction.RoleID = existingRole.ID;
                                    roleFunction.FunctionID = function.FunctionID;

                                    newRoleFunctions.Add(roleFunction);
                                }
                                if (newRoleFunctions != null && newRoleFunctions.Count != 0)
                                {
                                    context.RoleFunctions.AddRange(newRoleFunctions);
                                    context.SaveChanges();
                                }

                                //commit changes
                                transaction.Commit();
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                throw ex;
                            }
                        }

                    }

                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
