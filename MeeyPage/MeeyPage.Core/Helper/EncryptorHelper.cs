using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Net;
namespace MeeyPage.Core
{
    public class EncryptorHelper
    {
        #region MD5

        public static string Md5Hash(string text)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            //compute hash from the bytes of text
            md5.ComputeHash(Encoding.ASCII.GetBytes(text));

            //get hash result after compute it
            var result = md5.Hash;

            var strBuilder = new StringBuilder();
            foreach (var t in result)
            {
                //change it into 2 hexadecimal digits
                //for each byte
                strBuilder.Append(t.ToString("x2"));
            }

            return strBuilder.ToString();
        }

        #endregion

        #region BCrypt

        public static string BCryptHash(string pw)
        {
            return BCrypt.Net.BCrypt.HashPassword(pw);
        }

        public static bool BCryptVerify(string pw, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(pw, hash);
        }

        #endregion

        #region AES
        // This constant is used to determine the keysize of the encryption algorithm in bits.
        // We divide this by 8 within the code below to get the equivalent number of bytes.
        private const int Keysize = 256;
        private const int BlockSize = 128;

        // This constant determines the number of iterations for the password bytes generation function.
        private const int DerivationIterations = 1000;

        /// <summary>
        /// aes encrypt
        /// </summary>
        /// <param name="plainText">plain text</param>
        /// <param name="AesKey">key</param>
        /// <param name="encode">encode to use as url param</param>
        /// <returns></returns>
        public static string AESEncrypt(string plainText, string AesKey, byte[] AesIV, bool encode = false)
        {
            string cipherText = string.Empty;

            byte[] clearBytes = Encoding.Unicode.GetBytes(plainText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(AesKey, AesIV);
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    cipherText = Convert.ToBase64String(ms.ToArray());
                }
            }

            // encode cipher text to use as url param
            if (encode)
            {
                cipherText = Base64ForUrlEncode(cipherText);
            }

            return cipherText;
        }

        /// <summary>
        /// aes decrypt
        /// </summary>
        /// <param name="plainText">plain text</param>
        /// <param name="AESKey">pass phrase</param>
        /// <param name="encode">encode to use as url param</param>
        /// <returns></returns>
        public static string AESDecrypt(string cipherText, string AesKey, byte[] AesIV, bool encode = false)
        {
            string plainText = string.Empty;

            if (encode)
            {
                cipherText = Base64ForUrlDecode(cipherText);
            }

            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(AesKey, AesIV);
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    plainText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }

            return plainText;
        }

        private static byte[] GenerateRandomEntropy()
        {
            var randomBytes = new byte[BlockSize / 8]; // get bits from block size. Eg: Block size = 128, so 128 / 8 = 16  Bytes will give us 156 bits.
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                // Fill the array with cryptographically secure random bytes.
                rngCsp.GetBytes(randomBytes);
            }
            return randomBytes;
        }

        ///<summary>
        /// Base 64 Encoding with URL and Filename Safe Alphabet using UTF-8 character set.
        ///</summary>
        ///<param name="str">The origianl string</param>
        ///<returns>The Base64 encoded string</returns>
        public static string Base64ForUrlEncode(string str)
        {
            return WebUtility.UrlEncode(str);
        }
        ///<summary>
        /// Decode Base64 encoded string with URL and Filename Safe Alphabet using UTF-8.
        ///</summary>
        ///<param name="str">Base64 code</param>
        ///<returns>The decoded string.</returns>
        public static string Base64ForUrlDecode(string str)
        {
            return WebUtility.UrlDecode(str);
        }

        #endregion
    }
}
