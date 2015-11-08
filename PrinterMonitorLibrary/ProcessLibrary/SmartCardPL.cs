using PrinterMonitorLibrary.ModelLibrary.EntityFrameworkLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrinterMonitorLibrary
{
    public class SmartCardPL
    {
        public static bool InsertToken(string token, string username, string smartCardID)
        {
            try
            {
                return SmartCardDL.InsertToken(token, username, smartCardID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static CHECK_WEBTOKEN CheckNewToken(string username, string userSmartCardID)
        {
            try
            {
                return SmartCardDL.CheckNewToken(username, userSmartCardID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool InsertEncrptedToken(CHECK_WEBTOKEN webToken)
        {
            try
            {
                return SmartCardDL.InsertEncrptedToken(webToken);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool InsertSmartCardID(string smartCardID, out string response)
        {
            try
            {
                return SmartCardDL.InsertSmartCardID(smartCardID, out response);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool CheckInsertedTokenStatus(string token)
        {
            try
            {
                return SmartCardDL.CheckInsertedTokenStatus(token);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<Object> GetSmartCards()
        {
            try
            {
                List<Object> returnedSmartCards = new List<object>();

                List<SmartCard> smartCards = SmartCardDL.GetSmartCards();

                foreach(SmartCard smartCard in smartCards)
                {
                    Object smartCardObj = new
                    {
                        ID = smartCard.ID,
                        SmartCardID = Crypter.Decrypt(System.Configuration.ConfigurationManager.AppSettings.Get("ekey"), smartCard.EncryptedSmartCardID),
                        HashedSmartCardID = smartCard.HashedSmartCardID,
                        Allocated = smartCard.Allocated,
                        User = new {
                            TheUser = smartCard.Users.Count != 0 ? string.Format("{1} {2} [{0}]", smartCard.Users.FirstOrDefault().Username, smartCard.Users.FirstOrDefault().Lastname, smartCard.Users.FirstOrDefault().Othernames) : "None",
                            ID = smartCard.Users.Count != 0 ? smartCard.Users.FirstOrDefault().ID : 0
                        }
                    };

                    returnedSmartCards.Add(smartCardObj);
                }

                return returnedSmartCards;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Object GetSmartCardByID(int ID)
        {
            try
            {
                SmartCard smartCard = SmartCardDL.GetSmartCardByID(ID);

                Object smartCardObj = new
                {
                    ID = smartCard.ID,
                    SmartCardID = Crypter.Decrypt(System.Configuration.ConfigurationManager.AppSettings.Get("ekey"), smartCard.EncryptedSmartCardID),
                    HashedSmartCardID = smartCard.HashedSmartCardID,
                    Allocated = smartCard.Allocated
                };

                return smartCardObj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Object GetSmartCardByHashedID(string ID)
        {
            try
            {
                SmartCard smartCard = SmartCardDL.GetSmartCardByHashedID(ID);

                Object smartCardObj = new
                {
                    ID = smartCard.ID,
                    SmartCardID = Crypter.Decrypt(System.Configuration.ConfigurationManager.AppSettings.Get("ekey"), smartCard.EncryptedSmartCardID),
                    HashedSmartCardID = smartCard.HashedSmartCardID,
                    Allocated = smartCard.Allocated
                };

                return smartCardObj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<Object> GetSmartCardsNotAllocated()
        {
            try
            {
                List<Object> returnedSmartCards = new List<object>();

                List<SmartCard> smartCards = SmartCardDL.GetSmartCardsNotAllocated();

                foreach (SmartCard smartCard in smartCards)
                {
                    Object smartCardObj = new
                    {
                        ID = smartCard.ID,
                        SmartCardID = Crypter.Decrypt(System.Configuration.ConfigurationManager.AppSettings.Get("ekey"), smartCard.EncryptedSmartCardID),
                        HashedSmartCardID = smartCard.HashedSmartCardID,
                        Allocated = smartCard.Allocated
                    };

                    returnedSmartCards.Add(smartCardObj);
                }

                return returnedSmartCards;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<Object> GetSmartCardsAllocated()
        {
            try
            {
                List<Object> returnedSmartCards = new List<object>();

                List<SmartCard> smartCards = SmartCardDL.GetSmartCardsAllocated();

                foreach (SmartCard smartCard in smartCards)
                {
                    Object smartCardObj = new
                    {
                        ID = smartCard.ID,
                        SmartCardID = Crypter.Decrypt(System.Configuration.ConfigurationManager.AppSettings.Get("ekey"), smartCard.EncryptedSmartCardID),
                        HashedSmartCardID = smartCard.HashedSmartCardID,
                        Allocated = smartCard.Allocated
                    };

                    returnedSmartCards.Add(smartCardObj);
                }

                return returnedSmartCards;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool UpdateSmartCardID(string smartCardID, bool status)
        {
            try
            {
                return SmartCardDL.UpdateSmartCardID(smartCardID, status);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool UserExists(string username, string password)
        {
            try
            {
                var user = new User();

                user = UserDL.AuthenticateUser(username, PasswordHash.MD5Hash(password));

                if (user == null)
                    return false;
                else
                    return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
