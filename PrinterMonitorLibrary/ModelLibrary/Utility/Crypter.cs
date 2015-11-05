using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrinterMonitorLibrary
{
    public class Crypter
    {
        public static string Decrypt(string key, string encryptedValue)
        {
            string value = string.Empty;
            try
            {

                RijndaelEnhanced rijndaelKey = new RijndaelEnhanced(key);
                value = rijndaelKey.Decrypt(encryptedValue);
            }
            catch { throw; }
            return value;
        }

        public static string Encrypt(string key, string value)
        {
            string encryptedValue = string.Empty;
            try
            {

                RijndaelEnhanced rijndaelKey = new RijndaelEnhanced(key);
                // string hashValue = HashEncryption.SHA256Hash(value);
                encryptedValue = rijndaelKey.Encrypt(value);
            }
            catch { throw; }
            return encryptedValue;
        }

        public static string Mask(string value)
        {
            if (value.Length > 6)
            {
                string firstpart = value.Substring(0, 6);
                string secondpart = value.Substring(value.Length - 4, 4);
                int otherslen = value.Length - firstpart.Length - secondpart.Length;
                for (int i = 0; i < otherslen; i++)
                {
                    firstpart = firstpart.Trim();
                    firstpart += "*";

                }

                return firstpart.Trim() + secondpart.Trim();
            }
            return value;
        }

        public static string MaskAccount(string value)
        {
            if (value.Length > 3)
            {
                string firstpart = value.Substring(0, 3);
                string secondpart = value.Substring(value.Length - 3, 3);
                int otherslen = value.Length - firstpart.Length - secondpart.Length;
                for (int i = 0; i < otherslen; i++)
                {
                    firstpart = firstpart.Trim();
                    firstpart += "*";

                }

                return firstpart.Trim() + secondpart.Trim();
            }
            return value;
        }
    }
}
