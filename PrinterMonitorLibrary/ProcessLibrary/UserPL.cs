using PrinterMonitorLibrary.ModelLibrary.EntityFrameworkLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrinterMonitorLibrary
{
    public class UserPL
    {
        public static bool Save(User user, out string message)
        {
            try
            {
                if (UserDL.UserExists(user))
                {
                    message = string.Format("User with username: {0} exists already", user.Username);
                    return false;
                }
                else
                {
                    message = string.Empty;
                    if (UserDL.Save(user))
                    {
                        return true;
                    }
                    else
                        return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<User> RetrieveUsers()
        {
            try
            {
                return UserDL.RetrieveUsers();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<Object> UsersObject()
        {
            try
            {
                List<User> users = UserDL.RetrieveUsers();
                List<Object> returnedUsers = new List<Object>();
                foreach (User user in users)
                {
                    //Role role = RolePL.RetrieveRoleByID(user.Role);
                    //Branch branch = BranchPL.RetrieveBranchByID(user.Branch);
                    object userObj = new
                    {
                        ID = user.ID,
                        Lastname = user.Lastname,
                        Othernames = user.Othernames,
                        Gender = user.Gender,
                        PhoneNumber = user.PhoneNumber,
                        Email = user.Email,
                        Username = user.Username,
                        CreatedOn = user.CreatedOn,
                        //ModifiedOn = user.ModifiedOn,
                        //Role = role,
                        //Branch = branch
                    };

                    returnedUsers.Add(userObj);
                }
                return returnedUsers;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static User RetrieveUserByID(long? userID)
        {
            try
            {
                return UserDL.RetrieveUserByID(userID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static User RetrieveUserByUsername(string username)
        {
            try
            {
                return UserDL.RetrieveUserByUsername(username);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Object AuthenticateUser(string username, string password)
        {
            try
            {
                User user = UserDL.AuthenticateUser(username, password);
                if (user != null)
                {
                    Object userObj = new Object();
                    //Role role = RolePL.RetrieveRoleByID(user.Role);
                    //List<Function> functions = FunctionPL.GetFunctionsByIDs(role.FunctionID.Split('|'));
                    userObj = new
                    {
                        ID = user.ID,
                        Username = user.Username,
                        FirstTime = user.FirstTime,
                        //Role = role,
                        //Functions = functions
                    };

                    return userObj;
                }
                return user;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool Update(User user)
        {
            try
            {
                return UserDL.Update(user);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool ChangePassword(string username, string password)
        {
            try
            {
                return UserDL.ChangePassword(username, password);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
