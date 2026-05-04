using System;
using System.Security.Cryptography;
using System.Text;

namespace Cryptography
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //string Data = "Hello, World!";

            // string Signature = ComputeHashing(Data);

            // Console.WriteLine("Data: " + Data);
            // Console.WriteLine("Signature: " + Signature);

            // example Engressing and Degrressing


            string Data = "Hello, World!";
            string Key = "1234567890123456";
            try
            {
                string EncryptedData = Encrypt(Data, Key);
                Console.WriteLine("Encrypted Data: " + EncryptedData);
                string DecryptedData = Decrypt(EncryptedData, Key);
                Console.WriteLine("Decrypted Data: " + DecryptedData);

            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }

        static string Encrypt(string plainText, string key)
        {
            using (Aes aesAlg = Aes.Create())
            {
                // Set the key and IV for AES encryption
                aesAlg.Key = Encoding.UTF8.GetBytes(key);
                aesAlg.IV = new byte[aesAlg.BlockSize / 8];


                // Create an encryptor
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);


                // Encrypt the data
                using (var msEncrypt = new System.IO.MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    using (var swEncrypt = new System.IO.StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(plainText);
                    }


                    // Return the encrypted data as a Base64-encoded string
                    return Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
        }

        static string Decrypt(string cipherText, string key)
        {
            using (Aes aesAlg = Aes.Create())
            {
                // Set the key and IV for AES decryption
                aesAlg.Key = Encoding.UTF8.GetBytes(key);
                aesAlg.IV = new byte[aesAlg.BlockSize / 8];


                // Create a decryptor
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);


                // Decrypt the data
                using (var msDecrypt = new System.IO.MemoryStream(Convert.FromBase64String(cipherText)))
                using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                using (var srDecrypt = new System.IO.StreamReader(csDecrypt))
                {
                    // Read the decrypted data from the StreamReader
                    return srDecrypt.ReadToEnd();
                }
            }
        }

        public static string ComputeHashing(string Data)
        {

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] sing = sha256.ComputeHash(Encoding.UTF8.GetBytes(Data));
               return BitConverter.ToString(sing).Replace("-", "").ToLower();

            }
            return string.Empty;    
        }

    }
}
