using System.Security.Cryptography;
using System.Text;

namespace Breeze.Utilities;

public static class Helper
{
    public static DateTime GetCurrentDate()
    {
        return DateTime.Now;
    }

    public static bool IsNullOrEmpty<T>(T obj)
    {
        return obj is null;
    }

    public static bool IsEmail(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return false;
        }

        try
        {
            var addr = new System.Net.Mail.MailAddress(value);
            return addr.Address == value &&
                addr.Host.Contains('.') &&
                !addr.Host.StartsWith('.') &&
                !addr.Host.EndsWith('.');
        }
        catch
        {
            return false;
        }
    }

    public static string ComputeHmacSha512Hash(string input, string key)
    {
        using (var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(key)))
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] hashBytes = hmac.ComputeHash(inputBytes);
            string hash = Convert.ToBase64String(hashBytes);
            return hash;
        }
    }

    public static string Encrypt(string plaintext, byte[] key, byte[] iv)
    {
        using Aes aes = Aes.Create();
        aes.Key = key;
        aes.IV = iv;
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;

        byte[] encrypted;
        using MemoryStream ms = new();
        using (ICryptoTransform encryptor = aes.CreateEncryptor())
        using (CryptoStream cs = new(ms, encryptor, CryptoStreamMode.Write))
        {
            byte[] plaintextBytes = Encoding.UTF8.GetBytes(plaintext);
            cs.Write(plaintextBytes, 0, plaintextBytes.Length);
        }
        encrypted = ms.ToArray();

        return Convert.ToBase64String(encrypted);
    }

    public static string Decrypt(string ciphertext, byte[] key, byte[] iv)
    {
        byte[] encrypted = Convert.FromBase64String(ciphertext);
        using Aes aes = Aes.Create();
        aes.Key = key;
        aes.IV = iv;
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;

        string plaintext;
        using MemoryStream ms = new(encrypted);
        using (ICryptoTransform decryptor = aes.CreateDecryptor())
        using (CryptoStream cs = new(ms, decryptor, CryptoStreamMode.Read))
        {
            using StreamReader reader = new(cs);
            plaintext = reader.ReadToEnd();
        }

        return plaintext;
    }

    public static int GenerateRandomNumber()
    {
        var rng = new Random();
        return rng.Next(100);
    }
}