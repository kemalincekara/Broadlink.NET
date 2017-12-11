using System;
using System.IO;
using System.Security.Cryptography;
namespace Broadlink.NET
{
    public static class EncryptionExtensions
    {
        public static string MD5Hash(this string text) => text.GetBytesUTF8().MD5Hash();
        public static string MD5Hash(this byte[] array) => BitConverter.ToString(MD5.Create().ComputeHash(array)).Replace("-", "");


        public static readonly byte[] IV = new byte[] { 0x56, 0x2e, 0x17, 0x99, 0x6d, 0x09, 0x3d, 0x28, 0xdd, 0xb3, 0xba, 0x69, 0x5a, 0x2e, 0x6f, 0x58 };
        public static readonly byte[] InitialKey = new byte[] { 0x09, 0x76, 0x28, 0x34, 0x3f, 0xe9, 0x9e, 0x23, 0x76, 0x5c, 0x15, 0x13, 0xac, 0xcf, 0x8b, 0x02 };

        public static byte[] Encrypt(this byte[] input, byte[] encryptionKey = null)
        {
            using (var aes = new RijndaelManaged()
            {
                KeySize = 128,
                BlockSize = 128,
                Mode = CipherMode.CBC,
                Padding = PaddingMode.None,
                IV = IV,
                Key = encryptionKey ?? InitialKey
            })
            using (MemoryStream ms = new MemoryStream())
            using (CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(aes.Key, aes.IV), CryptoStreamMode.Write))
            {
                cs.Write(input, 0, input.Length);
                if (!cs.HasFlushedFinalBlock)
                    cs.FlushFinalBlock();
                return ms.ToArray();
            }
        }

        public static byte[] Decrypt(this byte[] input, byte[] encryptionKey = null)
        {
            using (var aes = new RijndaelManaged()
            {
                KeySize = 128,
                BlockSize = 128,
                Mode = CipherMode.CBC,
                Padding = PaddingMode.None,
                IV = IV,
                Key = encryptionKey ?? InitialKey
            })
            using (MemoryStream ms = new MemoryStream())
            using (CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(aes.Key, aes.IV), CryptoStreamMode.Write))
            {
                cs.Write(input, 0, input.Length);
                if (!cs.HasFlushedFinalBlock)
                    cs.FlushFinalBlock();
                return ms.ToArray();
            }
        }
    }
}