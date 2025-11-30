using System.Security.Cryptography;
using System.Text;

namespace Framework.Encryption;

public static class Crypt
{
    private static readonly string Key = "1404-!@#www.Inventory.com!@#-2025-asrin.com";

    public static string Encrypt(string clearText)
    {
        if (string.IsNullOrWhiteSpace(clearText)) return "";

        string EncryptionKey = Key;
        byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);

        using (Aes encryptor = Aes.Create())
        {
            Rfc2898DeriveBytes pdb = new(EncryptionKey, new byte[] { 73, 118, 97, 110, 32, 77, 101, 100, 118, 101, 100, 101, 118 });
            encryptor.Key = pdb.GetBytes(32);
            encryptor.IV = pdb.GetBytes(16);
            using MemoryStream ms = new();
            using (CryptoStream cs = new(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
            {
                cs.Write(clearBytes, 0, clearBytes.Length);
                cs.Close();
            }
            clearText = Convert.ToBase64String(ms.ToArray());
        }
        return clearText;
    }

    public static string Decrypt(string? cipherText)
    {
        if (string.IsNullOrWhiteSpace(cipherText)) return "";

        cipherText = cipherText!.Replace("%2F", "/");
        string EncryptionKey = Key;
        byte[] cipherBytes = Convert.FromBase64String(cipherText.Replace(" ", ""));

        using (Aes encryptor = Aes.Create())
        {
            Rfc2898DeriveBytes pdb = new(EncryptionKey, new byte[] { 73, 118, 97, 110, 32, 77, 101, 100, 118, 101, 100, 101, 118 });
            encryptor.Key = pdb.GetBytes(32);
            encryptor.IV = pdb.GetBytes(16);
            using MemoryStream ms = new();
            using (CryptoStream cs = new(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
            {
                cs.Write(cipherBytes, 0, cipherBytes.Length);
                cs.Close();
            }
            cipherText = Encoding.Unicode.GetString(ms.ToArray());
        }
        return cipherText;
    }
}