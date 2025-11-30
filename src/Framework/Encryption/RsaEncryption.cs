using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace Framework.Encryption;

public class RsaEncryption
{
    private static RSACryptoServiceProvider rsa = new();
    private string _publicKey = "";
    private string _privateKey = "";

    public RsaEncryption()
    {
        string _xmlPath = Path.Combine(Directory.GetCurrentDirectory(), "Keys", "RSAKeys.xml");
        string xmlFile = File.ReadAllText(_xmlPath);
        XmlDocument xmldoc = new();
        xmldoc.LoadXml(xmlFile);

        XmlNodeList nodeListPrivate = xmldoc.GetElementsByTagName("private");
        foreach (XmlNode node in nodeListPrivate)
        {
            _privateKey = node.InnerXml.ToString();
        }

        XmlNodeList nodeListPublic = xmldoc.GetElementsByTagName("public");
        foreach (XmlNode node in nodeListPublic)
        {
            _publicKey = node.InnerXml.ToString();
        }
    }

    public string Encryption(string strText)
    {
        if (string.IsNullOrWhiteSpace(strText)) return "";

        int length = strText.Length;
        if (length % 2 != 0)
        {
            strText += "!";
        }

        var testData = Encoding.UTF8.GetBytes(strText);
        using var rsa = new RSACryptoServiceProvider(1024);
        try
        {
            rsa.FromXmlString(_publicKey.ToString());
            var encryptedData = rsa.Encrypt(testData, true);
            var base64Encrypted = Convert.ToBase64String(encryptedData);
            return base64Encrypted;
        }
        catch
        {
            return "";
        }
        finally
        {
            rsa.PersistKeyInCsp = false;
        }
    }

    public string Decryption(string strText)
    {
        if (string.IsNullOrWhiteSpace(strText)) return "";

        using var rsa = new RSACryptoServiceProvider(1024);
        try
        {
            var base64Encrypted = strText;

            rsa.FromXmlString(_privateKey);
            var resultBytes = Convert.FromBase64String(base64Encrypted);
            var decryptedBytes = rsa.Decrypt(resultBytes, true);
            var decryptedData = Encoding.UTF8.GetString(decryptedBytes);

            char lastCharacter = decryptedData.Last();
            if (lastCharacter == '!')
            {
                decryptedData = decryptedData.Remove(decryptedData.Length - 1, 1);
            }

            return decryptedData.ToString();
        }
        catch
        {
            return "";
        }
        finally
        {
            rsa.PersistKeyInCsp = false;
        }
    }
}