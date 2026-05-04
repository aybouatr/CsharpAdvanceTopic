using System;
using System.Security.Cryptography;
using System.Text;

namespace Cryptography
{
    internal class Program
    {
        //static void Main(string[] args)
        //{
        //    //string Data = "Hello, World!";

        //    // string Signature = ComputeHashing(Data);

        //    // Console.WriteLine("Data: " + Data);
        //    // Console.WriteLine("Signature: " + Signature);

        //    // example Engressing and Degrressing


        //    string Data = "Hello, World!";
        //    string Key = "1234567890123456";
        //    try
        //    {
        //        string EncryptedData = Encrypt(Data, Key);
        //        Console.WriteLine("Encrypted Data: " + EncryptedData);
        //        string DecryptedData = Decrypt(EncryptedData, Key);
        //        Console.WriteLine("Decrypted Data: " + DecryptedData);

        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("An error occurred: " + ex.Message);
        //    }
        //}

        //static string Encrypt(string plainText, string key)
        //{
        //    using (Aes aesAlg = Aes.Create())
        //    {
        //        // Set the key and IV for AES encryption
        //        aesAlg.Key = Encoding.UTF8.GetBytes(key);
        //        aesAlg.IV = new byte[aesAlg.BlockSize / 8];


        //        // Create an encryptor
        //        ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);


        //        // Encrypt the data
        //        using (var msEncrypt = new System.IO.MemoryStream())
        //        {
        //            using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
        //            using (var swEncrypt = new System.IO.StreamWriter(csEncrypt))
        //            {
        //                swEncrypt.Write(plainText);
        //            }


        //            // Return the encrypted data as a Base64-encoded string
        //            return Convert.ToBase64String(msEncrypt.ToArray());
        //        }
        //    }
        //}

        //static string Decrypt(string cipherText, string key)
        //{
        //    using (Aes aesAlg = Aes.Create())
        //    {
        //        // Set the key and IV for AES decryption
        //        aesAlg.Key = Encoding.UTF8.GetBytes(key);
        //        aesAlg.IV = new byte[aesAlg.BlockSize / 8];


        //        // Create a decryptor
        //        ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);


        //        // Decrypt the data
        //        using (var msDecrypt = new System.IO.MemoryStream(Convert.FromBase64String(cipherText)))
        //        using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
        //        using (var srDecrypt = new System.IO.StreamReader(csDecrypt))
        //        {
        //            // Read the decrypted data from the StreamReader
        //            return srDecrypt.ReadToEnd();
        //        }
        //    }
        //}

        //public static string ComputeHashing(string Data)
        //{

        //    using (SHA256 sha256 = SHA256.Create())
        //    {
        //        byte[] sing = sha256.ComputeHash(Encoding.UTF8.GetBytes(Data));
        //       return BitConverter.ToString(sing).Replace("-", "").ToLower();

        //    }
        //    return string.Empty;    
        //}


        static void Main(string[] args)
        {
            try
            {
                // Generate public and private key pair
                using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
                {
                    // Get the public key
                    /*
                     When exporting the public key, ToXmlString(false) is used with the argument set 
                     to false to indicate that only the public parameters should be included in the XML string.
                     */
                    string publicKey = rsa.ToXmlString(false);


                    // Get the private key
                    string privateKey = rsa.ToXmlString(true);


                    // Original message
                    string originalMessage = "Hello, this is a secret message!";


                    // Encrypt using the public key
                    string encryptedMessage = Encrypt(originalMessage, publicKey);


                    // Decrypt using the private key
                    string decryptedMessage = Decrypt(encryptedMessage, privateKey);


                    // Display the results
                    Console.WriteLine($"\n\nPublic Key:\n {publicKey}");
                    Console.WriteLine($"\n\nPrivate Key:\n {privateKey}");
                    Console.WriteLine($"\nOriginal Message:\n {originalMessage}");
                    Console.WriteLine($"\nEncrypted Message:\n {encryptedMessage}");
                    Console.WriteLine($"\nDecrypted Message:\n {decryptedMessage}");


                    // Wait for user input before closing the console window
                    Console.WriteLine("\nPress any key to exit...");
                    Console.ReadKey();
                }
            }
            catch (CryptographicException ex)
            {
                Console.WriteLine($"Encryption/Decryption error: {ex.Message}");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                Console.ReadKey();
            }
        }


        static string Encrypt(string plainText, string publicKey)
        {
            try
            {
                using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
                {
                    rsa.FromXmlString(publicKey);


                    byte[] encryptedData = rsa.Encrypt(Encoding.UTF8.GetBytes(plainText), false);
                    return Convert.ToBase64String(encryptedData);
                }
            }
            catch (CryptographicException ex)
            {
                Console.WriteLine($"Encryption error: {ex.Message}");
                throw; // Rethrow the exception to be caught in the Main method
            }
        }


        static string Decrypt(string cipherText, string privateKey)
        {
            try
            {
                using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
                {
                    rsa.FromXmlString(privateKey);


                    byte[] encryptedData = Convert.FromBase64String(cipherText);
                    byte[] decryptedData = rsa.Decrypt(encryptedData, false);


                    return Encoding.UTF8.GetString(decryptedData);
                }
            }
            catch (CryptographicException ex)
            {
                Console.WriteLine($"Decryption error: {ex.Message}");
                throw; // Rethrow the exception to be caught in the Main method
            }

        }

    }
}
