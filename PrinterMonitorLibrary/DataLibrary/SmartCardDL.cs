using PrinterMonitorLibrary.ModelLibrary.EntityFrameworkLib;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrinterMonitorLibrary
{
    public class SmartCardDL
    {
        public static bool InsertToken(string token, string username, string smartCardID)
        {
            try
            {
                string clearSmartCardID = Crypter.Decrypt(System.Configuration.ConfigurationManager.AppSettings.Get("ekey"), smartCardID);
                var webToken = new CHECK_WEBTOKEN();
                webToken.DateOfrequest = System.DateTime.Now;
                webToken.Token = token;
                webToken.HashedToken = PasswordHash.MD5Hash(token);
                webToken.Username = username;
                webToken.SmartCardID = smartCardID;
                webToken.HashedSmartCardID = PasswordHash.MD5Hash(clearSmartCardID);
                webToken.Status = 0;

                using (var context = new PrinterMonitorDBEntities())
                {
                    // Saves the token
                    context.CHECK_WEBTOKEN.Add(webToken);
                    context.SaveChanges();
                }

                return true;
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
                string scID = PasswordHash.MD5Hash(userSmartCardID);

                using (var context = new PrinterMonitorDBEntities())
                {
                    var webToken = context.CHECK_WEBTOKEN
                                    .Where(t => t.Status == 0 && t.Username.Equals(username) && t.HashedSmartCardID.Equals(scID))
                                    .OrderByDescending(t => t.id)
                                    .FirstOrDefault();

                    if (webToken != null)
                        return webToken;
                    else
                        return null;
                }
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
                CHECK_WEBTOKEN token;
                using (var context = new PrinterMonitorDBEntities())
                {
                    // Query for the token
                    token = context.CHECK_WEBTOKEN
                                    .Where(t => t.id == webToken.id)
                                    .FirstOrDefault();
                }

                //Insert the Encrypted Token against the Token supplied and Update Status to 1
                if (token != null)
                {
                    token.EncyptedToken = Crypter.Encrypt(System.Configuration.ConfigurationManager.AppSettings.Get("ekey"), webToken.Token);

                    token.Status = 1;
                }

                using (var context = new PrinterMonitorDBEntities())
                {
                    context.Entry(token).State = EntityState.Modified;

                    context.SaveChanges();
                }

                return true;
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
                var sc = new SmartCard();
                string hashedSmartID = PasswordHash.MD5Hash(smartCardID);
                using (var context = new PrinterMonitorDBEntities())
                {
                    // Query for the token
                    sc = context.SmartCards
                                    .Where(t => t.HashedSmartCardID == hashedSmartID)
                                    .FirstOrDefault();
                }

                if (sc != null)
                {
                    response = "Smard Card ID has been registered already";
                    return false;
                }
                else
                {
                    var smartCard = new SmartCard();
                    smartCard.Allocated = false;
                    smartCard.EncryptedSmartCardID = Crypter.Encrypt(System.Configuration.ConfigurationManager.AppSettings.Get("ekey"), smartCardID);
                    smartCard.HashedSmartCardID = PasswordHash.MD5Hash(smartCardID);

                    using (var context = new PrinterMonitorDBEntities())
                    {
                        context.SmartCards.Add(smartCard);
                        context.SaveChanges();
                    }

                    response = "Successful";
                    return true;
                }
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
                var webToken = new CHECK_WEBTOKEN();
                string hashedToken = PasswordHash.MD5Hash(token);
                using (var context = new PrinterMonitorDBEntities())
                {
                    webToken = context.CHECK_WEBTOKEN
                                    .Where(t => t.HashedToken.Equals(hashedToken))
                                    .FirstOrDefault();
                }

                if (webToken.Status == 0)
                    return false;
                else
                    return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<SmartCard> GetSmartCards()
        {
            try
            {
                using (var context = new PrinterMonitorDBEntities())
                {
                    var smartCards = context.SmartCards
                                            .Include(sc => sc.Users)
                                            .ToList();

                    return smartCards;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static SmartCard GetSmartCardByID(int ID)
        {
            try
            {
                using (var context = new PrinterMonitorDBEntities())
                {
                    var smartCards = context.SmartCards
                        .Where(s => s.ID == ID)
                        .FirstOrDefault();

                    return smartCards;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static SmartCard GetSmartCardByHashedID(string ID)
        {
            try
            {
                using (var context = new PrinterMonitorDBEntities())
                {
                    string smartCardID = PasswordHash.MD5Hash(ID);
                    var smartCards = context.SmartCards
                        .Where(s => s.HashedSmartCardID == smartCardID)
                        .FirstOrDefault();

                    return smartCards;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<SmartCard> GetSmartCardsNotAllocated()
        {
            try
            {
                using (var context = new PrinterMonitorDBEntities())
                {
                    var smartCards = context.SmartCards
                                     .Where(t => t.Allocated == false);

                    return smartCards.ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<SmartCard> GetSmartCardsAllocated()
        {
            try
            {
                using (var context = new PrinterMonitorDBEntities())
                {
                    var smartCards = context.SmartCards
                                     .Where(t => t.Allocated == true);

                    return smartCards.ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool UpdateSmartCardID(long smartCardID, long userID, bool status)
        {
            try
            {
                var sc = new SmartCard();
                var user = new User();
                using (var context = new PrinterMonitorDBEntities())
                {
                    sc = context.SmartCards
                                    .Where(t => t.ID == smartCardID)
                                    .FirstOrDefault();

                    user = context.Users
                                    .Include(u => u.SmartCard)
                                    .Where(t => t.ID == userID)
                                    .FirstOrDefault();
                }

                if (sc != null && user != null)
                {
                    sc.Allocated = status;

                    if (status)
                    {
                        if (user.SmartCard != null)
                            throw new Exception(string.Format("User {0} has a smart card allocated to it already", user.Username));

                        user.SmartCardID = smartCardID;
                    }
                    else
                    {
                        user.SmartCard = null;
                        sc.Users = null;
                        user.SmartCardID = null;
                    }

                    using (var context = new PrinterMonitorDBEntities())
                    {
                        //Transaction block
                        using (var transaction = context.Database.BeginTransaction())
                        {
                            try
                            {
                                context.Entry(user).State = EntityState.Modified;
                                context.SaveChanges();
                                
                                context.Entry(sc).State = EntityState.Modified;
                                context.SaveChanges();

                                transaction.Commit();
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                throw ex;
                            }
                        }

                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
