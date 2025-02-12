using System.Security.Cryptography;
using System.Text;

namespace Employee_Management_System.Utilities
{
    public static class EncryptionHelper
    {
        // Encrypts the plain text using a key.
        public static string EncryptString(string plainText, string key)
        {
            using Aes aesAlg = Aes.Create();
            // Ensure the key is 32 bytes (256 bits); here we pad/truncate as needed.
            aesAlg.Key = Encoding.UTF8.GetBytes(key.PadRight(32).Substring(0, 32));
            aesAlg.GenerateIV();
            aesAlg.Mode = CipherMode.CBC;
            aesAlg.Padding = PaddingMode.PKCS7;

            byte[] iv = aesAlg.IV;

            using MemoryStream msEncrypt = new MemoryStream();
            using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, aesAlg.CreateEncryptor(), CryptoStreamMode.Write))
            {
                byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
                csEncrypt.Write(plainBytes, 0, plainBytes.Length);
                csEncrypt.FlushFinalBlock();
            }
            byte[] cipher = msEncrypt.ToArray();
            // Prepend the IV to the cipher text so it can be used during decryption.
            byte[] combinedIvCt = new byte[iv.Length + cipher.Length];
            Array.Copy(iv, 0, combinedIvCt, 0, iv.Length);
            Array.Copy(cipher, 0, combinedIvCt, iv.Length, cipher.Length);
            return Convert.ToBase64String(combinedIvCt);
        }

        // Decrypts the cipher text using the same key.
        public static string DecryptString(string cipherText, string key)
        {
            byte[] fullCipher = Convert.FromBase64String(cipherText);
            // The first 16 bytes are the IV.
            byte[] iv = new byte[16];
            Array.Copy(fullCipher, 0, iv, 0, iv.Length);
            byte[] cipher = new byte[fullCipher.Length - iv.Length];
            Array.Copy(fullCipher, iv.Length, cipher, 0, cipher.Length);

            using Aes aesAlg = Aes.Create();
            aesAlg.Key = Encoding.UTF8.GetBytes(key.PadRight(32).Substring(0, 32));
            aesAlg.IV = iv;
            aesAlg.Mode = CipherMode.CBC;
            aesAlg.Padding = PaddingMode.PKCS7;

            using MemoryStream msDecrypt = new MemoryStream();
            using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, aesAlg.CreateDecryptor(), CryptoStreamMode.Write))
            {
                csDecrypt.Write(cipher, 0, cipher.Length);
                csDecrypt.FlushFinalBlock();
            }
            return Encoding.UTF8.GetString(msDecrypt.ToArray());
        }
    }
}
